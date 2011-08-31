
using System.Collections.Generic;

namespace TemplateProject.Domain.Contracts.Tasks
{
    public interface ICategoryTasks
    {
        IList<Category> GetAll();
        Category Get(int productId);
    }
}
