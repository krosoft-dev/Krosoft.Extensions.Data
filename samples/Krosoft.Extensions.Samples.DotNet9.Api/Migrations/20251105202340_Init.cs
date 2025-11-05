using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Krosoft.Extensions.Samples.DotNet9.Api.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Activite");

            migrationBuilder.EnsureSchema(
                name: "Systeme");

            migrationBuilder.EnsureSchema(
                name: "Referentiel");

            migrationBuilder.CreateTable(
                name: "Categories",
                schema: "Activite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Libelle = table.Column<string>(type: "text", nullable: false),
                    StatutCode = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Langues",
                schema: "Systeme",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    Libelle = table.Column<string>(type: "text", nullable: false),
                    IsDefaut = table.Column<bool>(type: "boolean", nullable: false),
                    IsActif = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Langues", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Pays",
                schema: "Referentiel",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "text", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pays", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Statistique",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: true),
                    Nombre = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistique", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Logiciels",
                schema: "Activite",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Nom = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    CategorieId = table.Column<Guid>(type: "uuid", nullable: false),
                    StatutCode = table.Column<int>(type: "integer", nullable: false),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedAt = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    TenantId = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Logiciels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Logiciels_Categories_CategorieId",
                        column: x => x.CategorieId,
                        principalSchema: "Activite",
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                schema: "Activite",
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Libelle", "StatutCode", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Catégorie 1", 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { new Guid("4560f48d-4d86-4b1e-8b22-51c1a9e3d6ca"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Catégorie 2", 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { new Guid("789e4321-0987-4567-abcd-1234dcba5678"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Catégorie 3", 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null },
                    { new Guid("abcdef12-3456-7890-1234-567890abcdef"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Catégorie 4", 1, new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null }
                });

            migrationBuilder.InsertData(
                schema: "Systeme",
                table: "Langues",
                columns: new[] { "Id", "Code", "IsActif", "IsDefaut", "Libelle" },
                values: new object[,]
                {
                    { new Guid("18d7ae90-4f13-4494-a120-19d3cf8c5821"), "fr", true, true, "fr" },
                    { new Guid("18d7ae90-4f13-4494-a120-19d3cf8c5822"), "en", true, false, "Anglais" }
                });

            migrationBuilder.InsertData(
                schema: "Referentiel",
                table: "Pays",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("aba289b9-bfd0-4ef5-a4b6-efc60812c001"), "fr", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("aba289b9-bfd0-4ef5-a4b6-efc60812c002"), "de", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("aba289b9-bfd0-4ef5-a4b6-efc60812c005"), "it", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("aba289b9-bfd0-4ef5-a4b6-efc60812c006"), "es", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("aba289b9-bfd0-4ef5-a4b6-efc60812c007"), "gb", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" }
                });

            migrationBuilder.InsertData(
                schema: "Activite",
                table: "Logiciels",
                columns: new[] { "Id", "CategorieId", "CreatedAt", "CreatedBy", "Description", "Nom", "StatutCode", "TenantId", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("1f39c60d-4f92-4f17-aa45-99e8f86a3b3a"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Premier logiciel", "Logiciel1", 1, "Fake_Tenant_Id", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Logiciel de lecture de fichiers PDF", "Adobe Acrobat Reader", 1, "Adobe", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("3e8f94fd-03ea-47b0-b8c5-20b14c361fce"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 2, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Deuxième logiciel", "Logiciel2", 2, "Fake_Tenant_Id", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("4560f48d-4d86-4b1e-8b22-51c1a9e3d6ca"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 1, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Logiciel de calcul de tableaux", "Microsoft Excel", 1, "Microsoft", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("6c8a012b-cc8a-4da0-85a5-c9a87bb1a20a"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 3, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Troisième logiciel", "Logiciel3", 1, "Fake_Tenant_Id", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("9b5ccfd1-8c3e-4a0c-b4af-543a6f87042d"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 4, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Quatrième logiciel", "Logiciel4", 2, "Fake_Tenant_Id", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" },
                    { new Guid("c40e3ff1-4f3a-4a3a-8b0c-c52a45e6e7b6"), new Guid("2a7c1a9d-6db3-4d1e-8d8b-9e8431d1b55c"), new DateTimeOffset(new DateTime(2023, 5, 1, 12, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme", "Cinquième logiciel", "Logiciel5", 1, "Fake_Tenant_Id", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "Systeme" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Logiciels_CategorieId",
                schema: "Activite",
                table: "Logiciels",
                column: "CategorieId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Langues",
                schema: "Systeme");

            migrationBuilder.DropTable(
                name: "Logiciels",
                schema: "Activite");

            migrationBuilder.DropTable(
                name: "Pays",
                schema: "Referentiel");

            migrationBuilder.DropTable(
                name: "Statistique");

            migrationBuilder.DropTable(
                name: "Categories",
                schema: "Activite");
        }
    }
}
