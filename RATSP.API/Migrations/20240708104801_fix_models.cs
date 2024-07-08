using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RATSP.API.Migrations
{
    /// <inheritdoc />
    public partial class fix_models : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Companies_Fractions_FractionId",
                table: "Companies");

            migrationBuilder.DropIndex(
                name: "IX_Companies_FractionId",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "FractionId",
                table: "Companies");

            migrationBuilder.AddColumn<Guid>(
                name: "CompanyId",
                table: "Fractions",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Fractions_CompanyId",
                table: "Fractions",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_Fractions_Companies_CompanyId",
                table: "Fractions",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Fractions_Companies_CompanyId",
                table: "Fractions");

            migrationBuilder.DropIndex(
                name: "IX_Fractions_CompanyId",
                table: "Fractions");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "Fractions");

            migrationBuilder.AddColumn<int>(
                name: "FractionId",
                table: "Companies",
                type: "integer",
                nullable: false,
                defaultValue: 0);

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
    }
}
