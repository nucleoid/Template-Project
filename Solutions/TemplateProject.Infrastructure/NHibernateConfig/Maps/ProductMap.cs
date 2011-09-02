using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using TemplateProject.Domain;

namespace TemplateProject.Infrastructure.NHibernateConfig.Maps
{
    public class ProductMap : IAutoMappingOverride<Product>
    {
        public void Override(AutoMapping<Product> mapping)
        {
            mapping.Map(x => x.Created).Column("Created").Not.Update();
        }
    }
}
