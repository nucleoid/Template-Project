using System.Web.WebPages;
using Microsoft.Web.Helpers;
using TemplateProject.Tasks.CustomContracts;

namespace TemplateProject.Web.Mvc.Wrappers
{
    public class ReCaptchaWrapper : ICaptchaTasks
    {
        public bool Validate(string privateKey = null)
        {
            return ReCaptcha.Validate(privateKey);
        }

        public HelperResult GetHtml(string publicKey = null, string theme = "red", string language = "en", int tabIndex = 0)
        {
            return ReCaptcha.GetHtml(publicKey, theme, language, tabIndex);
        }
    }
}