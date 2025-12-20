using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations.Auto
{
    /// <inheritdoc />
    public partial class AddShipmentDocumentFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "shipment_documents",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "required",
                table: "shipment_documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "uploaded",
                table: "shipment_documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "name",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "required",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "uploaded",
                table: "shipment_documents");
        }
    }
}
