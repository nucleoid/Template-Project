using System;
using System.Reflection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Web;
using AutofacContrib.CommonServiceLocator;
using NLog;
using Quartz;
using Quartz.Impl;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Infrastructure.FluentMigrations;
using TemplateProject.Infrastructure.NHibernateConfig;
using TemplateProject.Infrastructure.Quartz;
using TemplateProject.Infrastructure.Quartz.Jobs;
using TemplateProject.Web.Mvc.Areas.Admin.Models;
using TemplateProject.Web.Mvc.Attributes;
using TemplateProject.Web.Mvc.Autofac;
using TemplateProject.Web.Mvc.Binders;
using TemplateProject.Web.Mvc.Controllers;
using Microsoft.Practices.ServiceLocation;
using SharpArch.NHibernate;
using SharpArch.Web.Mvc.ModelBinder;
using System.Configuration;
using Autofac;
using Autofac.Integration.Mvc;
using TemplateProject.Infrastructure;

namespace TemplateProject.Web.Mvc
{
    public class MvcApplication : HttpApplication
    {
        private ThreadAndWebSessionStorage _threadAndWebSessionStorage;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static IScheduler scheduler;
        private string _connectionString;

        public override void Init()
        {
            base.Init();
            _threadAndWebSessionStorage = new ThreadAndWebSessionStorage(this);
        }

        protected void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorsAttribute());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitializeNHibernateSessions);
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception ex = Server.GetLastError();
            logger.LogException(LogLevel.Error, "ASP.Net Application_Error exception", ex);
        }

        protected void Application_Start()
        {
            _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
            InitializeAutofacDependencyResolver();
            ModelBinders.Binders.Add(typeof(Product), new ProductBinder(DependencyResolver.Current.GetService<ICategoryTasks>()));
            ModelBinders.Binders.Add(typeof(ProductEditViewModel), new ProductEditViewModelBinder(DependencyResolver.Current.GetService<ICategoryTasks>()));
            AreaRegistration.RegisterAllAreas();
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            LogManager.Configuration = NLogConfiguration.CreateConfig();
        }

        private void InitializeAutofacDependencyResolver()
        {
            var builder = new ContainerBuilder();
            var executingAssembly = Assembly.GetExecutingAssembly();
            ComponentRegistrar.AddComponentsTo(builder, executingAssembly);

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            InitializeJobScheduler(container);
        }

        private void InitializeJobScheduler(IContainer container)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitializeNHibernateSessions);
            ISchedulerFactory factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
            scheduler.JobFactory = new AutofacJobFactory(new ContainerProvider(container));
            scheduler.Start();

            var trigger = TriggerUtils.MakeSecondlyTrigger(5, 10);
            trigger.Name = @"Job Trigger";
            scheduler.ScheduleJob(new JobDetail("Job", null, typeof(OddJob)), trigger);
        }

        private void InitializeNHibernateSessions()
        {
            MigrateDatabase();
            _threadAndWebSessionStorage = new ThreadAndWebSessionStorage(this);
            var config = new NHibernateConfiguration(_connectionString);
            NHibernateSession.Init(_threadAndWebSessionStorage, new[] { Server.MapPath("~/bin/TemplateProject.Infrastructure.dll") },
                new AutoPersistenceModelGenerator().Generate(), null, null, null, config);
        }

        private void MigrateDatabase()
        {
            var runner = new Runner(_connectionString, typeof(Runner).Assembly);
            runner.Run();
        }
    }
}