using System.Web.WebPages;

namespace TemplateProject.Tasks.CustomContracts
{
    public interface ICaptchaTasks
    {
        bool Validate(string privateKey = null);
        HelperResult GetHtml(string publicKey = null, string theme = "red", string language = "en", int tabIndex = 0);
    }
}
