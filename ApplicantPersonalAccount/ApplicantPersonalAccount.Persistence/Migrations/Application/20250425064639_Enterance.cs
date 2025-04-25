using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantPersonalAccount.Persistence.Migrations.Application
{
    /// <inheritdoc />
    public partial class Enterance : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Enterances",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ApplicantId = table.Column<Guid>(type: "uuid", nullable: false),
                    ManagerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    UpdateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Enterances", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "EnterancePrograms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    Priority = table.Column<int>(type: "integer", nullable: false),
                    EnteranceId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnterancePrograms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnterancePrograms_Enterances_EnteranceId",
                        column: x => x.EnteranceId,
                        principalTable: "Enterances",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnterancePrograms_EnteranceId",
                table: "EnterancePrograms",
                column: "EnteranceId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnterancePrograms");

            migrationBuilder.DropTable(
                name: "Enterances");
        }
    }
}
