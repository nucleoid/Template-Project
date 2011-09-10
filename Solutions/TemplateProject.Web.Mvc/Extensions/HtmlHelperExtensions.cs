using System;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;
using System.Web.Routing;

namespace TemplateProject.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, object value)
        {
            return CheckBoxWithValue(htmlHelper, name, false, value, null);
        }

        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, bool isChecked, object value)
        {
            return CheckBoxWithValue(htmlHelper, name, isChecked, value, null);
        }

        /// <summary>
        /// Outputs a checkbox with a value other than true/false
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="isChecked"></param>
        /// <param name="value"></param>
        /// <param name="htmlAttributes"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, bool isChecked, object value, object htmlAttributes)
        {
            if (name == null)
                throw new ArgumentNullException("name");
            var tagBuilder = new TagBuilder("input");
            tagBuilder.MergeAttributes(new RouteValueDictionary(htmlAttributes));
            tagBuilder.MergeAttribute("type", "checkbox");
            tagBuilder.MergeAttribute("name", name, true);
            if (isChecked)
                tagBuilder.MergeAttribute("checked", "checked");
            tagBuilder.MergeAttribute("value", value.ToString(), true);
            tagBuilder.GenerateId(name);
            return htmlHelper.Raw(tagBuilder.ToString(TagRenderMode.SelfClosing));
        }

        /// <summary>
        /// Safely routes a link to a specific area.
        /// </summary>
        public static IHtmlString ActionLinkArea<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string linkText, string area) where TController : Controller
        {
            return ActionLinkArea(htmlHelper, action, linkText, area, null);
        }

        /// <summary>
        /// Safely routes a link to a specific area.
        /// </summary>
        public static IHtmlString ActionLinkArea<TController>(this HtmlHelper htmlHelper, Expression<Action<TController>> action, string linkText, string area, object htmlAttributes) where TController : Controller
        {
            var routingValues = Microsoft.Web.Mvc.Internal.ExpressionHelper.GetRouteValuesFromExpression(action);
            routingValues.Add("area", area);
            return htmlHelper.RouteLink(linkText, routingValues, HtmlHelper.AnonymousObjectToHtmlAttributes(htmlAttributes));
        }
    }
}