using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GatherBuddy.Web.Database.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FishRecords",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    BaitId = table.Column<uint>(type: "int unsigned", nullable: false),
                    CatchId = table.Column<uint>(type: "int unsigned", nullable: false),
                    Timestamp = table.Column<int>(type: "int", nullable: false),
                    Effects = table.Column<uint>(type: "int unsigned", nullable: false),
                    Bite = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    Perception = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    Gathering = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    FishingSpot = table.Column<ushort>(type: "smallint unsigned", nullable: false),
                    TugAndHook = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    Amount = table.Column<byte>(type: "tinyint unsigned", nullable: false),
                    PositionX = table.Column<float>(type: "float", nullable: false),
                    PositionY = table.Column<float>(type: "float", nullable: false),
                    PositionZ = table.Column<float>(type: "float", nullable: false),
                    Rotation = table.Column<float>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FishRecords", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "SecretKeys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Key = table.Column<string>(type: "longtext", nullable: false),
                    Expiry = table.Column<DateTime>(type: "datetime(6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecretKeys", x => x.Id);
                })
                .Annotation("MySQL:Charset", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FishRecords");

            migrationBuilder.DropTable(
                name: "SecretKeys");
        }
    }
}
