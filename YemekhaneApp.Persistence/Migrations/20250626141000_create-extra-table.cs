using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace YemekhaneApp.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class createextratable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Extras",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Extras", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MealRecordExtras",
                columns: table => new
                {
                    ExtrasId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MealRecordsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MealRecordExtras", x => new { x.ExtrasId, x.MealRecordsId });
                    table.ForeignKey(
                        name: "FK_MealRecordExtras_Extras_ExtrasId",
                        column: x => x.ExtrasId,
                        principalTable: "Extras",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_MealRecordExtras_MealRecords_MealRecordsId",
                        column: x => x.MealRecordsId,
                        principalTable: "MealRecords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MealRecordExtras_MealRecordsId",
                table: "MealRecordExtras",
                column: "MealRecordsId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MealRecordExtras");

            migrationBuilder.DropTable(
                name: "Extras");
        }
    }
}
