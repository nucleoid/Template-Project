using System;
using Autofac;
using Autofac.Integration.Web;
using MbUnit.Framework;
using Quartz;
using Quartz.Spi;
using Rhino.Mocks;
using TemplateProject.Infrastructure.Quartz;
using TemplateProject.Infrastructure.Quartz.Jobs;

namespace TemplateProject.Tests.Infrastructure.Quartz
{
    [TestFixture]
    public class AutofacJobFactoryTest
    {
        [Test]
        public void NewJob_Injects_Properties()
        {
            //Arrange
            var containerProvider = MockRepository.GenerateMock<IContainerProvider>();
            var container = MockRepository.GenerateMock<IContainer>();
            containerProvider.Expect(x => x.ApplicationContainer).Return(container);
            container.Expect(x => x.InjectUnsetProperties(Arg<IJob>.Is.Anything));
            var jobDetail = new JobDetail("blag", null, typeof (OddJob));
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var factory = new AutofacJobFactory(containerProvider);

            //Act
            factory.NewJob(bundle);

            //Assert
            containerProvider.VerifyAllExpectations();
            container.VerifyAllExpectations();
        }
    }
}
