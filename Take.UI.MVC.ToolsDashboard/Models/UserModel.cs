using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Models
{
    public class UserModel
    {
        [Key]
        public int idUser { get; set; }

        [Required]
        public string email { get; set; }

        [Required]
        public string password { get; set; }
        
        [Required]
        public string name { get; set; }

        public DateTime dateTimeInclusion { get; set; }
    }
}
