using Take.UI.MVC.ToolsDashboard.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard
{
    public class Util
    {
        private AppSettings _appSettings;
        public Util(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }      
    }
}
