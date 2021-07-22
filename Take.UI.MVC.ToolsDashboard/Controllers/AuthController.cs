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
using Take.UI.MVC.ToolsDashboard.Request;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    
    public class AuthController : BaseController
    {

        private readonly AppSettings _appSettings;

        public AuthController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        public IActionResult Index()
        {
            if (User.Identity.IsAuthenticated)
                return RedirectToAction("Index", "Home");
            return View();
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(LoginRequest loginRequest)
        {
            try
            {
                Encryption encryption = new Encryption();

                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from user in bank.User
                                 where user.isDeleted == false
                                 && user.login == loginRequest.login                               
                                 select user).SingleOrDefault();

                    if (query == null)
                    {
                        ShowNotification(NotificationType.Error, "Usuário e senha incorretos", 10);
                        return View();
                    }

                    if (encryption.EncryptString(loginRequest.password) != query.password)
                    {
                        ShowNotification(NotificationType.Error, "Usuário e senha incorretos", 10);
                        return View();
                    }

                    if (!query.isActive)
                    {
                        ShowNotification(NotificationType.Error, "Este usuário está desativado, favor comunicar com o administrador", 10);
                        return View();
                    }

                    // User Login
                    Login(query.userName, query.idUser);

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

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult GitHubLogin()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return Challenge(new AuthenticationProperties() { RedirectUri = "/" });

                return RedirectToAction("Index", "Home");
            }
            catch(Exception ex)
            {
                ShowNotification(NotificationType.Error, ex.Message);
                return View("Index","Auth");
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
                new Claim(ClaimTypes.Name, name, ClaimValueTypes.String),
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