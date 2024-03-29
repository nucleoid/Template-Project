﻿using FluentMigrator;

namespace TemplateProject.Infrastructure.FluentMigrations.Migrations
{
    /// <summary>
    /// Created using the Migration Template located at the root of this project solution.
    /// In order to use it you will need to import it into your ReSharper File Templates.
    /// The template takes care of boiler plate code and the migration attribute number.
    /// </summary>
    [Migration(20110919093608)]
    public class Migration_0002 : MigrationSafe
    {
        public override void Up()
        {
            Execute.ScriptLocal("aspNetAppServices.sql");
        }
    }
}