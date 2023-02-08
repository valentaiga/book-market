using FluentMigrator;

namespace Application.Migrations;

[Migration(1)]
public class CreateTables : Migration
{
    public override void Up()
    {
        Execute.Sql("CREATE EXTENSION IF NOT EXISTS \"uuid-ossp\";");
        Create.Table("books")
            .WithColumn("id").AsGuid().PrimaryKey().WithDefault(SystemMethods.NewGuid)
            .WithColumn("title").AsString(60).NotNullable()
            .WithColumn("description").AsString(512).NotNullable()
            .WithColumn("publish_date").AsDate().Nullable()
            .WithColumn("pages_count").AsInt16().NotNullable()
            .WithColumn("author_id").AsGuid().NotNullable()
            .WithColumn("language").AsString(20).NotNullable();
    }

    public override void Down()
    {
        Delete.Table("books");
    }
}