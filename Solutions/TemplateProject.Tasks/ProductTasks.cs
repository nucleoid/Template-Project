using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;

namespace TemplateProject.Tasks
{
    public class ProductTasks : IProductTasks
    {
        private readonly INHibernateRepository<Product> _productRepository;

        public ProductTasks(INHibernateRepository<Product> categoryRepository)
        {
            _productRepository = categoryRepository;
        }

        public IList<Product> GetAll()
        {
            return _productRepository.GetAll();
        }

        public Product Get(int productId)
        {
            return _productRepository.Get(productId);
        }
    }
}
