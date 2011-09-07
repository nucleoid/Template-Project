using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using Autofac.Integration.Web;
using AutofacContrib.CommonServiceLocator;
using NLog;
using Quartz;
using Quartz.Impl;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
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
    public class MvcApplication : System.Web.HttpApplication
    {
        private ThreadAndWebSessionStorage threadAndWebSessionStorage;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        private static IScheduler scheduler;

        public override void Init()
        {
            base.Init();
            threadAndWebSessionStorage = new ThreadAndWebSessionStorage(this);
        }

        protected void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorsAttribute());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            NHibernateInitializer.Instance().InitializeNHibernateOnce(InitialiseNHibernateSessions);
        }

        protected void Application_Error(object sender, EventArgs e) 
        {
            Exception ex = Server.GetLastError();
            logger.LogException(LogLevel.Error, "ASP.Net Application_Error exception", ex);
        }

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
//            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
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
            ComponentRegistrar.AddComponentsTo(builder);

            builder.RegisterModelBinders(Assembly.GetExecutingAssembly());
            builder.RegisterModelBinderProvider();
            builder.RegisterControllers(Assembly.GetExecutingAssembly());
            builder.RegisterModule(new AutofacWebTypesModule());

            var container = builder.Build();
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
            ServiceLocator.SetLocatorProvider(() => new AutofacServiceLocator(container));
            InitialiseJobScheduler(container);
        }

        private void InitialiseNHibernateSessions()
        {
            var config = new NHibernateConfiguration(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            NHibernateSession.Init(threadAndWebSessionStorage, new[] { Server.MapPath("~/bin/TemplateProject.Infrastructure.dll") },
                new AutoPersistenceModelGenerator().Generate(), null, null, null, config);
        }

        protected void InitialiseJobScheduler(IContainer container)
        {
            ISchedulerFactory factory = new StdSchedulerFactory();
            scheduler = factory.GetScheduler();
            scheduler.JobFactory = new AutofacJobFactory(new ContainerProvider(container), ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            scheduler.Start();

            var trigger = TriggerUtils.MakeImmediateTrigger(3, TimeSpan.FromSeconds(5));
            trigger.Name = @"Job Trigger";
            scheduler.ScheduleJob(new JobDetail("Job", null, typeof(OddJob)), trigger);

            EndRequest += ShutdownQuartz;
        }

        private static void ShutdownQuartz(object sender, EventArgs e)
        {
            scheduler.Shutdown(false);
        }
    }
}