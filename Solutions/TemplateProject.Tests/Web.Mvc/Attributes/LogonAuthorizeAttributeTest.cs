using System.Web.Mvc;
using MbUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using TemplateProject.Web.Mvc.Attributes;

namespace TemplateProject.Tests.Web.Mvc.Attributes
{
    [TestFixture]
    public class LogonAuthorizeAttributeTest
    {
        [Test]
        public void OnAuthorization_Without_AllowAnonymousAttribute()
        {
            //Arrange
            var context = new AuthorizationContext
            {
                ActionDescriptor = new ReflectedActionDescriptor(typeof (NonAnnonController).GetMethod("Secured"), 
                    "Secured", new ReflectedControllerDescriptor(typeof (NonAnnonController))),
            };
            var attribute = MockRepository.GenerateMock<LogonAuthorizeAttribute>();
            attribute.Expect(x => x.BaseAuthorization(Arg<AuthorizationContext>.Is.Anything));
            attribute.Expect(x => x.OnAuthorization(null)).IgnoreArguments().CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            //Act
            attribute.OnAuthorization(context);

            //Assert
            attribute.VerifyAllExpectations();
        }

        [Test]
        public void OnAuthorization_With_AllowAnonymous_Action()
        {
            //Arrange
            var context = new AuthorizationContext
            {
                ActionDescriptor = new ReflectedActionDescriptor(typeof(NonAnnonController).GetMethod("AnonAllowed"),
                    "AnonAllowed", new ReflectedControllerDescriptor(typeof(NonAnnonController))),
            };
            var attribute = MockRepository.GenerateMock<LogonAuthorizeAttribute>();
            attribute.Expect(x => x.BaseAuthorization(Arg<AuthorizationContext>.Is.Anything)).Repeat.Never();
            attribute.Expect(x => x.OnAuthorization(null)).IgnoreArguments().CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            //Act
            attribute.OnAuthorization(context);

            //Assert
            attribute.VerifyAllExpectations();
        }

        [Test]
        public void OnAuthorization_With_AllowAnonymous_Controller()
        {
            //Arrange
            var context = new AuthorizationContext
            {
                ActionDescriptor = new ReflectedActionDescriptor(typeof(AnnonController).GetMethod("AnonAllowed"),
                    "AnonAllowed", new ReflectedControllerDescriptor(typeof(AnnonController))),
            };
            var attribute = MockRepository.GenerateMock<LogonAuthorizeAttribute>();
            attribute.Expect(x => x.BaseAuthorization(Arg<AuthorizationContext>.Is.Anything)).Repeat.Never();
            attribute.Expect(x => x.OnAuthorization(null)).IgnoreArguments().CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            //Act
            attribute.OnAuthorization(context);

            //Assert
            attribute.VerifyAllExpectations();
        }

        [AllowAnonymous]
        private class AnnonController : Controller
        {
            public ActionResult AnonAllowed()
            {
                return new EmptyResult();
            }
        }

        private class NonAnnonController : Controller
        {
            [AllowAnonymous]
            public ActionResult AnonAllowed()
            {
                return new EmptyResult();
            }

            public ActionResult Secured()
            {
                return new EmptyResult();
            }
        }
    }
}
