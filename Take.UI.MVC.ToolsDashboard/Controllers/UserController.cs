﻿using System;
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
        public IActionResult Details(int id)
        {
            Models.User model = new Models.User();

            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from user in bank.User
                                 where user.idUser == id
                                 select user).SingleOrDefault();

                    model = query;
                }
            }
            catch (Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao acessar página: {ex.Message}");
                return RedirectToAction("Index", "User");
            }

            return View(model);
        }


        [HttpGet]
        public IActionResult Excluir(int id)
        {
            Models.User model = new Models.User();

            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from user in bank.User
                                 where user.idUser == id
                                 select user).SingleOrDefault();


                    if (query == null)
                    {
                        ShowNotification(NotificationType.Error, $"Usuário a ser deletado não exite",1);
                        return View(model);
                    }

                    query.isDeleted = true;
                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao acessar página: {ex.Message}");
                return RedirectToAction("Index", "User");
            }

            return View(model);
        }
    }
}