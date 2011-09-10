using System;
using System.Web.Mvc;

namespace TemplateProject.Web.Mvc.Controllers
{
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
