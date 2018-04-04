using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace MySqlEfCoreConsole.Migrations
{
    public partial class AddedMetadataTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MetaData",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    DataSeeded = table.Column<bool>(nullable: false),
                    StatusAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MetaData", x => x.Id);
                });

            SeedMetadata(migrationBuilder);
        }

        private void SeedMetadata(MigrationBuilder migrationBuilder)
        {
            // insert some sample seed data
            migrationBuilder.Sql(@"INSERT INTO metadata (Id, DataSeeded, StatusAt) VALUES
                (1, b'0', '2000-01-01 00:00:01.000000');");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MetaData");
        }
    }
}
