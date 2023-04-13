using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ObaGoupDataAccess.Migrations
{
    public partial class _44 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Position",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Position",
                table: "AspNetUsers");
        }
    }
}
