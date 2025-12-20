using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class SchemaCleanup : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Drop unused columns from shipments
            migrationBuilder.DropColumn(
                name: "carrier",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "incoterm",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "insurance_required",
                table: "shipments");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Recreate dropped columns for rollback
            migrationBuilder.AddColumn<string>(
                name: "carrier",
                table: "shipments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "UPS")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "incoterm",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "insurance_required",
                table: "shipments",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }
    }
}
