using System.Linq;
using MbUnit.Framework;
using SharpArch.Domain.DomainModel;
using TemplateProject.Domain;
using TemplateProject.Infrastructure.NHibernateConfig;
using TemplateProject.Infrastructure.NHibernateConfig.Conventions;

namespace TemplateProject.Tests.Infrastructure.NHibernateConfig
{
    [TestFixture]
    public class AutoPersistenceModelGeneratorTest
    {
        [Test]
        public void Generate_Configures_Mappings()
        {
            //Act
            var model = new AutoPersistenceModelGenerator().Generate();
            var mappings = model.BuildMappings();

            //Assert
            Assert.IsTrue(mappings.Any(x => x.Classes.Any(y => y.Type == typeof(Product))));
            Assert.IsFalse(mappings.Any(x => x.Classes.Any(y => y.Type == typeof(Entity))));
            Assert.IsFalse(mappings.Any(x => x.Classes.Any(y => y.Type == typeof(EntityWithTypedId<>))));
            Assert.AreEqual(1, model.Conventions.Find<PrimaryKeyConvention>().Count());
            Assert.AreEqual(1, model.Conventions.Find<CustomForeignKeyConvention>().Count());
            Assert.AreEqual(1, model.Conventions.Find<HasManyConvention>().Count());
            Assert.AreEqual(1, model.Conventions.Find<TableNameConvention>().Count());
        }
    }
}
