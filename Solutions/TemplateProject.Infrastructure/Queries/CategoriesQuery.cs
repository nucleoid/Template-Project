using MvcContrib.Pagination;
using SharpArch.NHibernate;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Queries;

namespace TemplateProject.Infrastructure.Queries
{
    public class CategoriesQuery : NHibernateQuery, ICategoriesQuery
    {
        public IPagination<Category> GetPagedList(int page, int size)
        {
            var query = Session.QueryOver<Category>().OrderBy(x => x.Name).Asc;

            var count = query.ToRowCountQuery();
            var totalCount = count.FutureValue<int>();

            var firstResult = (page - 1) * size;

            var models = query.Skip(firstResult).Take(size).Future<Category>();

            return new CustomPagination<Category>(models, page, size, totalCount.Value);
        }
    }
}
