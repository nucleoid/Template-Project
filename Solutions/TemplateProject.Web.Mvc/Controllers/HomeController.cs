using System;

namespace TemplateProject.Web.Mvc.Controllers
{
    using System.Web.Mvc;

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
