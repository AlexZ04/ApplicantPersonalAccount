using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ApplicantPersonalAccount.Persistence.Migrations.Files
{
    /// <inheritdoc />
    public partial class FixEducationInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "EducationInfos");

            migrationBuilder.DropColumn(
                name: "Type",
                table: "EducationInfos");

            migrationBuilder.AddColumn<Guid>(
                name: "DocumentTypeId",
                table: "EducationInfos",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DocumentTypeId",
                table: "EducationInfos");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "EducationInfos",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "EducationInfos",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
