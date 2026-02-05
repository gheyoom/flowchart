using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FlowChartApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FlowBoxes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Name = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    DescriptionArabic = table.Column<string>(type: "TEXT", nullable: true),
                    LinkLeftUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LinkLeftText = table.Column<string>(type: "TEXT", nullable: true),
                    LinkMiddleUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LinkMiddleText = table.Column<string>(type: "TEXT", nullable: true),
                    LinkRightUrl = table.Column<string>(type: "TEXT", nullable: true),
                    LinkRightText = table.Column<string>(type: "TEXT", nullable: true),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false),
                    PosX = table.Column<double>(type: "REAL", nullable: false),
                    PosY = table.Column<double>(type: "REAL", nullable: false),
                    Width = table.Column<string>(type: "TEXT", nullable: true),
                    Height = table.Column<string>(type: "TEXT", nullable: true),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    BorderColor = table.Column<string>(type: "TEXT", nullable: false),
                    BorderStyle = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowBoxes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LegendItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    BackgroundColor = table.Column<string>(type: "TEXT", nullable: false),
                    BorderColor = table.Column<string>(type: "TEXT", nullable: false),
                    BorderStyle = table.Column<string>(type: "TEXT", nullable: false),
                    OrderIndex = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LegendItems", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FlowConnections",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    SourceBoxId = table.Column<int>(type: "INTEGER", nullable: false),
                    TargetBoxId = table.Column<int>(type: "INTEGER", nullable: false),
                    ConnectionType = table.Column<string>(type: "TEXT", nullable: false),
                    Color = table.Column<string>(type: "TEXT", nullable: false),
                    Direction = table.Column<string>(type: "TEXT", nullable: false),
                    LabelIcon = table.Column<string>(type: "TEXT", nullable: true),
                    Waypoints = table.Column<string>(type: "TEXT", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlowConnections", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FlowConnections_FlowBoxes_SourceBoxId",
                        column: x => x.SourceBoxId,
                        principalTable: "FlowBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_FlowConnections_FlowBoxes_TargetBoxId",
                        column: x => x.TargetBoxId,
                        principalTable: "FlowBoxes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FlowConnections_SourceBoxId",
                table: "FlowConnections",
                column: "SourceBoxId");

            migrationBuilder.CreateIndex(
                name: "IX_FlowConnections_TargetBoxId",
                table: "FlowConnections",
                column: "TargetBoxId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FlowConnections");

            migrationBuilder.DropTable(
                name: "LegendItems");

            migrationBuilder.DropTable(
                name: "FlowBoxes");
        }
    }
}
