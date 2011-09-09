using FluentMigrator;

namespace TemplateProject.Infrastructure.FluentMigrations.Migrations
{
    /// <summary>
    /// Created using the Migration Template located at the root of this project solution.
    /// In order to use it you will need to import it into your ReSharper File Templates.
    /// The template takes care of boiler plate code and the migration attribute number.
    /// </summary>
    [Migration(20110908023541)]
    public class Migration_001 : MigrationSafe
    {
        public override void Up()
        {
            Create.Table("Categories")
                .WithColumn("CategoryId").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Modified").AsDateTime()
                .WithColumn("Name").AsFixedLengthString(255);

            Create.Table("Products")
                .WithColumn("ProductId").AsInt32().NotNullable().PrimaryKey().Identity()
                .WithColumn("Created").AsDateTime()
                .WithColumn("Modified").AsDateTime()
                .WithColumn("Name").AsFixedLengthString(255)
                .WithColumn("CategoryId").AsInt32()
                .WithColumn("DefaultAvailability").AsFixedLengthString(255)
                .WithColumn("MultipleAvailability").AsFixedLengthString(255);

            Create.ForeignKey("fk_Products_CategoryId_Categories_Id").FromTable("Products")
                .ForeignColumn("CategoryId").ToTable("Categories").PrimaryColumn("CategoryId");
        }
    }
}