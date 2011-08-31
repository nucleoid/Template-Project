using System.Collections.Generic;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;

namespace TemplateProject.Tasks
{
    public class CategoryTasks
    {
        private readonly INHibernateRepository<Category> _productRepository;

        public CategoryTasks(INHibernateRepository<Category> categoryRepository)
        {
            _productRepository = categoryRepository;
        }

        public IList<Category> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Category Get(int productId)
        {
            return _productRepository.Get(productId);
        }
    }
}
