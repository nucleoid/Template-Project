using MvcContrib.Pagination;
using SharpArch.NHibernate;
using TemplateProject.Domain;

namespace TemplateProject.Infrastructure.Queries
{
    public class ProductsQuery : NHibernateQuery, IProductsQuery
    {
        public IPagination<Product> GetPagedList(int page, int size)
        {
            var query = Session.QueryOver<Product>().OrderBy(x => x.Name).Asc;

            var count = query.ToRowCountQuery();
            var totalCount = count.FutureValue<int>();

            var firstResult = (page - 1) * size;

            var models = query.Skip(firstResult).Take(size).Future<Product>();

            return new CustomPagination<Product>(models, page, size, totalCount.Value);
        }
    }
}
