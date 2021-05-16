using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace Front.Adm.Controllers
{
    public class BillingHotelController : BaseController
    {
        private readonly Endpoints _endpoints;

        public BillingHotelController(IOptions<Endpoints> endpoints)
        {
            _endpoints = endpoints.Value;

        }

        public IActionResult Index()
        {
            return View();
        }
    }
}