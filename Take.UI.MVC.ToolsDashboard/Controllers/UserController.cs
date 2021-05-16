using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Take.UI.MVC.ToolsDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    public class UserController : BaseController
    {

        private readonly Endpoints _endpoints;

        public UserController(IOptions<Endpoints> endpoints)
        {
            _endpoints = endpoints.Value;
        }

        public async Task<IActionResult> Index()
        {

            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(_endpoints.ServiceUser + $"User");

                    // If Successful
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(responseString);
                    }

                    List<User> model = JsonConvert.DeserializeObject<List<User>>(responseString);
                    ViewBag.listUserModelmeucheckin = model;

                    return View();
                }
            }
            catch (Exception e)
            {
                ShowNotificationRedirect(NotificationType.Error, e.Message);
                return RedirectToAction("Index", "Home");
            }
        }

        public async Task<IActionResult> Details(int id)
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = await client.GetAsync(_endpoints.ServiceUser + $"User/idUser/{id}");

                    // If Successful
                    var responseString = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        throw new Exception(responseString);
                    }

                    User model = JsonConvert.DeserializeObject<User>(responseString);
                    ViewBag.UserMeucheckin = model;

                    return View(model);
                }
            }
            catch (Exception e)
            {
                ShowNotification(NotificationType.Error, e.Message);
                return View();
            }
        }
    }
}