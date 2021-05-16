using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Take.UI.MVC.ToolsDashboard.Models;
using Microsoft.Extensions.Options;
using System.Net.Http;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    
    public class AuthController : BaseController
    {
        private readonly Endpoints _endpoints;

        public AuthController(IOptions<Endpoints> endpoints)
        {
            _endpoints = endpoints.Value;

        }
        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public async Task<IActionResult> Index(User userModelAdm)
        {
            try
            {

                if (!User.Identity.IsAuthenticated)
                    return Challenge(new AuthenticationProperties() { RedirectUri = "/" });

                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(_endpoints.ServiceUserAdm + $"Authentication/email/{userModelAdm.email}/password/{userModelAdm.password}");

                    // If Successful
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(responseString);
                    }

                    var model = JsonConvert.DeserializeObject<User>(responseString);

                    // User Login
                    Login(model.name, model.idUser);

                    // Return Success
                    return RedirectToAction("Index", "Home");
                }
            }
            catch (Exception e)
            {
                ShowNotification(NotificationType.Error, e.Message);
                return View();
            }
        }

        #region [ POST  Logout ]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Auth");
        }
        #endregion

        #region [ Metodos Privados ]
        private async void Login(string name, int idUser)
        {
            var claims = new List<Claim> {
                new Claim(ClaimTypes.NameIdentifier, idUser.ToString(), ClaimValueTypes.Integer),
                new Claim("nome", name, ClaimValueTypes.String),
                new Claim("http://schemas.microsoft.com/accesscontrolservice/2010/07/claims/identityprovider", "ASP.NET Identity", "http://www.w3.org/2001/XMLSchema#string"),
            };

            var userIdentity = new ClaimsIdentity(claims, "TakeDashboard");

            var userPrincipal = new ClaimsPrincipal(userIdentity);

            var authenticationProperties = new Microsoft.AspNetCore.Authentication.AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(20),
                IsPersistent = true,
                AllowRefresh = false
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, userPrincipal);
        }
        #endregion
    }
}