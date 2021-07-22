using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard.Models
{
    public class Tool
    {
        [Key]
        public int idTool { get; set; }

        public string name { get; set; }

        public string description { get; set; }

        public string baseUrl { get; set; }

        public string link { get; set; }

        public DateTime dateTimeInclusion { get; set; }

        public bool isDeleted { get; set; }

        public bool isActive { get; set; }
    }
}
