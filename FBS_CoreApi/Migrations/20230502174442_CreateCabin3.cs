using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBS_CoreApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateCabin3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "Cabins");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Cabins",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }
    }
}
