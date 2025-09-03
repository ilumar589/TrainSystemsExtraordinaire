using Microsoft.EntityFrameworkCore.Migrations;

namespace Trains.Base.DB.Configurations;

public partial class EnablePgcrypto : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql(@"CREATE EXTENSION IF NOT EXISTS pgcrypto;");
    }
}
