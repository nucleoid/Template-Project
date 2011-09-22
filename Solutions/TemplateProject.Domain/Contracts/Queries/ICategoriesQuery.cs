using MvcContrib.Pagination;

namespace TemplateProject.Domain.Contracts.Queries
{
    public interface ICategoriesQuery
    {
        IPagination<Category> GetPagedList(int page, int size);
    }
}
