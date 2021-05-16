using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{

    public class HomeController : BaseController
    {
        private readonly AppSettings _appSettings;

        public HomeController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;

        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            {
                if (!User.Identity.IsAuthenticated)
                    return RedirectToAction("Index", "Auth");

                return View();
            }
            catch (Exception e)
            {
                ShowNotification(NotificationType.Error, e.Message);
                return View();
            }

        }
    }
}
