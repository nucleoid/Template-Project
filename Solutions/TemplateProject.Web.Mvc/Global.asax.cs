using System;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;
using AutofacContrib.CommonServiceLocator;
using NLog;
using TemplateProject.Web.Mvc.Attributes;
using TemplateProject.Web.Mvc.Autofac;
using TemplateProject.Web.Mvc.Controllers;
using TemplateProject.Infrastructure.NHibernateMaps;
using Microsoft.Practices.ServiceLocation;
using SharpArch.NHibernate;
using SharpArch.NHibernate.Web.Mvc;
using SharpArch.Web.Mvc.ModelBinder;
using System.Configuration;
using Autofac;
using Autofac.Integration.Mvc;
using TemplateProject.Infrastructure;

namespace TemplateProject.Web.Mvc
{
    public class MvcApplication : System.Web.HttpApplication
    {
        private WebSessionStorage webSessionStorage;
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public override void Init()
        {
            base.Init();
            webSessionStorage = new WebSessionStorage(this);
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
            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
            InitializeServiceLocator();
            AreaRegistration.RegisterAllAreas();
            RouteRegistrar.RegisterRoutesTo(RouteTable.Routes);
            RegisterGlobalFilters(GlobalFilters.Filters);
            LogManager.Configuration = NLogConfiguration.CreateConfig();
        }

        protected virtual void InitializeServiceLocator() 
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());
            ModelBinders.Binders.DefaultBinder = new SharpModelBinder();
            ModelValidatorProviders.Providers.Add(new ClientDataTypeModelValidatorProvider());
            InitializeAutofacDependencyResolver();
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
        }

        private void InitialiseNHibernateSessions()
        {
            var config = new NHibernateConfiguration(ConfigurationManager.ConnectionStrings["Default"].ConnectionString);
            NHibernateSession.Init(webSessionStorage, new[] { Server.MapPath("~/bin/TemplateProject.Infrastructure.dll") },
                new AutoPersistenceModelGenerator().Generate(), null, null, null, config);
        }
    }
}