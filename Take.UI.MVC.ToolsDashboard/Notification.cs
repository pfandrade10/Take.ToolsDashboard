using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Take.UI.MVC.ToolsDashboard
{
    public class Notification
    {
        public Notification(NotificationType notificationType, string texto, int segundos)
        {
            NotificationType = notificationType;
            Texto = texto;
            Segundos = segundos;
        }

        public string Texto { get; set; }

        public NotificationType NotificationType { get; set; }

        public int Segundos { get; set; }
    }

    [Description("Tipo de notificação")]
    public enum NotificationType
    {
        [Description("Sucesso")]
        Success = 0,
        [Description("Error")]
        Error = 1,
        [Description("Não setado")]
        Default = 1,
    }
}

