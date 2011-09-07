
using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace TemplateProject.Web.Mvc.Extensions
{
    public static class HtmlHelperExtensions
    {
        /// <summary>
        /// Needed for output of flagged enums.
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
        /// Needed for output of flagged enums.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static IHtmlString CheckBoxWithValue(this HtmlHelper htmlHelper, string name, bool isChecked, object value)
        {
            return CheckBoxWithValue(htmlHelper, name, isChecked, value, null);
        }

        /// <summary>
        /// Needed for output of flagged enums.
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <param name="name"></param>
        /// <param name="value"></param>
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
    }
}