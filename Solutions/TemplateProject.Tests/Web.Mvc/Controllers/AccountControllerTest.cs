using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MbUnit.Framework;
using MvcContrib.TestHelper.Fakes;
using Rhino.Mocks;
using TemplateProject.Tasks;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Controllers;
using TemplateProject.Web.Mvc.Models;

namespace TemplateProject.Tests.Web.Mvc.Controllers
{
    [TestFixture]
    public class AccountControllerTest
    {
        private AccountController _controller;
        private IAuthenticationTasks _authTasks;
        private IMembershipTasks _membershipTasks;

        [SetUp]
        public void Setup()
        {
            _authTasks = MockRepository.GenerateMock<IAuthenticationTasks>();
            _membershipTasks = MockRepository.GenerateMock<IMembershipTasks>();
            _controller = new AccountController(_authTasks, _membershipTasks);
        }

        [Test]
        public void LogOn_Forwards_To_LogOn_View()
        {
            //Act
            var result = _controller.LogOn() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        public void LogOn_Validates_Bad_Model_And_Forwards_To_LogOn()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            
            //Act
            var result = _controller.LogOn(new LogOnModel(), null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<LogOnModel>(result.Model);
        }

        [Test]
        public void LogOn_Validates_Bad_Credentials_And_Forwards_To_LogOn()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel {UserName = "babygirl", Password = "nono"};
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("babygirl"), Arg<string>.Is.Equal("nono"))).Return(false);

