using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordJunkDrawer.App.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DiscordGuilds",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordGuilds", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DiscordRoles",
                columns: table => new
                {
                    Id = table.Column<ulong>(nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ServerId = table.Column<ulong>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    DiscordGuildModelId = table.Column<ulong>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRoles_DiscordGuilds_DiscordGuildModelId",
                        column: x => x.DiscordGuildModelId,
                        principalTable: "DiscordGuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRoles_DiscordGuildModelId",
                table: "DiscordRoles",
                column: "DiscordGuildModelId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DiscordRoles");

            migrationBuilder.DropTable(
                name: "DiscordGuilds");
        }
    }
}
