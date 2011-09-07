
using System.Web.Mvc;

namespace TemplateProject.Domain.Validation
{
    public class ModelClientNameValidationRule : ModelClientValidationRule
    {
        public ModelClientNameValidationRule(string errorMessage, string letterRegex)
        {
            ErrorMessage = errorMessage;
            ValidationType = "namefirstlettercaps";
            ValidationParameters.Add("letterregex", letterRegex);
        }
    }
}
