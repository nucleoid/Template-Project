﻿
using System.Web.Mvc;

namespace TemplateProject.Web.Mvc.Controllers
{
    public class ErrorController : Controller
    {
        public ActionResult NotFound()
        {
            return View();
        }

        public ActionResult Excepted()
        {
            return View();
        }
    }
}