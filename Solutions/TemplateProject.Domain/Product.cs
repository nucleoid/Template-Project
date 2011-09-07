using System;
using System.ComponentModel.DataAnnotations;
using TemplateProject.Domain.Validation.Attributes;

namespace TemplateProject.Domain
{
    using SharpArch.Domain.DomainModel;

    public class Product : Entity
    {
        public virtual DateTime Created { get; set; }

        public virtual DateTime Modified { get; set; }

        [Required(ErrorMessage = "Must have a name")]
        [NameValidation]
        public virtual string Name { get; set; }

        [Required(ErrorMessage = "Must have a category")]
        public virtual Category Category { get; set; }

    }
}