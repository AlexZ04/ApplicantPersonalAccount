using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantPersonalAccount.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddInfoForEvents : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InfoForEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    EducationPlace = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false),
                    SocialNetwork = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InfoForEvents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InfoForEvents_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InfoForEvents_UserId",
                table: "InfoForEvents",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InfoForEvents");
        }
    }
}
