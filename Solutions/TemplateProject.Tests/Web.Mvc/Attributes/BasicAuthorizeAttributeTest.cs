
using System.Web.Mvc;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Attributes;

namespace TemplateProject.Tests.Web.Mvc.Attributes
{
    [TestFixture]
    public class BasicAuthorizeAttributeTest
    {
        [Test]
        public void Constructor_Resolves_Dependencies()
        {
            //Assert
            var resolver = MockRepository.GenerateMock<IDependencyResolver>();
            resolver.Expect(x => x.GetService(typeof(IMembershipTasks)));
            DependencyResolver.SetResolver(resolver);

            //Act
            var attr = new BasicAuthorizeAttribute();

            //Assert
            Assert.IsTrue(attr.RequireSsl);
            resolver.VerifyAllExpectations();
        }

        //TODO Mitch Finish BasicAuthorizeAttribute tests
    }
}
