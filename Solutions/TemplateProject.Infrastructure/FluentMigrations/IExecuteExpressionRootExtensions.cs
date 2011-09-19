using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using FluentMigrator.Builders.Execute;

namespace TemplateProject.Infrastructure.FluentMigrations
{
    public static class IExecuteExpressionRootExtensions
    {
        public static void ScriptLocal(this IExecuteExpressionRoot root, string file)
        {
            var assembly = typeof (MigrationSafe).Assembly;
            var resourceName = assembly.GetManifestResourceNames().SingleOrDefault(x => x.Contains(file));
            root.EmbeddedScript(resourceName);
        }
    }
}
