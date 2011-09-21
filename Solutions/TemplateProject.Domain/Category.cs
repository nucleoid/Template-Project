
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using SharpArch.Domain.DomainModel;

namespace TemplateProject.Domain
{
    [DataContract]
    public class Category : Entity
    {
        public Category()
        {
            Products = new List<Product>();
        }

        [DataMember]
        public virtual DateTime Created { get; set; }

        [DataMember]
        public virtual DateTime Modified { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Must have a name")]
        public virtual string Name { get; set; }

//        [DataMember]      //TODO Mitch Figure out how to resolve issue with circular referencing when serializing
        public virtual IList<Product> Products { get; set; }

        public virtual void AddProduct(Product product)
        {
            if (product == null) 
                throw new ArgumentNullException("product");

            product.Category = this;
            Products.Add(product);
        }

        public virtual string ProductNames
        {
            get
            {
                if (Products.Count == 0)
                    return "N/A";
                return String.Join(", ", Products.Select(p => p.Name));
            }
        }

        public virtual Category Self
        {
            get { return this; }
        }
    }
}
