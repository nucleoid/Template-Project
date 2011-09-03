
using SharpArch.Domain.Commands;
using TemplateProject.Domain.Contracts.Tasks;
using TemplateProject.Tasks.Commands;

namespace TemplateProject.Tasks.CommandHandlers
{
    public class MassCategoryChangeHandler : ICommandHandler<MassCategoryChangeCommand>
    {
        private readonly IProductTasks _productTasks;
        private readonly ICategoryTasks _categoryTasks;

        public MassCategoryChangeHandler(IProductTasks productTasks, ICategoryTasks categoryTasks)
        {
            _productTasks = productTasks;
            _categoryTasks = categoryTasks;
        }

        public ICommandResult Handle(MassCategoryChangeCommand command)
        {
            var category = _categoryTasks.Get(command.CategoryId);

            if (category == null)
                return new MassCategoryChangeResult(false);

            foreach (var productId in command.ProductIds)
            {
                var product = _productTasks.Get(productId);

                if (product == null)
                    return new MassCategoryChangeResult(false);

                if(product.Category.Id != category.Id)
                {
                    product.Category = category;
                    _productTasks.CreateOrUpdate(product);
                }
            }
            return new MassCategoryChangeResult(true);
        }
    }
}
