
using MvcContrib.Pagination;
using TemplateProject.Domain;

namespace TemplateProject.Infrastructure.Queries
{
    public interface IProductsQuery
    {
        IPagination<Product> GetPagedList(int page, int size);
    }
}
