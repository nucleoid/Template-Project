using System;
using System.Web.Mvc;
using TemplateProject.Web.Mvc.Attributes;

namespace TemplateProject.Web.Mvc.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ThrowTestException()
        {
            throw new Exception("Oh lordy, an exception has happened!");
        }
    }
}
