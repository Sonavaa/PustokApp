using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PustokApp.Data.Migrations
{
    public partial class Added_IsIdentity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIdentity",
                table: "Tags",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdentity",
                table: "Products",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdentity",
                table: "ProductImages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsIdentity",
                table: "Categories",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIdentity",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "IsIdentity",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsIdentity",
                table: "ProductImages");

            migrationBuilder.DropColumn(
                name: "IsIdentity",
                table: "Categories");
        }
    }
}
