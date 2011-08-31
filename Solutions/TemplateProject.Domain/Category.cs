
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharpArch.Domain.DomainModel;

namespace TemplateProject.Domain
{
    public class Category : Entity
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [Required(ErrorMessage = "Must have a name")]
        public virtual string Name { get; set; }

        public virtual IList<Product> Products { get; set; }
    }
}
