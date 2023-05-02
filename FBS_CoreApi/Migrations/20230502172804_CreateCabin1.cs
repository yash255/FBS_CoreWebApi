﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FBS_CoreApi.Migrations
{
    /// <inheritdoc />
    public partial class CreateCabin1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoOfSeatsAvailable",
                table: "Flights");

            migrationBuilder.CreateTable(
                name: "Cabins",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NoOfSeats = table.Column<int>(type: "int", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FlightId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cabins", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cabins_Flights_FlightId",
                        column: x => x.FlightId,
                        principalTable: "Flights",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cabins_FlightId",
                table: "Cabins",
                column: "FlightId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cabins");

            migrationBuilder.AddColumn<int>(
                name: "NoOfSeatsAvailable",
                table: "Flights",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
