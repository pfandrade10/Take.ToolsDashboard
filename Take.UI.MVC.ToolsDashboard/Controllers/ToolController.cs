using Microsoft.AspNetCore.Mvc;
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
                             select new
                             {
                                 idTool = tool.idTool,
                                 name = tool.name,
                                 description = tool.description,
                                 link = tool.link,
                                 dateTimeInclusion = tool.dateTimeInclusion,
                             }).ToList();

                List<Tool> tools = new List<Tool>();

                foreach(var item in query)
                {
                    Tool tool = new Tool();

                    tool.idTool = item.idTool;
                    tool.name = item.name;
                    tool.description = item.description;
                    tool.link = item.link;
                    tool.dateTimeInclusion = item.dateTimeInclusion;

                    tools.Add(tool);
                }

                ViewBag.listTools = tools;
            }

            return View(model);
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
                                 select new
                                 {
                                     idTool = tool.idTool,
                                     name = tool.name,
                                     description = tool.description,
                                     link = tool.link,
                                     dateTimeInclusion = tool.dateTimeInclusion,
                                 }).SingleOrDefault();

                    
                    if(query != null)
                    {
                        model.idTool = query.idTool;
                        model.name = query.name;
                        model.description = query.description;
                        model.link = query.link;
                        model.dateTimeInclusion = query.dateTimeInclusion;
                    }
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
        public IActionResult Excluir(int idTool)
        {
            Tool model = new Tool();

            try
            {
                using (var bank = ContextFactory.Create(_appSettings.connectionString))
                {
                    var query = (from tool in bank.Tool
                                 where tool.idTool == idTool
                                 select new
                                 {
                                     idTool = tool.idTool,
                                     name = tool.name,
                                     description = tool.description,
                                     link = tool.link,
                                     dateTimeInclusion = tool.dateTimeInclusion,
                                 }).SingleOrDefault();


                    if (query != null)
                    {
                        model.idTool = query.idTool;
                        model.name = query.name;
                        model.description = query.description;
                        model.link = query.link;
                        model.dateTimeInclusion = query.dateTimeInclusion;
                        model.isDeleted = true;
                    }

                    bank.Tool.Add(model);
                    bank.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                ShowNotificationRedirect(NotificationType.Error, $"Erro ao acessar página: {ex.Message}");
                return RedirectToAction("Index", "Tool");
            }

            return View(model);
        }
    }
}
