
using System;
using MbUnit.Framework;
using TemplateProject.Infrastructure.FluentMigrations;

namespace TemplateProject.Tests.Infrastructure.FluentMigrations
{
    [TestFixture]
    public class MigrationSafeTest
    {
        [Test, ExpectedException(typeof(NotSupportedException), "Backward migration not supported")]
        public void Down_Throws_Exception()
        {
            //Act
            new TestMigration().Down();
        }

        private class TestMigration : MigrationSafe
        {
            public override void Up()
            {
                throw new NotImplementedException();
            }
        }
    }
}
