
using System;
using System.Collections.Specialized;
using System.IO;
using System.Security.Principal;
using System.Web;
using System.Web.Mvc;
using MbUnit.Framework;
using MvcContrib.TestHelper.Fakes;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Attributes;
using TemplateProject.Web.Mvc.Results;

namespace TemplateProject.Tests.Web.Mvc.Attributes
{
    [TestFixture]
    public class BasicAuthorizeAttributeTest
    {
        private BasicAuthorizeAttribute _attribute;
        private IMembershipTasks _membershipTasks;

        [SetUp]
        public void Setup()
        {
            var resolver = MockRepository.GenerateMock<IDependencyResolver>();
            _membershipTasks = MockRepository.GenerateMock<IMembershipTasks>();
            resolver.Expect(x => x.GetService(typeof(IMembershipTasks))).Return(_membershipTasks);
            DependencyResolver.SetResolver(resolver);
            _attribute = new BasicAuthorizeAttribute();
        }

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

        [Test]
        public void CacheValidateHandler_Sets_Validation_Status()
        {
            //Arrange
            var context = new HttpContext(new HttpRequest("blah.txt", "http://google.com", string.Empty), new HttpResponse(new StreamWriter(new MemoryStream())));
            context.User = new FakePrincipal(new FakeIdentity("ninja"), new string[0]);
            var status = HttpValidationStatus.Invalid;

            //Act
            _attribute.CacheValidateHandler(context, "blah", ref status);

            //Assert
            Assert.AreNotEqual(HttpValidationStatus.Invalid, status);
        }

        [Test]
        public void ParseAuthHeader_With_Null_Header()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader(null);

            //Assert
            Assert.IsNull(parsed);
        }

        [Test]
        public void ParseAuthHeader_Without_Basic_Prefix()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader("Grinch");

