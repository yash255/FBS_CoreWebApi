using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBS_CoreApi.Migrations
{
    /// <inheritdoc />
    public partial class cabinremove : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CabinClass");

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

            migrationBuilder.CreateTable(
                name: "CabinClass",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FlightId = table.Column<int>(type: "int", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Seats = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CabinClass", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CabinClass_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CabinClass_FlightId",
                table: "CabinClass",
                column: "FlightId");
        }
    }
}
