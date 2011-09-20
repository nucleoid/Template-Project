using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using SharpArch.Domain.DomainModel;

namespace TemplateProject.Web.Mvc.Models
{
    public class LogOnModel : ValidatableObject
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }

        protected override IEnumerable<PropertyInfo> GetTypeSpecificSignatureProperties()
        {
            return GetType().GetProperties().Where((p => Attribute.IsDefined(p, typeof(DomainSignatureAttribute), true)));
        }
    }
}