            //Assert
            Assert.IsNull(parsed);
        }

        [Test]
        public void ParseAuthHeader_With_Too_Many_Creds()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader("Basic dXNlcm5hbWU6cGFzc3dvcmQ6YmxhaA==");

            //Assert
            Assert.IsNull(parsed);
        }

        [Test]
        public void ParseAuthHeader_With_Empty_Username()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader("Basic OnBhc3N3b3Jk");

            //Assert
            Assert.IsNull(parsed);
        }

        [Test]
        public void ParseAuthHeader_With_Empty_Password()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader("Basic dXNlcm5hbWU6");

            //Assert
            Assert.IsNull(parsed);
        }

        [Test]
        public void ParseAuthHeader_With_Correct_Header()
        {
            //Act
            var parsed = _attribute.ParseAuthHeader("Basic dXNlcm5hbWU6cGFzc3dvcmQ=");

            //Assert
            Assert.AreEqual(2, parsed.Length);
            Assert.AreEqual("username", parsed[0]);
            Assert.AreEqual("password", parsed[1]);
        }

        [Test]
        public void TryGetPrincipal_Username_Password_With_Invalid_User()
        {
            //Arrange
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            IPrincipal principal = new FakePrincipal(new FakeIdentity("blah"), null);

            //Act
            var principaled = _attribute.TryGetPrincipal("blah", "password", out principal);

            //Assert
            Assert.IsNull(principal);
            Assert.IsFalse(principaled);
        }

        [Test]
        public void TryGetPrincipal_Username_Password_With_Valid_User()
        {
            //Arrange
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.FindRolesFor(Arg<string>.Is.Equal("blah"))).Return(new[] {"admin", "ninja"});
            IPrincipal principal;

            //Act
            var principaled = _attribute.TryGetPrincipal("blah", "password", out principal);

            //Assert
            Assert.AreEqual("blah", principal.Identity.Name);
            Assert.IsTrue(principal.IsInRole("admin"));
            Assert.IsTrue(principal.IsInRole("ninja"));
            Assert.IsTrue(principaled);
            _attribute.VerifyAllExpectations();
        }

        [Test]
        public void TryGetPrincipal_Authheader_Without_Creds()
        {
            //Arrange
            IPrincipal principal;

            //Act
            var principaled = _attribute.TryGetPrincipal(null, out principal);

            Assert.IsNull(principal);
            Assert.IsFalse(principaled);
        }

        [Test]
        public void TryGetPrincipal_Authheader_With_Invalid_User()
        {
            //Arrange
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            IPrincipal principal;

            //Act
            var principaled = _attribute.TryGetPrincipal("Basic dXNlcm5hbWU6cGFzc3dvcmQ=", out principal);

            Assert.IsNull(principal);
            Assert.IsFalse(principaled);
        }

        [Test]
        public void TryGetPrincipal_Authheader_With_Valid_User()
        {
            //Arrange
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(true);
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.FindRolesFor(Arg<string>.Is.Equal("username"))).Return(new[] { "admin", "ninja" });
            IPrincipal principal;

            //Act
            var principaled = _attribute.TryGetPrincipal("Basic dXNlcm5hbWU6cGFzc3dvcmQ=", out principal);

            Assert.AreEqual("username", principal.Identity.Name);
            Assert.IsTrue(principal.IsInRole("admin"));
            Assert.IsTrue(principal.IsInRole("ninja"));
            Assert.IsTrue(principaled);
            _attribute.VerifyAllExpectations();
        }

        [Test]
        public void Authenticate_With_RequireSsl_And_Not_Secure()
        {
            //Arrange
            var context = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.IsSecureConnection).Return(false);
            request.Expect(x => x.IsLocal).Return(false);
            context.Expect(x => x.Request).Return(request);

            //Act
            var authed = _attribute.Authenticate(context);

            //Assert
            Assert.IsFalse(authed);
        }

        [Test]
        public void Authenticate_Without_Authentication_Header()
        {
            //Arrange
            var context = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.Headers).Return(new NameValueCollection());
            context.Expect(x => x.Request).Return(request);
            _attribute.RequireSsl = false;

            //Act
            var authed = _attribute.Authenticate(context);

            //Assert
            Assert.IsFalse(authed);
        }

        [Test]
        public void Authenticate_With_Invalid_User()
        {
            //Arrange
            var context = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.Headers).Return(new NameValueCollection { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });
            context.Expect(x => x.Request).Return(request);
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Anything, Arg<string>.Is.Anything)).Return(false);
            _attribute.RequireSsl = false;

            //Act
            var authed = _attribute.Authenticate(context);

            //Assert
            Assert.IsFalse(authed);
        }

        [Test]
        public void Authenticate_With_Valid_User()
        {
            //Arrange
            var context = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.Headers).Return(new NameValueCollection { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });
            context.Expect(x => x.Request).Return(request);
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("username"), Arg<string>.Is.Equal("password"))).Return(true);
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.FindRolesFor(Arg<string>.Is.Equal("username"))).Return(new[] { "admin", "ninja" });
            _attribute.RequireSsl = false;
            HttpContext.Current = new HttpContext(new HttpRequest("blah.txt", "http://google.com", string.Empty), 
                new HttpResponse(new StreamWriter(new MemoryStream())));

            //Act
            var authed = _attribute.Authenticate(context);

            //Assert
            Assert.AreEqual("username", HttpContext.Current.User.Identity.Name);
            Assert.IsTrue(authed);
        }

        [Test, ExpectedArgumentNullException]
        public void OnAuthorization_With_Null_FilterContext()
        {
            //Act
            _attribute.OnAuthorization(null);
        }

        [Test]
        public void OnAuthorization_Without_Restful_Request()
        {
            //Arrange
            var context = MockRepository.GenerateMock<AuthorizationContext>();
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            httpContext.Expect(x => x.Request).Return(MockRepository.GenerateMock<HttpRequestBase>());
            context.Expect(x => x.HttpContext).Return(httpContext);
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.BaseAuthorization(null)).IgnoreArguments();
            _attribute.Expect(x => x.OnAuthorization(null)).IgnoreArguments().CallOriginalMethod(
                OriginalCallOptions.CreateExpectation);

            //Act
            _attribute.OnAuthorization(context);

            //Assert
            context.VerifyAllExpectations();
            _attribute.VerifyAllExpectations();
        }

        [Test]
        public void OnAuthorization_With_Restful_Request_And_Bad_Authentication()
        {
            //Arrange
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] { "text/json" });
            request.Expect(x => x.Headers).Return(new NameValueCollection());
            httpContext.Expect(x => x.Request).Return(request);
            var context = new AuthorizationContext {HttpContext = httpContext};
            _attribute.RequireSsl = false;

            //Act
            _attribute.OnAuthorization(context);

            //Assert
            Assert.IsInstanceOfType<HttpBasicUnauthorizedResult>(context.Result);
            request.VerifyAllExpectations();
        }

        [Test]
        public void OnAuthorization_With_Restful_Request_Authenticated_But_Not_Authorized()
        {
            //Arrange
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] { "text/json" });
            request.Expect(x => x.Headers).Return(new NameValueCollection { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("username"), Arg<string>.Is.Equal("password"))).Return(true);
            httpContext.Expect(x => x.Request).Return(request);
            var context = new AuthorizationContext { HttpContext = httpContext };
            HttpContext.Current = new HttpContext(new HttpRequest("blah.txt", "http://google.com", string.Empty),
                new HttpResponse(new StreamWriter(new MemoryStream())));
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.FindRolesFor(Arg<string>.Is.Equal("username"))).Return(new[] { "admin", "ninja" });
            _attribute.RequireSsl = false;
            _attribute.Expect(x => x.BaseAuthorizeCore(Arg<HttpContextBase>.Is.Anything)).Return(false);
            _attribute.Expect(x => x.OnAuthorization(Arg<AuthorizationContext>.Is.Anything)).CallOriginalMethod(
                OriginalCallOptions.CreateExpectation);

            //Act
            _attribute.OnAuthorization(context);

            //Assert
            Assert.IsInstanceOfType<HttpBasicUnauthorizedResult>(context.Result);
            httpContext.VerifyAllExpectations();
            request.VerifyAllExpectations();
            _attribute.VerifyAllExpectations();
        }

        [Test]
        public void OnAuthorization_With_Restful_Request_Authenticated_And_Authorized()
        {
            //Arrange
            var httpContext = MockRepository.GenerateMock<HttpContextBase>();
            var request = MockRepository.GenerateMock<HttpRequestBase>();
            request.Expect(x => x.AcceptTypes).Return(new[] { "text/json" });
            request.Expect(x => x.Headers).Return(new NameValueCollection { { "Authorization", "Basic dXNlcm5hbWU6cGFzc3dvcmQ=" } });
            _membershipTasks.Expect(x => x.ValidateUser(Arg<string>.Is.Equal("username"), Arg<string>.Is.Equal("password"))).Return(true);
            httpContext.Expect(x => x.Request).Return(request);
            var response = MockRepository.GenerateMock<HttpResponseBase>();
            var cachePolicy = new TestCachePolicy();
            response.Expect(x => x.Cache).Return(cachePolicy);
            httpContext.Expect(x => x.Response).Return(response);
            var context = new AuthorizationContext { HttpContext = httpContext };
            HttpContext.Current = new HttpContext(new HttpRequest("blah.txt", "http://google.com", string.Empty),
                new HttpResponse(new StreamWriter(new MemoryStream())));
            _attribute = MockRepository.GenerateMock<BasicAuthorizeAttribute>();
            _attribute.Expect(x => x.FindRolesFor(Arg<string>.Is.Equal("username"))).Return(new[] { "admin", "ninja" });
            _attribute.RequireSsl = false;
            _attribute.Expect(x => x.BaseAuthorizeCore(Arg<HttpContextBase>.Is.Anything)).Return(true);
            _attribute.Expect(x => x.OnAuthorization(Arg<AuthorizationContext>.Is.Anything)).CallOriginalMethod(
                OriginalCallOptions.CreateExpectation);

            //Act
            _attribute.OnAuthorization(context);

            //Assert
            Assert.AreEqual(0, cachePolicy.MaxAge.Seconds);
            Assert.IsTrue(cachePolicy.ValidationCallbackSet);
            httpContext.VerifyAllExpectations();
            request.VerifyAllExpectations();
            _attribute.VerifyAllExpectations();
        }

        private class TestCachePolicy : HttpCachePolicyBase
        {
            public TestCachePolicy()
            {
                MaxAge = TimeSpan.FromSeconds(5);
                ValidationCallbackSet = false;
            }

            public TimeSpan MaxAge { get; set; }
            public bool ValidationCallbackSet { get; set; }

            public override void SetProxyMaxAge(TimeSpan delta)
            {
                MaxAge = delta;
            }

            public override void AddValidationCallback(HttpCacheValidateHandler handler, object data)
            {
                ValidationCallbackSet = true;
            }
        }
    }
}
