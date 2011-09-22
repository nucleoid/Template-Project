using System.Web.Mvc;
using System.Web.UI;
using MvpRestApiLib;
using SharpArch.NHibernate.Web.Mvc;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Queries;
using TemplateProject.Domain.Contracts.Tasks;
using MvcContrib;
using TemplateProject.Web.Mvc.Areas.Admin.Models;

namespace TemplateProject.Web.Mvc.Areas.Admin.Controllers
{
    public class CategoriesController : Controller
    {
        private readonly ICategoryTasks _tasks;
        private readonly ICategoriesQuery _categoriesQuery;

        private const int DefaultPageSize = 10;

        public CategoriesController(ICategoryTasks tasks, ICategoriesQuery categoriesQuery)
        {
            _tasks = tasks;
            _categoriesQuery = categoriesQuery;
        }

        [EnableJson, EnableXml]
        [HttpGet, OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Index(int? page)
        {
            var categories = _categoriesQuery.GetPagedList(page ?? 1, DefaultPageSize);
            return View(new CategoriesViewModel { Categories = categories });
        }

        public ActionResult Details(int id)
        {
            var category = _tasks.Get(id);
            return View(category);
        }

        public ActionResult Create()
        {
            return View(new Category());
        } 

        public ActionResult Edit(int id)
        {
            var category = _tasks.Get(id);
            return View(category);
        }

        [EnableJson, EnableXml]
        [Transaction]
        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Put)]
        public ActionResult Save(Category category)
        {
            if (ModelState.IsValid && category.IsValid())
            {
                _tasks.CreateOrUpdate(category);
                return this.RedirectToAction(x => x.Index(null));
            }

            if (category.Id == 0)
                return View("Create", category);
            return View(category);
        }

        [Transaction]
        [EnableJson, EnableXml]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Delete)]
        public ActionResult Delete(int id)
        {
            _tasks.Delete(id);
            return this.RedirectToAction(x => x.Index(null));
        }
    }
}
