
using System.Data.SqlClient;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.SqlServer;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using MbUnit.Framework;
using Rhino.Mocks;
using Rhino.Mocks.Interfaces;
using TemplateProject.Infrastructure.FluentMigrations;

namespace TemplateProject.Tests.Infrastructure.FluentMigrations
{
    [TestFixture]
    public class RunnerTest
    {
        [Test]
        public void Constructor_Sets_Defaults()
        {
            //Act
            var runner = new Runner(null, null);

            //Assert
            Assert.IsInstanceOfType<NullAnnouncer>(runner.Announcer);
            Assert.IsInstanceOfType<ProcessorOptions>(runner.Options);
            Assert.IsInstanceOfType<SqlServer2008Generator>(runner.MigrationGenerator);
        }

        [Test]
        public void Run_Migrates_DB()
        {
            //Arrange
            var announcer = MockRepository.GenerateMock<IAnnouncer>();
            var migrationGenerator = MockRepository.GenerateMock<IMigrationGenerator>();
            var migrationProcessorOptions = MockRepository.GenerateMock<IMigrationProcessorOptions>();
            var processor = MockRepository.GenerateStub<IMigrationProcessor>();
            var migrationRunner = MockRepository.GenerateMock<IMigrationRunner>();
            migrationRunner.Expect(x => x.MigrateUp());
            var runner = MockRepository.GenerateMock<Runner>(string.Empty, null);
            runner.Announcer = announcer;
            runner.MigrationGenerator = migrationGenerator;
            runner.Options = migrationProcessorOptions;
            runner.Expect(x => x.GenerateProcessor(Arg<SqlConnection>.Is.Anything)).Return(processor);
            runner.Expect(x => x.GenerateRunner(Arg<IRunnerContext>.Is.Anything, Arg<IMigrationProcessor>.Is.Anything)).Return(migrationRunner);
            runner.Expect(x => x.Run()).CallOriginalMethod(OriginalCallOptions.CreateExpectation);

            //Act
            runner.Run();

            //Assert
            migrationRunner.VerifyAllExpectations();
            runner.VerifyAllExpectations();
        }
    }
}
