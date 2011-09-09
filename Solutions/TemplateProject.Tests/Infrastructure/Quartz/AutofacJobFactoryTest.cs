using System;
using Autofac;
using Autofac.Integration.Web;
using MbUnit.Framework;
using Quartz;
using Quartz.Spi;
using Rhino.Mocks;
using TemplateProject.Infrastructure.Quartz;
using TemplateProject.Infrastructure.Quartz.Jobs;
using TemplateProject.Web.Mvc.Autofac;

namespace TemplateProject.Tests.Infrastructure.Quartz
{
    [TestFixture]
    public class AutofacJobFactoryTest
    {
        [Test]
        public void NewJob_Injects_Properties()
        {
            //Arrange
            var builder = new ContainerBuilder();
            ComponentRegistrar.AddComponentsTo(builder);
            var containerProvider = new ContainerProvider(builder.Build());
            var jobDetail = new JobDetail("blag", null, typeof (OddJob));
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var factory = new AutofacJobFactory(containerProvider);

            //Act
            var job = factory.NewJob(bundle) as OddJob;

            //Assert
            Assert.IsNotNull(job.ProductTasks);
        }
    }
}
