﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI;
using MvpRestApiLib;
using SharpArch.Domain.Commands;
using SharpArch.NHibernate.Web.Mvc;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using MvcContrib;
using TemplateProject.Infrastructure.Queries;
using TemplateProject.Tasks.Commands;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Areas.Admin.Models;

namespace TemplateProject.Web.Mvc.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private readonly IProductTasks _productTasks;
        private readonly ICategoryTasks _categoryTasks;
        private readonly IProductsQuery _productsQuery;
        private readonly ICommandProcessor _commandProcessor;
        private readonly ICaptchaTasks _captchaTasks;

        private const int DefaultPageSize = 10;

        public ProductsController(IProductTasks productTasks, ICategoryTasks categoryTasks, IProductsQuery productsQuery, 
            ICommandProcessor commandProcessor, ICaptchaTasks captchaTasks)
        {
            _productTasks = productTasks;
            _categoryTasks = categoryTasks;
            _productsQuery = productsQuery;
            _commandProcessor = commandProcessor;
            _captchaTasks = captchaTasks;
        }

        [EnableJson, EnableXml]
        [HttpGet, OutputCache(NoStore = true, Location = OutputCacheLocation.None)]
        public ActionResult Index(int? page)
        {
            //TODO Mitch Figure out better paging architecture to allow for json and xml serialization as well as html views
//            var categories = _categoryTasks.GetAll();
//            ViewBag.Categories = categories.ToDictionary(k => k.Id, v => v.Name);
//            var products = _productsQuery.GetPagedList(page ?? 1, DefaultPageSize);
//            if(products.Count() == 1)
            return View(new ProductsViewModel { Products = _productTasks.GetAll() });
//            return View(products.ToList());
        }

        public ActionResult Create()
        {
            var categories = _categoryTasks.GetAll();
            if (categories.Count == 0)
                ViewBag.CategoryError = "You must create at least one Category before creating a Product.";

            var model = new ProductEditViewModel
            {
                Categories = _categoryTasks.GetAll(),
                Product = new Product(),
                SelectedCategoryId = 0
            };
            return View(model);
        } 

        public ActionResult Edit(int id)
        {
            var product = _productTasks.Get(id);
            var model = new ProductEditViewModel
            {
                Categories = _categoryTasks.GetAll(),
                Product = product,
                SelectedCategoryId = product.Category.Id
            };
            return View(model);
        }

        [EnableJson, EnableXml]
        [Transaction]
        [ValidateAntiForgeryToken]
        [HttpPost, HttpPut]
        public ActionResult Edit(Product product)
        {
            if (_captchaTasks.Validate(ConfigurationManager.AppSettings["ReCaptchaPrivate"]))
            {
                if (ModelState.IsValid && product.IsValid())
                {
                    _productTasks.CreateOrUpdate(product);
                    return this.RedirectToAction(x => x.Index(null));
                }
            }
            else
                ModelState.AddModelError("ReCaptcha", "ReCaptcha failed!");

            var model = new ProductEditViewModel
            {
                Categories = _categoryTasks.GetAll(),
                Product = product,
                SelectedCategoryId = product.Category.Id
            };

            if (product.Id == 0)
                return View("Create", model);
            return View(model);
        }

        [Transaction]
        [EnableJson, EnableXml]
        [HttpDelete]
        public ActionResult Delete(int id)
        {
            _productTasks.Delete(id);
            return this.RedirectToAction(x => x.Index(null));
        }

        //Ajax action
        [Transaction]
        [HttpPost]
        public ActionResult ChangeCategory(FormCollection collection)
        {
            var catId = Int32.Parse(collection["category"]);
            var productIds = new List<int>();
            foreach (var key in collection.AllKeys)
            {
                int productId;
                if(Int32.TryParse(key, out productId) && collection[key] != bool.FalseString.ToLowerInvariant())
                    productIds.Add(productId);
            }

            var command = new MassCategoryChangeCommand(catId, productIds);
            var results = _commandProcessor.Process(command);
            if (results.Success)
                return new ContentResult { Content = "Categories successfully changed! Refresh to see the changes.", ContentType = "text/html" };
            return new ContentResult { Content = "One or more categories failed to change!", ContentType = "text/html" };
        }
    }
}