            //Act
            var result = _controller.LogOn(model, null) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<LogOnModel>(result.Model);
            Assert.AreEqual("The user name or password provided is incorrect.", result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void LogOn_Validates_Good_Credentials_And_Redirects_With_Bad_Local_Url()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel { UserName = "cooluser", Password = "coolpassword" };
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);
            var urlHelper = MockRepository.GenerateMock<UrlHelper>(new RequestContext(httpContext, new RouteData()));
            _controller.Url = urlHelper;
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("cooluser"), Arg<string>.Is.Equal("coolpassword"))).Return(true);
            _authTasks.Expect(x => x.SetAuthCookie(Arg<string>.Is.Equal("cooluser"), Arg<bool>.Is.Equal(false)));

            //Act
            var result = _controller.LogOn(model, null) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _authTasks.VerifyAllExpectations();
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void LogOn_Validates_Good_Credentials_And_Redirects_With_Good_Local_Url()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new LogOnModel { UserName = "cooluser", Password = "coolpassword" };
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);
            var urlHelper = MockRepository.GenerateMock<UrlHelper>(new RequestContext(httpContext, routeData));
            _controller.Url = urlHelper;
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("cooluser"), Arg<string>.Is.Equal("coolpassword"))).Return(true);
            _authTasks.Expect(x => x.SetAuthCookie(Arg<string>.Is.Equal("cooluser"), Arg<bool>.Is.Equal(false)));

            //Act
            var result = _controller.LogOn(model, "/blah") as RedirectResult;

            //Assert
            Assert.AreEqual("/blah", result.Url);
            _authTasks.VerifyAllExpectations();
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void LogOff_Signs_Out_And_Redirects()
        {
            //Arrange
            var request = new SimulatedHttpRequest("/", string.Empty, string.Empty, string.Empty, null, "localhost");
            HttpContext.Current = new HttpContext(request);
            _authTasks.Expect(x => x.SignOut());

            //Act
            var result = _controller.LogOff() as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _authTasks.VerifyAllExpectations();
        }

        [Test]
        public void Register_Forwards_Register_With_Model()
        {
            //Arrange
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);

            //Act
            var result = _controller.Register() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<RegisterModel>(result.Model);
            Assert.AreEqual(6, (result.Model as RegisterModel).MinRequiredPasswordLength);
        }

        [Test]
        public void Register_Validates_Bad_Model_And_Forwards_To_Register()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);

            //Act
            var result = _controller.Register(new RegisterModel()) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<RegisterModel>(result.Model);
            Assert.AreEqual(6, (result.Model as RegisterModel).MinRequiredPasswordLength);
        }

        [Test]
        public void Register_With_CreateUser_Failure()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);
            MembershipCreateStatus status;
            _membershipTasks.Stub(x => x.CreateUser("hot", "5t1ckY", "head@toFeet.com", null, null, true, null, out status)).IgnoreArguments().
                OutRef(MembershipCreateStatus.DuplicateUserName);
            var model = new RegisterModel { UserName = "hot", Password = "5t1ckY", ConfirmPassword = "5t1ckY", Email = "sweetHead@toFeet.com" };

            //Act
            var result = _controller.Register(model) as ViewResult;

            //Assert
            Assert.AreEqual("User name already exists. Please enter a different user name.", result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<RegisterModel>(result.Model);
            Assert.AreEqual(6, (result.Model as RegisterModel).MinRequiredPasswordLength);
        }

        [Test]
        public void Register_With_CreateUser_Success()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);
            MembershipCreateStatus status;
            _membershipTasks.Stub(x => x.CreateUser("hot", "5t1ckY", "head@toFeet.com", null, null, true, null, out status)).IgnoreArguments().
                OutRef(MembershipCreateStatus.Success);
            _authTasks.Expect(x => x.SetAuthCookie(Arg<string>.Is.Equal("hot"), Arg<bool>.Is.Equal(false)));
            var model = new RegisterModel { UserName = "hot", Password = "5t1ckY", ConfirmPassword = "5t1ckY", Email = "sweetHead@toFeet.com" };

            //Act
            var result = _controller.Register(model) as RedirectToRouteResult;

            //Assert
            Assert.AreEqual("Home", result.RouteValues["controller"]);
            Assert.AreEqual("Index", result.RouteValues["action"]);
            _authTasks.VerifyAllExpectations();
        }

        [Test]
        public void ChangePassword_Forwards_To_ChangePassword_With_Model()
        {
            //Arrange
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);

            //Act
            var result = _controller.ChangePassword() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ChangePasswordModel>(result.Model);
            Assert.AreEqual(6, (result.Model as ChangePasswordModel).MinRequiredPasswordLength);
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void ChangePassword_Validates_Bad_Model_And_Forwards_To_ChangePassword()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateStub<HttpContextBase>();
            var controllerContext = MockRepository.GenerateStub<ControllerContext>(httpContext, routeData, _controller);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);

            //Act
            var result = _controller.ChangePassword(new ChangePasswordModel()) as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ChangePasswordModel>(result.Model);
            Assert.AreEqual(6, (result.Model as ChangePasswordModel).MinRequiredPasswordLength);
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void ChangePassword_Fails_And_Forwards_To_ChangePassword()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.User).Return(new FakePrincipal(new FakeIdentity("Bruce"), null));
            var controllerContext = MockRepository.GenerateMock<ControllerContext>(httpContext, routeData, _controller);
            controllerContext.Expect(x => x.HttpContext).Return(httpContext);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            _membershipTasks.Expect(x => x.MinRequiredPasswordLength).Return(6);
            var model = new ChangePasswordModel { OldPassword = "peace", NewPassword = "ofMind", ConfirmPassword = "ofMind" };
            _membershipTasks.Expect(x => x.GetUser(Arg<string>.Is.Anything, Arg<bool>.Is.Anything)).Throw(new Exception());

            //Act
            var result = _controller.ChangePassword(model) as ViewResult;

            //Assert
            Assert.AreEqual("The current password is incorrect or the new password is invalid.", result.ViewData.ModelState[string.Empty].Errors[0].ErrorMessage);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOfType<ChangePasswordModel>(result.Model);
            Assert.AreEqual(6, (result.Model as ChangePasswordModel).MinRequiredPasswordLength);
            _membershipTasks.VerifyAllExpectations();
        }

        [Test]
        public void ChangePassword_Succeeds_And_Redirects()
        {
            //Arrange
            var routeData = new RouteData();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.User).Return(new FakePrincipal(new FakeIdentity("Bruce"), null));
            var controllerContext = MockRepository.GenerateMock<ControllerContext>(httpContext, routeData, _controller);
            controllerContext.Expect(x => x.HttpContext).Return(httpContext);
            _controller.ControllerContext = controllerContext;
            _controller.ValueProvider = new FormCollection().ToValueProvider();
            var model = new ChangePasswordModel { OldPassword = "peace", NewPassword = "ofMind", ConfirmPassword = "ofMind" };
            var user = MockRepository.GenerateMock<MembershipUser>();
            user.Expect(x => x.ChangePassword(Arg<string>.Is.Equal("peace"), Arg<string>.Is.Equal("ofMind"))).Return(true);
            _membershipTasks.Expect(x => x.GetUser(Arg<string>.Is.Equal("Bruce"), Arg<bool>.Is.Equal(true))).Return(user);

            //Act
            var result = _controller.ChangePassword(model) as RedirectToRouteResult;

            //Assert
            Assert.IsNull(result.RouteValues["controller"]);
            Assert.AreEqual("ChangePasswordSuccess", result.RouteValues["action"]);
            user.VerifyAllExpectations();
            httpContext.VerifyAllExpectations();
        }

        [Test]
        public void ChangePasswordSuccess_Forwards_To_ChangePasswordSuccess_View()
        {
            //Act
            var result = _controller.ChangePasswordSuccess() as ViewResult;

            //Assert
            Assert.AreEqual(string.Empty, result.ViewName);
        }

        [Test]
        [Row(MembershipCreateStatus.DuplicateEmail, "A user name for that e-mail address already exists. Please enter a different e-mail address.")]
        [Row(MembershipCreateStatus.DuplicateProviderUserKey, "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.")]
        [Row(MembershipCreateStatus.DuplicateUserName, "User name already exists. Please enter a different user name.")]
        [Row(MembershipCreateStatus.InvalidAnswer, "The password retrieval answer provided is invalid. Please check the value and try again.")]
        [Row(MembershipCreateStatus.InvalidEmail, "The e-mail address provided is invalid. Please check the value and try again.")]
        [Row(MembershipCreateStatus.InvalidPassword, "The password provided is invalid. Please enter a valid password value.")]
        [Row(MembershipCreateStatus.InvalidProviderUserKey, "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.")]
        [Row(MembershipCreateStatus.InvalidQuestion, "The password retrieval question provided is invalid. Please check the value and try again.")]
        [Row(MembershipCreateStatus.InvalidUserName, "The user name provided is invalid. Please check the value and try again.")]
        [Row(MembershipCreateStatus.ProviderError, "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.")]
        [Row(MembershipCreateStatus.UserRejected, "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.")]
        public void ErrorCodeToString_Returns_Correct_String(MembershipCreateStatus status, string codeMessage)
        {
            //Act
            var message = AccountController.ErrorCodeToString(status);

            //Assert
            Assert.AreEqual(codeMessage, message);
        }
    }
}
