using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantPersonalAccount.Persistence.Migrations.Files
{
    /// <inheritdoc />
    public partial class DocumentInfoRelation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumentId",
                table: "EducationInfos",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_EducationInfos_DocumentId",
                table: "EducationInfos",
                column: "DocumentId");

            migrationBuilder.AddForeignKey(
                name: "FK_EducationInfos_Documents_DocumentId",
                table: "EducationInfos",
                column: "DocumentId",
                principalTable: "Documents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EducationInfos_Documents_DocumentId",
                table: "EducationInfos");

            migrationBuilder.DropIndex(
                name: "IX_EducationInfos_DocumentId",
                table: "EducationInfos");

            migrationBuilder.DropColumn(
                name: "DocumentId",
                table: "EducationInfos");
        }
    }
}
