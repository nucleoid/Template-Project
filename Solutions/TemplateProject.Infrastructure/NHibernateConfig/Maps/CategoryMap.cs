using FluentNHibernate.Automapping;
using FluentNHibernate.Automapping.Alterations;
using TemplateProject.Domain;

namespace TemplateProject.Infrastructure.NHibernateConfig.Maps
{
    public class CategoryMap : IAutoMappingOverride<Category>
    {
        public void Override(AutoMapping<Category> mapping)
        {
            mapping.Map(x => x.Created).Column("Created").Not.Update();
        }
    }
}
