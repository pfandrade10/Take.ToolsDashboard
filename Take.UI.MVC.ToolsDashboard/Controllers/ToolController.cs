﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Take.UI.MVC.ToolsDashboard.Models;

namespace Take.UI.MVC.ToolsDashboard.Controllers
{
    
    public class ToolController : BaseController
    {
        private readonly AppSettings _appSettings;

        public ToolController(IOptions<AppSettings> appSettings)
        {
            _appSettings = appSettings.Value;
        }

        [HttpGet]
        public IActionResult Index()
        {
            Tool model = new Tool();

            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from tool in bank.Tool
                             where tool.isDeleted == false
                             select tool).AsQueryable();

                ViewBag.listTools = query.ToList();
            }

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Index(string searchText)
        {
            Tool model = new Tool();

            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from tool in bank.Tool
                             where tool.isDeleted == false
                             && (!string.IsNullOrEmpty(searchText) ? (tool.name.Contains(searchText) 
                             || tool.description.Contains(searchText)) : true)
                             select tool).AsQueryable();              

                ViewBag.listTools = query.ToList();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            Tool model = new Tool();      

            return View(model);
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Create(Tool tool)
        {
            try
            {
                
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    tool.isDeleted = false;
                    tool.dateTimeInclusion = DateTime.Now;
                    bank.Tool.Add(tool);
                    bank.SaveChanges();
                } 
            }
            catch(Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao criar ferramenta: {ex.Message}");
                return View();
            }

            return RedirectToAction("Index","Tool");
        }

        [HttpGet]
        public IActionResult Details(int idTool)
        {
            Tool model = new Tool();

            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from tool in bank.Tool
                                 where tool.idTool == idTool
                                 select tool).SingleOrDefault();

                    
                    if(query == null)
                    {
                        ShowNotificationRedirect(NotificationType.Error, $"Ferramenta procurada não existe");
                        return RedirectToAction("Index", "Tool");
                    }

                    model = query;
                }
            }
            catch(Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error,$"Erro ao acessar página: {ex.Message}");
                return RedirectToAction("Index", "Tool");
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Delete(int idTool)
        {
            using (var bank = ContextFactory.Create(_appSettings.connectionString))
            {
                var query = (from tool in bank.Tool
                             where tool.idTool == idTool
                             select tool).SingleOrDefault();


                if (query == null)
                {
                    ShowNotificationRedirect(NotificationType.Error, $"Ferramenta procurada não existe");
                    return RedirectToAction("Index", "Tool");
                }

                return View(query);
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Excluir(int idTool)
        {
            Tool query = new Tool();
            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    query = (from tool in bank.Tool
                                 where tool.idTool == idTool
                                 select tool).SingleOrDefault();

                    if (query == null)
                    {
                        ShowNotificationRedirect(NotificationType.Error, $"Ferramenta procurada não existe");
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

            return RedirectToAction("Index", "Tool");
        }
    }
}
