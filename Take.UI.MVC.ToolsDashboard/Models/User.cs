using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Models
{
    [Table("user")]
    public class User
    {
        [Key]
        [Display(Name = "ID de usuário")]
        public int idUser { get; set; }

        [Display(Name = "Nome")]
        public string userName { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        public string login { get; set; }

        public string password { get; set; }

        public bool isMaster { get; set; }

        public bool isDeleted { get; set; }

        public DateTime dateTimeInclusion { get; set; }
    }
}
