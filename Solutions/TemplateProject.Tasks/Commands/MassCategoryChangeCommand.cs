using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using SharpArch.Domain.Commands;

namespace TemplateProject.Tasks.Commands
{
    public class MassCategoryChangeCommand : CommandBase
    {
        public MassCategoryChangeCommand(int categoryId, IEnumerable<int> productIds)
        {
            CategoryId = categoryId;
            ProductIds = productIds;
        }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        public IEnumerable<int> ProductIds { get; set; }
    }
}
