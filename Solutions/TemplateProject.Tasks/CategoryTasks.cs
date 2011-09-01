using System;
using System.Collections.Generic;
using SharpArch.NHibernate.Contracts.Repositories;
using TemplateProject.Domain;
using TemplateProject.Domain.Contracts.Tasks;

namespace TemplateProject.Tasks
{
    public class CategoryTasks : ICategoryTasks
    {
        private readonly INHibernateRepository<Category> _categoryRepository;

        public CategoryTasks(INHibernateRepository<Category> categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IList<Category> GetAll()
        {
            return _categoryRepository.GetAll();
        }

        public Category Get(int productId)
        {
            return _categoryRepository.Get(productId);
        }

        public Category CreateOrUpdate(Category category)
        {
            category.Modified = DateTime.Now;
            _categoryRepository.SaveOrUpdate(category);
            return category;
        }

        public void Delete(int id)
        {
            var category = _categoryRepository.Get(id);
            _categoryRepository.Delete(category);
        }

        public void Delete(Category category)
        {
            _categoryRepository.Delete(category);
        }
    }
}
