using System;
using System.Collections.Generic;
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

        public Product CreateOrUpdate(Product product)
        {
            if(product.Id == 0)
                product.Created = DateTime.Now;
            product.Modified = DateTime.Now;
            _productRepository.SaveOrUpdate(product);
            return product;
        }

        public void Delete(int id)
        {
            var product = _productRepository.Get(id);
            _productRepository.Delete(product);
        }

        public void Delete(Product product)
        {
            _productRepository.Delete(product);
        }
    }
}
