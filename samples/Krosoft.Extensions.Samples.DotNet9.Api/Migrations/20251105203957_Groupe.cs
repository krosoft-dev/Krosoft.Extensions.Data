using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Krosoft.Extensions.Samples.DotNet9.Api.Migrations
{
    /// <inheritdoc />
    public partial class Groupe : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupeId",
                schema: "Activite",
                table: "Logiciels",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Groupes",
                schema: "Activite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groupes", x => x.Id);
                });

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("1f39c60d-4f92-4f17-aa45-99e8f86a3b3a"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("3e8f94fd-03ea-47b0-b8c5-20b14c361fce"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("4560f48d-4d86-4b1e-8b22-51c1a9e3d6ca"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("6c8a012b-cc8a-4da0-85a5-c9a87bb1a20a"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("9b5ccfd1-8c3e-4a0c-b4af-543a6f87042d"),
                column: "GroupeId",
                value: null);

            migrationBuilder.UpdateData(
                schema: "Activite",
                table: "Logiciels",
                keyColumn: "Id",
                keyValue: new Guid("c40e3ff1-4f3a-4a3a-8b0c-c52a45e6e7b6"),
                column: "GroupeId",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Logiciels_GroupeId",
                schema: "Activite",
                table: "Logiciels",
                column: "GroupeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Logiciels_Groupes_GroupeId",
                schema: "Activite",
                table: "Logiciels",
                column: "GroupeId",
                principalSchema: "Activite",
                principalTable: "Groupes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Logiciels_Groupes_GroupeId",
                schema: "Activite",
                table: "Logiciels");

            migrationBuilder.DropTable(
                name: "Groupes",
                schema: "Activite");

            migrationBuilder.DropIndex(
                name: "IX_Logiciels_GroupeId",
                schema: "Activite",
                table: "Logiciels");

            migrationBuilder.DropColumn(
                name: "GroupeId",
                schema: "Activite",
                table: "Logiciels");
        }
    }
}
