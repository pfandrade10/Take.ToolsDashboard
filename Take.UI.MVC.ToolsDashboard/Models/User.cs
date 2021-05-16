using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Models
{
    public class User
    {
        [Display(Name = "ID de usuário")]
        public int idUser { get; set; }

        public string keyFacebook { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }

        public string password { get; set; }

        [Display(Name = "Nome")]
        public string name { get; set; }

        public string lastName { get; set; }

        public string ddiCellNumber { get; set; }

        [Display(Name = "Telefone")]
        public string cellNumber { get; set; }

        public string address { get; set; }

        public string city { get; set; }

        public string country { get; set; }

        public string cpf { get; set; }

        public string postalCode { get; set; }

        public DateTime dateTimeInclusion { get; set; }
    }
}
