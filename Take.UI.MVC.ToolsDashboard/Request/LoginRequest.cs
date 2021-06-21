using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Request
{
    public class LoginRequest
    {
        [Required]
        public string login { get; set; }

        [Required]
        public string password { get; set; }
    }
}
