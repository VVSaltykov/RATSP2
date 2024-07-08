using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace RATSP.API.Migrations
{
    /// <inheritdoc />
    public partial class add_fractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "FractionId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Fractions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Start = table.Column<DateOnly>(type: "date", nullable: false),
                    End = table.Column<DateOnly>(type: "date", nullable: false),
                    Value = table.Column<decimal>(type: "numeric", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Fractions", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Companies_FractionId",
                table: "Companies",
                column: "FractionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Companies_Fractions_FractionId",
                table: "Companies",
                column: "FractionId",
                principalTable: "Fractions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Fractions_FractionId",
                table: "Companies");

            migrationBuilder.DropTable(
                name: "Fractions");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FractionId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FractionId",
                table: "Companies");
        }
    }
}
