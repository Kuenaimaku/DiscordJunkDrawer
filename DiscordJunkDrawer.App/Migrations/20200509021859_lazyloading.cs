using Microsoft.EntityFrameworkCore.Migrations;

namespace DiscordJunkDrawer.App.Migrations
{
    public partial class lazyloading : Migration
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
                    Name = table.Column<string>(nullable: true),
                    GuildId = table.Column<ulong>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DiscordRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DiscordRoles_DiscordGuilds_GuildId",
                        column: x => x.GuildId,
                        principalTable: "DiscordGuilds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DiscordRoles_GuildId",
                table: "DiscordRoles",
                column: "GuildId");
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
