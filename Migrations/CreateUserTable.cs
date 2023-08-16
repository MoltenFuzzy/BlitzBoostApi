using FluentMigrator;
using FluentMigrator.Postgres;

[Migration(1)]
public class CreateUserTable : Migration
{
    public override void Up()
    {
        Create
            .Table("User")
            .WithColumn("id")
            .AsInt32()
            .PrimaryKey()
            .Identity()
            .WithColumn("name")
            .AsString(255)
            .NotNullable()
            .WithColumn("email")
            .AsString(255)
            .NotNullable()
            .WithColumn("created_at")
            .AsDateTime()
            .NotNullable()
            .WithColumn("updated_at")
            .AsDateTime()
            .NotNullable();
    }

    public override void Down()
    {
        Delete.Table("User");
    }
}
