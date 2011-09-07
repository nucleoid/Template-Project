using System;
using System.Collections.Generic;
using MbUnit.Framework;
using Quartz;
using Quartz.Spi;
using Rhino.Mocks;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Infrastructure.Quartz.Jobs;

namespace TemplateProject.Tests.Infrastructure.Quartz.Jobs
{
    [TestFixture]
    public class OddJobTest
    {
        [Test]
        public void Execute_Gets_All_Products_For_Logger()
        {
            //Arrange
            var productTasks = MockRepository.GenerateMock<IProductTasks>();
            productTasks.Expect(x => x.GetAll()).Return(new List<Product>());
            var job = new OddJob {ProductTasks = productTasks};
            var jobDetail = new JobDetail("blag", null, typeof(OddJob));
            var trigger = TriggerUtils.MakeImmediateTrigger(0, TimeSpan.FromSeconds(2));
            var bundle = new TriggerFiredBundle(jobDetail, trigger, null, false, null, null, null, null);
            var jobExec = new JobExecutionContext(null, bundle, null);

            //Act
            job.Execute(jobExec);

            //Assert
            productTasks.VerifyAllExpectations();
        }
    }
}
