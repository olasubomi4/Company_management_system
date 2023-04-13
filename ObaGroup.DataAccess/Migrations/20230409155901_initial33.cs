using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObaGoupDataAccess.Migrations
{
    public partial class initial33 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EventViewModels",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    start = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    end = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    allDay = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EventViewModels", x => x.id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EventViewModels");
        }
    }
}
