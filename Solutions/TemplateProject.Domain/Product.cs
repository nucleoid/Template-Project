using System.ComponentModel.DataAnnotations;

namespace TemplateProject.Domain
{
    using SharpArch.Domain.DomainModel;

    public class Product : Entity
    {
        [Required(ErrorMessage = "Must have a name")]
        public virtual string Name { get; set; }
    }
}