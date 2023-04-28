using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBS_CoreApi.Migrations
{
    /// <inheritdoc />
    public partial class noOfSeatsAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NoOfSeatsAvailable",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfSeatsAvailable",
                table: "Flights");
        }
    }
}
