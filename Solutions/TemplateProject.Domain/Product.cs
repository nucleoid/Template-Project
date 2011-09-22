using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using TemplateProject.Domain.Validation.Attributes;

namespace TemplateProject.Domain
{
    using SharpArch.Domain.DomainModel;

    [DataContract]
    public class Product : Entity
    {
        private Category _category;

        [DataMember]
        public virtual DateTime Created { get; set; }

        [DataMember]
        public virtual DateTime Modified { get; set; }

        [DataMember]
        [Required(ErrorMessage = "Must have a name")]
        [NameValidation]
        public virtual string Name { get; set; }

        //TODO Mitch Figure out how to either add proxy types as known types for serialization or a cleaner way of scrubbing proxy type
        [DataMember]
        [Required(ErrorMessage = "Must have a category")]
        public virtual Category Category
        {
            get { return _category != null ? _category.Self : null; }
            set { _category = value; }
        }

        [DataMember]
        public virtual Availability DefaultAvailability { get; set; }

        [DataMember]
        public virtual FlaggedAvailability? MultipleAvailability { get; set; }
    }
}