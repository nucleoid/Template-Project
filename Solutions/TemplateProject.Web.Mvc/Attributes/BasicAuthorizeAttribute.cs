using System;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Mvc;
using TemplateProject.Tasks.CustomContracts;
using TemplateProject.Web.Mvc.Extensions;
using TemplateProject.Web.Mvc.Results;

namespace TemplateProject.Web.Mvc.Attributes
{
    /// <summary>
    /// Modified from http://cacheandquery.com/blog/2011/03/customizing-asp-net-mvc-basic-authentication/
    /// </summary>
    public class BasicAuthorizeAttribute : AuthorizeAttribute
    {
        public bool RequireSsl { get; set; }

        public IMembershipTasks MembershipTasks { get; set; }

        public BasicAuthorizeAttribute()
        {
            RequireSsl = true;
            MembershipTasks = DependencyResolver.Current.GetService(typeof (IMembershipTasks)) as IMembershipTasks;
        }

        internal virtual void CacheValidateHandler(HttpContext context, object data, ref HttpValidationStatus validationStatus)
        {
            validationStatus = OnCacheAuthorization(new HttpContextWrapper(context));
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext == null) 
                throw new ArgumentNullException("filterContext");

            if (filterContext.HttpContext.Request.IsRestful())
            {
                if (!Authenticate(filterContext.HttpContext))
                    filterContext.Result = new HttpBasicUnauthorizedResult();
                else
                {
                    if (BaseAuthorizeCore(filterContext.HttpContext))
                    {
                        HttpCachePolicyBase cachePolicy = filterContext.HttpContext.Response.Cache;
                        cachePolicy.SetProxyMaxAge(new TimeSpan(0));
                        cachePolicy.AddValidationCallback(CacheValidateHandler, null);
                    }
                    else
                    {
                        filterContext.Result = new HttpBasicUnauthorizedResult();
                    }
                }
            }
            else
                BaseAuthorization(filterContext);
        }

        public virtual bool BaseAuthorizeCore(HttpContextBase httpContext)
        {
            return AuthorizeCore(httpContext);
        }

        public virtual void BaseAuthorization(AuthorizationContext filterContext)
        {
            base.OnAuthorization(filterContext);
        }

        internal virtual bool Authenticate(HttpContextBase context)
        {
            if (RequireSsl && !context.Request.IsSecureConnection && !context.Request.IsLocal) 
                return false;

            if (!context.Request.Headers.AllKeys.Contains("Authorization")) 
                return false;

            string authHeader = context.Request.Headers["Authorization"];

            IPrincipal principal;
            if (TryGetPrincipal(authHeader, out principal))
            {
                HttpContext.Current.User = principal;
                return true;
            }
            return false;
        }

        internal virtual bool TryGetPrincipal(string authHeader, out IPrincipal principal)
        {
            var creds = ParseAuthHeader(authHeader);
            if (creds != null && TryGetPrincipal(creds[0], creds[1], out principal))
                return true;

            principal = null;
            return false;
        }

        internal virtual string[] ParseAuthHeader(string authHeader)
        {
            if (string.IsNullOrEmpty(authHeader) || !authHeader.StartsWith("Basic")) 
                return null;

            string base64Credentials = authHeader.Substring(6);
            string[] credentials = Encoding.ASCII.GetString(Convert.FromBase64String(base64Credentials)).Split(new [] { ':' });

            if (credentials.Length != 2 || string.IsNullOrEmpty(credentials[0]) || string.IsNullOrEmpty(credentials[1])) 
                return null;

            return credentials;
        }

        internal virtual bool TryGetPrincipal(string userName, string password, out IPrincipal principal)
        {
            if (MembershipTasks.ValidateUser(userName, password))
            {
                var rolesForUser = FindRolesFor(userName);
                principal = new GenericPrincipal(new GenericIdentity(userName), rolesForUser);
                return true;
            }
            principal = null;
            return false;
        }

        public virtual string[] FindRolesFor(string username)
        {
            return System.Web.Security.Roles.GetRolesForUser(username);
        }
    }
}