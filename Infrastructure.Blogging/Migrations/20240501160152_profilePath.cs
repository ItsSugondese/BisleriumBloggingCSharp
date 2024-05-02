using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Blogging.Migrations
{
    /// <inheritdoc />
    public partial class profilePath : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePath",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePath",
                table: "AspNetUsers");
        }
    }
}
