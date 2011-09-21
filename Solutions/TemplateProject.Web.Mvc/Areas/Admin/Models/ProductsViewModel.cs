
using System.Collections.Generic;
using System.Runtime.Serialization;
using TemplateProject.Domain;

namespace TemplateProject.Web.Mvc.Areas.Admin.Models
{
    [DataContract]
    public class ProductsViewModel
    {
        [DataMember]
        public IList<Product> Products { get; set; }
    }
}