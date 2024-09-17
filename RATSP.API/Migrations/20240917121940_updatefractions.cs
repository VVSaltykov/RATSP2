using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RATSP.API.Migrations
{
    /// <inheritdoc />
    public partial class updatefractions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Sanctionality",
                table: "Fractions",
                type: "boolean",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Sanctionality",
                table: "Fractions");
        }
    }
}
