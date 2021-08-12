using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Take.UI.MVC.ToolsDashboard.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    public class UserController : BaseController
    {

        private readonly AppSettings _appSettings;

        public UserController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Models.User model = new Models.User();

            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from user in bank.User
                             where user.isDeleted == false
                             select user).AsQueryable();

                ViewBag.listUser = query.ToList();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Index(string searchText, bool? isActive = null)
        {
            Models.User model = new Models.User();

            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from user in bank.User
                             where user.isDeleted == false
                             && (!string.IsNullOrEmpty(searchText) ? 
                             (user.userName.Contains(searchText) || user.email.Contains(searchText)) : true) 
                             select user).AsQueryable();

                if (isActive != null)
                    query = query.Where(x => x.isActive == isActive);

                ViewBag.listUser = query.ToList();
            }

            return View(model);
        }    

        [HttpGet]
        public IActionResult Update(int idUser)
        {
            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from user in bank.User
                                 where user.idUser == idUser
                                 select user).SingleOrDefault();

                    if (query == null)
                    {
                        ShowNotificationRedirect(NotificationType.Error, $"Usuário procurado não existe");
                        return RedirectToAction("Index", "User");
                    }

                    return View(query);
                }
            }
            catch (Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao alterar Usuário: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(User updateUser)
        {
            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from user in bank.User
                                 where user.idUser == updateUser.idUser
                                 select user).SingleOrDefault();

                    query.email = updateUser.email;
                    query.login = updateUser.login;
                    query.password = updateUser.password;
                    query.userName = updateUser.userName;
                    query.isActive = updateUser.isActive;

                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotification(NotificationType.Error, $"Erro ao alterar Usuário: {ex.Message}");
                return View();
            }

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Create()
        {
            User model = new User();

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(User user)
        {
            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    user.isDeleted = false;
                    user.dateTimeInclusion = DateTime.Now;
                    bank.User.Add(user);
                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotification(NotificationType.Error, $"Erro ao criar usuário: {ex.Message}");
                return View();
            }

            return RedirectToAction("Index", "User");
        }

        [HttpGet]
        public IActionResult Delete(int idUser)
        {
            if (!isMaster)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Você não tem permissão para realizar esta ação");
                return RedirectToAction("Index", "User");
            }

            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from user in bank.User
                             where user.idUser == idUser
                             select user).SingleOrDefault();


                if (query == null)
                {
                    ShowNotificationRedirect(NotificationType.Error, $"O usuário procurado não existe");
                    return RedirectToAction("Index", "User");
                }

                return View(query);
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Excluir(int idUser)
        {
            User query = new User();
            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    query = (from user in bank.User
                                 where user.idUser == idUser
                                 select user).SingleOrDefault();


                    if (query == null)
                    {
                        ShowNotificationRedirect(NotificationType.Error, $"O usuário procurado não existe");
                        return RedirectToAction("Index", "Tool");
                    }

                    query.isDeleted = true;
                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotification(NotificationType.Error, $"Erro ao acessar página: {ex.Message}");
                return View(query);
            }

            return RedirectToAction("Index", "User");
        }
    }
}