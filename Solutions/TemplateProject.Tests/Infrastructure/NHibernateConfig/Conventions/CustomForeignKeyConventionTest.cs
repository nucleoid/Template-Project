using System.Reflection;
using MbUnit.Framework;
using TemplateProject.Domain;
using TemplateProject.Infrastructure.NHibernateConfig.Conventions;

namespace TemplateProject.Tests.Infrastructure.NHibernateConfig.Conventions
{
    [TestFixture]
    public class CustomForeignKeyConventionTest
    {
        [Test]
        public void GetKeyName_With_Null_Property()
        {
            //Arrange
            var type = typeof (Product);
            var convention = new CustomForeignKeyConvention();

            //Act
            MethodInfo method = convention.GetType().GetMethod("GetKeyName", BindingFlags.Instance | BindingFlags.NonPublic);
            var name = method.Invoke(convention, new object[] {null, type}) as string;

            //Assert
            Assert.AreEqual("ProductId", name);
        }
    }
}
