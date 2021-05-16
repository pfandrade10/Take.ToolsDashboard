using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    public class BaseController : Controller
    {
        public void ShowNotification(NotificationType notificationType, string message, int? seconds = 0)
        {
            ViewData["Notification"] = new Notification(notificationType, message, seconds.Value);
        }

        public void ShowNotificationRedirect(NotificationType notificationType, string message)
        {
            if (notificationType == NotificationType.Success)
                TempData["NotificationSuccessRedirect"] = message;
            else if (notificationType == NotificationType.Error)
                TempData["NotificationErrorRedirect"] = message;
        }

        public int idUserADM { get; set; }


        public string userNameADM { get; set; }

        #region[ OnActionExecuting ]
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if (User.Identity.IsAuthenticated)
            {
                ClaimsIdentity claimsIdentity = User.Identity as ClaimsIdentity;

                Claim claimNameIdentifier = claimsIdentity?.FindFirst(ClaimTypes.NameIdentifier);
                idUserADM = Convert.ToInt32(claimNameIdentifier.Value);

                Claim claimName = claimsIdentity?.FindFirst("nome");
                userNameADM = claimName.Value;
                ViewBag.userName = userNameADM;
            }
        }
        #endregion
    }
}