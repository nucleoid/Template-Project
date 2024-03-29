﻿using System.Collections.Generic;

namespace TemplateProject.Domain.Contracts.Tasks
{
    public interface IProductTasks
    {
        IList<Product> GetAll();
        Product Get(int productId);
        Product CreateOrUpdate(Product product);
        void Delete(int id);
        void Delete(Product product);
    }
}
