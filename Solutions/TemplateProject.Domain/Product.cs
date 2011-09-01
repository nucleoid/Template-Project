using System;
using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Domain
{
    using SharpArch.Domain.DomainModel;

    public class Product : Entity
    {
        public Product()
        {
            var now = DateTime.Now;
            Created = now;
            Modified = now;
        }

        public virtual DateTime Created { get; set; }

        public virtual DateTime Modified { get; set; }

        [Required(ErrorMessage = "Must have a name")]
        public virtual string Name { get; set; }

        public virtual Category Category { get; set; }

    }
}