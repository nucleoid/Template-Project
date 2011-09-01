
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharpArch.Domain.DomainModel;

namespace TemplateProject.Domain
{
    public class Category : Entity
    {
        public Category()
        {
            var now = DateTime.Now;
            Created = now;
            Modified = now;
            Products = new List<Product>();
        }

        public virtual DateTime Created { get; set; }

        public virtual DateTime Modified { get; set; }

        [Required(ErrorMessage = "Must have a name")]
        public virtual string Name { get; set; }

        public virtual IList<Product> Products { get; set; }

        public virtual void AddProduct(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException("product");

            product.Category = this;
            Products.Add(product);
        }
    }
}
