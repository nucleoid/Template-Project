using System;
using System.Web.Mvc;
using SharpArch.Web.Mvc.ModelBinder;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Binders
{
    public class FlaggedAvailabilityBinder : SharpModelBinder
    {
        private const string FullIdPropertyName = "Product.MultipleAvailability";

        public override object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
        {
            var id = bindingContext.ValueProvider.GetValue(FullIdPropertyName);
            if(id != null && !string.IsNullOrEmpty(id.AttemptedValue))
            {
                FlaggedAvailability? bound = null;
                var values = id.AttemptedValue.Split(',');
                foreach (var value in values)
                {
                    FlaggedAvailability parsed;
                    if (Enum.TryParse(value, out parsed))
                    {
                        if (!bound.HasValue)
                            bound = parsed;
                        else
                            bound |= parsed;
                    }
                }
                return bound;
            }
            return null;
        }
    }
}