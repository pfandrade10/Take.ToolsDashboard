using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    [Authorize]
    public class HomeController : BaseController
    {
        private readonly Endpoints _endpoints;

        public HomeController(IOptions<Endpoints> endpoints)
        {
            _endpoints = endpoints.Value;

        }

        [HttpGet]
        public IActionResult Index()
        {
            try
            { 
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
