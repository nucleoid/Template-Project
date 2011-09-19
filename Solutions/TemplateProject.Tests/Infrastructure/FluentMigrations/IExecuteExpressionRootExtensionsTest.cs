
using FluentMigrator.Builders.Execute;
using MbUnit.Framework;
using Rhino.Mocks;
using TemplateProject.Infrastructure.FluentMigrations;

namespace TemplateProject.Tests.Infrastructure.FluentMigrations
{
    [TestFixture]
    public class IExecuteExpressionRootExtensionsTest
    {
        [Test]
        public void ExecuteScriptLocal_Executes_Script_From_Correct_Location()
        {
            //Arrange
            var root = MockRepository.GenerateMock<IExecuteExpressionRoot>();
            const string location = "FluentMigrations.Migrations.Scripts.aspNetAppServices.sql";
            root.Expect(x => x.EmbeddedScript(Arg<string>.Matches(y => y.Contains(location))));

            //Act
            root.ScriptLocal("Migrations.Scripts.aspNetAppServices.sql");

            //Assert
            root.VerifyAllExpectations();
        }
    }
}
