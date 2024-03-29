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
        public IActionResult Update(int idTool)
        {
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

                    return View(query);
                }
            }
            catch(Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao alterar ferramenta: {ex.Message}");
                return View();
            }
        }

        [HttpPost]
        [AutoValidateAntiforgeryToken]
        public IActionResult Update(Tool updateTool)
        {
            try
            {
                if (!isMaster)
                {
                    ShowNotificationRedirect(NotificationType.Error, $"Você não tem permissão para realizar esta ação");
                    return RedirectToAction("Update", "Tool", new { idTool = updateTool.idTool });
                }

                if (string.IsNullOrEmpty(updateTool.name))
                {
                    ShowNotification(NotificationType.Error, $"O nome é obrigatório");
                    return View(updateTool);
                }

                if (string.IsNullOrEmpty(updateTool.link))
                {
                    ShowNotification(NotificationType.Error, $"O link de acesso da ferramenta é Obrigatorio");
                    return View(updateTool);
                }

                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from tool in bank.Tool
                                 where tool.idTool == updateTool.idTool
                                 select tool).SingleOrDefault();

                    query.description = updateTool.description;
                    query.link = updateTool.link;
                    query.baseUrl = updateTool.baseUrl;
                    query.name = updateTool.name;
                    query.isActive = updateTool.isActive;

                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotification(NotificationType.Error, $"Erro ao alterar ferramenta: {ex.Message}");
                return View();
            }

            ShowNotificationRedirect(NotificationType.Success, $"Ferramenta alterada com sucesso");
            return RedirectToAction("Index", "Tool");
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
                if (string.IsNullOrEmpty(tool.name))
                {
                    ShowNotification(NotificationType.Error, $"O nome é obrigatório");
                    return View(tool);
                }

                if (string.IsNullOrEmpty(tool.link))
                {
                    ShowNotification(NotificationType.Error, $"O link de acesso da ferramenta é Obrigatorio");
                    return View(tool);
                }

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
                ShowNotification(NotificationType.Error, $"Erro ao criar ferramenta: {ex.Message}");
                return View(tool);
            }

            ShowNotificationRedirect(NotificationType.Success, $"Ferramenta criada com sucesso");
            return RedirectToAction("Index","Tool");
        }        

        [HttpGet]
        public IActionResult Delete(int idTool)
        {
            if (!isMaster)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Você não tem permissão para realizar esta ação");
                return RedirectToAction("Index", "Tool");
            }
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

            ShowNotificationRedirect(NotificationType.Success, $"Ferramenta removida com sucesso");
            return RedirectToAction("Index", "Tool");
        }
    }
}
