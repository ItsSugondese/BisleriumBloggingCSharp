using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Blogging.Migrations
{
    /// <inheritdoc />
    public partial class BlogReactMappingsReaction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Reaction",
                table: "BlogReactMappings",
                type: "text",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Reaction",
                table: "BlogReactMappings");
        }
    }
}
