using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StudentReservations.Migrations
{
    public partial class grouptable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Group",
                table: "Reservations");

            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Reservations",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Groups",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ReservationCount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Groups", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Reservations_GroupId",
                table: "Reservations",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservations_Groups_GroupId",
                table: "Reservations",
                column: "GroupId",
                principalTable: "Groups",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservations_Groups_GroupId",
                table: "Reservations");

            migrationBuilder.DropTable(
                name: "Groups");

            migrationBuilder.DropIndex(
                name: "IX_Reservations_GroupId",
                table: "Reservations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Reservations");

            migrationBuilder.AddColumn<string>(
                name: "Group",
                table: "Reservations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
