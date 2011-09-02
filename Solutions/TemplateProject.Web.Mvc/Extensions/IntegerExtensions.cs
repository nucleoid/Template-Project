
namespace TemplateProject.Web.Mvc.Extensions
{
    public static class IntegerExtensions
    {
        public static string PluralizeLabel(this int count, string singular, string plural)
        {
            return count == 1 ? singular : plural;
        }
    }
}