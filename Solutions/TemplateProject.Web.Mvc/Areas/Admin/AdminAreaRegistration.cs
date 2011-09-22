using System.Web.Mvc;
using System.Web.Routing;

namespace TemplateProject.Web.Mvc.Areas.Admin
{
    public class AdminAreaRegistration : AreaRegistration
    {
        public override string AreaName
        {
            get
            {
                return "Admin";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context)
        {
            context.MapRoute(
                "get-object",
                "Admin/{controller}/{id}",
                new { action = "Index", id = UrlParameter.Optional },
                new { httpMethod = new HttpMethodConstraint("GET"), }
            );

            context.MapRoute(
                "post-object",
                "Admin/{controller}",
                new { action = "Save" },
                new { httpMethod = new HttpMethodConstraint("POST") }
            );

            context.MapRoute(
                "put-object",
                "Admin/{controller}/{id}",
                new { action = "Save" },
                new { httpMethod = new HttpMethodConstraint("PUT") }
            );

            context.MapRoute(
                "delete-object",
                "Admin/{controller}/{id}",
                new { action = "Delete" },
                new { httpMethod = new HttpMethodConstraint("DELETE") }
            );

            context.MapRoute(
                "Admin_default",
                "Admin/{controller}/{action}/{id}",
                new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
