using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace backend.Migrations.Auto
{
    /// <inheritdoc />
    public partial class AlignSnapshotWithDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_brokerreviews_broker",
                table: "broker_reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_brokerreviews_shipment",
                table: "broker_reviews");

            migrationBuilder.DropForeignKey(
                name: "fk_notifications_user",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "fk_compliance_shipment",
                table: "shipment_compliance");

            migrationBuilder.DropForeignKey(
                name: "fk_docs_shipment",
                table: "shipment_documents");

            migrationBuilder.DropForeignKey(
                name: "fk_msgs_sender",
                table: "shipment_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_msgs_shipment",
                table: "shipment_messages");

            migrationBuilder.DropForeignKey(
                name: "fk_packages_shipment",
                table: "shipment_packages");

            migrationBuilder.DropForeignKey(
                name: "fk_parties_shipment",
                table: "shipment_parties");

            migrationBuilder.DropForeignKey(
                name: "fk_shipments_createdby",
                table: "shipments");

            migrationBuilder.DropTable(
                name: "ai_findings");

            migrationBuilder.DropTable(
                name: "approval_logs");

            migrationBuilder.DropTable(
                name: "audit_logs");

            migrationBuilder.DropTable(
                name: "broker_requests");

            migrationBuilder.DropTable(
                name: "payments");

            migrationBuilder.DropTable(
                name: "rule_change_requests");

            migrationBuilder.DropTable(
                name: "shipment_items");

            migrationBuilder.DropTable(
                name: "shipment_services");

            migrationBuilder.DropTable(
                name: "shipment_tracking");

            migrationBuilder.DropTable(
                name: "import_export_rules");

            migrationBuilder.DropIndex(
                name: "idx_shipments_reference",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "idx_compliance_ai_status",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "name",
                table: "users");

            migrationBuilder.DropColumn(
                name: "invite_code",
                table: "users");

            migrationBuilder.DropColumn(
                name: "invite_expires_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "carrier",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "name",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "required",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "uploaded",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "verified_by_broker",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "version",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "ai_score",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "ai_status",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "restricted_flag",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "sanctioned_country_flag",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "created_at",
                table: "broker_reviews");

            migrationBuilder.DropColumn(
                name: "status",
                table: "broker_reviews");

            migrationBuilder.RenameIndex(
                name: "IX_shipments_created_by",
                table: "shipments",
                newName: "idx_shipments_created_by");

            migrationBuilder.RenameColumn(
                name: "address2",
                table: "shipment_parties",
                newName: "address_2");

            migrationBuilder.RenameColumn(
                name: "address1",
                table: "shipment_parties",
                newName: "address_1");

            migrationBuilder.RenameIndex(
                name: "IX_shipment_messages_sender_id",
                table: "shipment_messages",
                newName: "idx_msgs_sender");

            migrationBuilder.RenameColumn(
                name: "file_url",
                table: "shipment_documents",
                newName: "file_path");

            migrationBuilder.RenameColumn(
                name: "eccn",
                table: "shipment_compliance",
                newName: "suggested_hs_code");

            migrationBuilder.RenameColumn(
                name: "ai_notes",
                table: "shipment_compliance",
                newName: "required_documents");

            migrationBuilder.RenameIndex(
                name: "IX_broker_reviews_broker_id",
                table: "broker_reviews",
                newName: "idx_brokerreviews_broker");

            migrationBuilder.AlterColumn<string>(
                name: "verification_token",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(200)",
                oldMaxLength: 200,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "tos_accepted_at",
                table: "users",
                type: "datetime(6)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(3)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "company",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "shipment_type",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "International")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "mode",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "Ground")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                table: "shipments",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ai_approval_status",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "ai_compliance_score",
                table: "shipments",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "assigned_broker_id",
                table: "shipments",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "broker_approval_status",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "currency",
                table: "shipments",
                type: "varchar(3)",
                maxLength: 3,
                nullable: true,
                defaultValue: "USD")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "customs_value",
                table: "shipments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "service_level",
                table: "shipments",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "token_generated_at",
                table: "shipments",
                type: "datetime(3)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "weight_unit",
                table: "shipment_packages",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "kg")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "package_type",
                table: "shipment_packages",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Box")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "dimension_unit",
                table: "shipment_packages",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "cm")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "sender_id",
                table: "shipment_messages",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "document_type",
                table: "shipment_documents",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldDefaultValue: "Other")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "file_name",
                table: "shipment_documents",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "file_size",
                table: "shipment_documents",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "mime_type",
                table: "shipment_documents",
                type: "varchar(150)",
                maxLength: 150,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "validation_confidence",
                table: "shipment_documents",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "validation_notes",
                table: "shipment_documents",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "validation_status",
                table: "shipment_documents",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "risk_level",
                table: "shipment_compliance",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10,
                oldDefaultValue: "Low")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "estimated_duty",
                table: "shipment_compliance",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "estimated_tax",
                table: "shipment_compliance",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "evaluated_at",
                table: "shipment_compliance",
                type: "datetime(3)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "missing_documents",
                table: "shipment_compliance",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<decimal>(
                name: "overall_score",
                table: "shipment_compliance",
                type: "decimal(5,2)",
                nullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "notifications",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "notifications",
                keyColumn: "type",
                keyValue: null,
                column: "type",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "notifications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "notifications",
                keyColumn: "message",
                keyValue: null,
                column: "message",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "notifications",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<long>(
                name: "shipment_id",
                table: "notifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "title",
                table: "notifications",
                type: "varchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "action",
                table: "broker_reviews",
                type: "varchar(50)",
                maxLength: 50,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "requested_documents",
                table: "broker_reviews",
                type: "json",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "broker_profiles",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    license_number = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    years_of_experience = table.Column<int>(type: "int", nullable: true),
                    origin_countries = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    destination_countries = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hs_categories = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timezone = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    language = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_available = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    max_concurrent_shipments = table.Column<int>(type: "int", nullable: false, defaultValue: 10),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    updated_at = table.Column<DateTime>(type: "datetime(3)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_broker_profiles", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_broker_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipment_products",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    package_id = table.Column<long>(type: "bigint", nullable: false),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hs_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 1m),
                    unit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "pcs")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unit_price = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    total_value = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    origin_country = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    export_reason = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Sale")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_products", x => x.id);
                    table.ForeignKey(
                        name: "FK_shipment_products_shipment_packages_package_id",
                        column: x => x.package_id,
                        principalTable: "shipment_packages",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_shipment_products_shipments_shipment_id",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipper_profiles",
                columns: table => new
                {
                    user_id = table.Column<long>(type: "bigint", nullable: false),
                    address_line_1 = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    address_line_2 = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    city = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    state = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    postal_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    country_code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: false, defaultValue: "US")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    timezone = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, defaultValue: "America/New_York")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    language = table.Column<string>(type: "varchar(10)", maxLength: 10, nullable: false, defaultValue: "en")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    company_role = table.Column<string>(type: "varchar(150)", maxLength: 150, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    updated_at = table.Column<DateTime>(type: "datetime(3)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipper_profiles", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_shipper_profiles_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_shipments_assigned_broker",
                table: "shipments",
                column: "assigned_broker_id");

            migrationBuilder.CreateIndex(
                name: "idx_shipments_reference",
                table: "shipments",
                column: "reference_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_shipment_documents_uploaded_by",
                table: "shipment_documents",
                column: "uploaded_by");

            migrationBuilder.CreateIndex(
                name: "idx_notifications_shipment",
                table: "notifications",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_products_hscode",
                table: "shipment_products",
                column: "hs_code");

            migrationBuilder.CreateIndex(
                name: "idx_products_package",
                table: "shipment_products",
                column: "package_id");

            migrationBuilder.CreateIndex(
                name: "idx_products_shipment",
                table: "shipment_products",
                column: "shipment_id");

            migrationBuilder.AddForeignKey(
                name: "FK_broker_reviews_shipments_shipment_id",
                table: "broker_reviews",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_broker_reviews_users_broker_id",
                table: "broker_reviews",
                column: "broker_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_shipments_shipment_id",
                table: "notifications",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_notifications_users_user_id",
                table: "notifications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_compliance_shipments_shipment_id",
                table: "shipment_compliance",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_documents_shipments_shipment_id",
                table: "shipment_documents",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_documents_users_uploaded_by",
                table: "shipment_documents",
                column: "uploaded_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_messages_shipments_shipment_id",
                table: "shipment_messages",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_messages_users_sender_id",
                table: "shipment_messages",
                column: "sender_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_packages_shipments_shipment_id",
                table: "shipment_packages",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipment_parties_shipments_shipment_id",
                table: "shipment_parties",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_users_assigned_broker",
                table: "shipments",
                column: "assigned_broker_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_shipments_users_created_by",
                table: "shipments",
                column: "created_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_broker_reviews_shipments_shipment_id",
                table: "broker_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_broker_reviews_users_broker_id",
                table: "broker_reviews");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_shipments_shipment_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_notifications_users_user_id",
                table: "notifications");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_compliance_shipments_shipment_id",
                table: "shipment_compliance");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_documents_shipments_shipment_id",
                table: "shipment_documents");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_documents_users_uploaded_by",
                table: "shipment_documents");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_messages_shipments_shipment_id",
                table: "shipment_messages");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_messages_users_sender_id",
                table: "shipment_messages");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_packages_shipments_shipment_id",
                table: "shipment_packages");

            migrationBuilder.DropForeignKey(
                name: "FK_shipment_parties_shipments_shipment_id",
                table: "shipment_parties");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_users_assigned_broker",
                table: "shipments");

            migrationBuilder.DropForeignKey(
                name: "FK_shipments_users_created_by",
                table: "shipments");

            migrationBuilder.DropTable(
                name: "broker_profiles");

            migrationBuilder.DropTable(
                name: "shipment_products");

            migrationBuilder.DropTable(
                name: "shipper_profiles");

            migrationBuilder.DropIndex(
                name: "idx_shipments_assigned_broker",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "idx_shipments_reference",
                table: "shipments");

            migrationBuilder.DropIndex(
                name: "IX_shipment_documents_uploaded_by",
                table: "shipment_documents");

            migrationBuilder.DropIndex(
                name: "idx_notifications_shipment",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "ai_approval_status",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "ai_compliance_score",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "assigned_broker_id",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "broker_approval_status",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "currency",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "customs_value",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "service_level",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "token_generated_at",
                table: "shipments");

            migrationBuilder.DropColumn(
                name: "file_name",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "file_size",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "mime_type",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "validation_confidence",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "validation_notes",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "validation_status",
                table: "shipment_documents");

            migrationBuilder.DropColumn(
                name: "estimated_duty",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "estimated_tax",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "evaluated_at",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "missing_documents",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "overall_score",
                table: "shipment_compliance");

            migrationBuilder.DropColumn(
                name: "shipment_id",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "title",
                table: "notifications");

            migrationBuilder.DropColumn(
                name: "action",
                table: "broker_reviews");

            migrationBuilder.DropColumn(
                name: "requested_documents",
                table: "broker_reviews");

            migrationBuilder.RenameIndex(
                name: "idx_shipments_created_by",
                table: "shipments",
                newName: "IX_shipments_created_by");

            migrationBuilder.RenameColumn(
                name: "address_2",
                table: "shipment_parties",
                newName: "address2");

            migrationBuilder.RenameColumn(
                name: "address_1",
                table: "shipment_parties",
                newName: "address1");

            migrationBuilder.RenameIndex(
                name: "idx_msgs_sender",
                table: "shipment_messages",
                newName: "IX_shipment_messages_sender_id");

            migrationBuilder.RenameColumn(
                name: "file_path",
                table: "shipment_documents",
                newName: "file_url");

            migrationBuilder.RenameColumn(
                name: "suggested_hs_code",
                table: "shipment_compliance",
                newName: "eccn");

            migrationBuilder.RenameColumn(
                name: "required_documents",
                table: "shipment_compliance",
                newName: "ai_notes");

            migrationBuilder.RenameIndex(
                name: "idx_brokerreviews_broker",
                table: "broker_reviews",
                newName: "IX_broker_reviews_broker_id");

            migrationBuilder.AlterColumn<string>(
                name: "verification_token",
                table: "users",
                type: "varchar(200)",
                maxLength: 200,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<DateTime>(
                name: "tos_accepted_at",
                table: "users",
                type: "datetime(3)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime(6)",
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "phone",
                keyValue: null,
                column: "phone",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "phone",
                table: "users",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.UpdateData(
                table: "users",
                keyColumn: "company",
                keyValue: null,
                column: "company",
                value: "");

            migrationBuilder.AlterColumn<string>(
                name: "company",
                table: "users",
                type: "varchar(255)",
                maxLength: 255,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255)",
                oldMaxLength: 255,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "invite_code",
                table: "users",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "invite_expires_at",
                table: "users",
                type: "datetime(3)",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "shipment_type",
                table: "shipments",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "International",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "mode",
                table: "shipments",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Ground",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "created_by",
                table: "shipments",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "carrier",
                table: "shipments",
                type: "varchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "UPS")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "weight_unit",
                table: "shipment_packages",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "kg",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "package_type",
                table: "shipment_packages",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Box",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "dimension_unit",
                table: "shipment_packages",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "cm",
                oldClrType: typeof(string),
                oldType: "varchar(10)",
                oldMaxLength: 10)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<long>(
                name: "sender_id",
                table: "shipment_messages",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "document_type",
                table: "shipment_documents",
                type: "varchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "Other",
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

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

            migrationBuilder.AddColumn<bool>(
                name: "verified_by_broker",
                table: "shipment_documents",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "version",
                table: "shipment_documents",
                type: "int",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.AlterColumn<string>(
                name: "risk_level",
                table: "shipment_compliance",
                type: "varchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "Low",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldMaxLength: 50,
                oldNullable: true)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<int>(
                name: "ai_score",
                table: "shipment_compliance",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ai_status",
                table: "shipment_compliance",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "NeedsDocuments")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<bool>(
                name: "restricted_flag",
                table: "shipment_compliance",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "sanctioned_country_flag",
                table: "shipment_compliance",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "user_id",
                table: "notifications",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "type",
                table: "notifications",
                type: "varchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(100)",
                oldMaxLength: 100)
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AlterColumn<string>(
                name: "message",
                table: "notifications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text")
                .Annotation("MySql:CharSet", "utf8mb4")
                .OldAnnotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "created_at",
                table: "broker_reviews",
                type: "datetime(3)",
                nullable: false,
                defaultValueSql: "CURRENT_TIMESTAMP(3)");

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "broker_reviews",
                type: "varchar(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Pending")
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<string>(
                name: "name",
                table: "users",
                type: "longtext",
                nullable: false,
                computedColumnSql: "CONCAT(COALESCE(first_name,''),' ',COALESCE(last_name,''))",
                stored: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ai_findings",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    details = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message = table.Column<string>(type: "text", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rule_code = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    severity = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "warning")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    suggested_action = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ai_findings", x => x.id);
                    table.ForeignKey(
                        name: "fk_aifindings_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "approval_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    action = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    approver_id = table.Column<long>(type: "bigint", nullable: true),
                    approver_role = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    comments = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    entity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    entity_id = table.Column<long>(type: "bigint", nullable: true),
                    metadata = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    new_state = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    previous_state = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_approval_logs", x => x.id);
                    table.ForeignKey(
                        name: "FK_approval_logs_users_approver_id",
                        column: x => x.approver_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "audit_logs",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    action = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    details = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    entity = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    entity_id = table.Column<long>(type: "bigint", nullable: true),
                    performed_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    user_id = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_audit_logs", x => x.id);
                    table.ForeignKey(
                        name: "fk_audit_user",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "broker_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    fulfilled_at = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    message = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    requested_document = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Open")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_broker_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_brokerrequests_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "import_export_rules",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    active = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: true),
                    country_code = table.Column<string>(type: "varchar(2)", maxLength: 2, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    created_by = table.Column<long>(type: "bigint", nullable: true),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    effective_from = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    effective_to = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    hs_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rule_json = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    rule_key = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    source = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_at = table.Column<DateTime>(type: "datetime(3)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    version = table.Column<int>(type: "int", nullable: false, defaultValue: 1)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_export_rules", x => x.id);
                    table.ForeignKey(
                        name: "FK_import_export_rules_users_created_by",
                        column: x => x.created_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "payments",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, defaultValue: "USD")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    paid_at = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    payer = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Shipper")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_method = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    payment_status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "pending")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payments", x => x.id);
                    table.ForeignKey(
                        name: "fk_payments_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipment_items",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    country_of_origin = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    export_reason = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "Sale")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hs_code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    product_name = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    quantity = table.Column<decimal>(type: "decimal(18,3)", nullable: false, defaultValue: 1m),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    total_value = table.Column<decimal>(type: "decimal(18,4)", nullable: true),
                    unit = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, defaultValue: "pcs")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    unit_price = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_items", x => x.id);
                    table.ForeignKey(
                        name: "fk_items_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipment_services",
                columns: table => new
                {
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    bill_to = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Shipper")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    currency = table.Column<string>(type: "varchar(3)", maxLength: 3, nullable: false, defaultValue: "USD")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    declared_value = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    incoterm = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    insurance_required = table.Column<bool>(type: "tinyint(1)", nullable: false, defaultValue: false),
                    service_level = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "Standard")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_services", x => x.shipment_id);
                    table.ForeignKey(
                        name: "fk_services_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "shipment_tracking",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    details = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    event_time = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    shipment_id = table.Column<long>(type: "bigint", nullable: false),
                    status = table.Column<string>(type: "varchar(200)", maxLength: 200, nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_shipment_tracking", x => x.id);
                    table.ForeignKey(
                        name: "fk_tracking_shipment",
                        column: x => x.shipment_id,
                        principalTable: "shipments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "rule_change_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    created_at = table.Column<DateTime>(type: "datetime(3)", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP(3)"),
                    proposed_rule_json = table.Column<string>(type: "json", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    proposer_id = table.Column<long>(type: "bigint", nullable: true),
                    rationale = table.Column<string>(type: "text", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reviewed_at = table.Column<DateTime>(type: "datetime(3)", nullable: true),
                    reviewed_by = table.Column<long>(type: "bigint", nullable: true),
                    rule_id = table.Column<long>(type: "bigint", nullable: true),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValue: "pending")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    updated_at = table.Column<DateTime>(type: "datetime(3)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_rule_change_requests", x => x.id);
                    table.ForeignKey(
                        name: "FK_rule_change_requests_import_export_rules_rule_id",
                        column: x => x.rule_id,
                        principalTable: "import_export_rules",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_rule_change_requests_users_proposer_id",
                        column: x => x.proposer_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_rule_change_requests_users_reviewed_by",
                        column: x => x.reviewed_by,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.SetNull);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "idx_shipments_reference",
                table: "shipments",
                column: "reference_id");

            migrationBuilder.CreateIndex(
                name: "idx_compliance_ai_status",
                table: "shipment_compliance",
                column: "ai_status");

            migrationBuilder.CreateIndex(
                name: "idx_aifindings_shipment",
                table: "ai_findings",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_approval_approver",
                table: "approval_logs",
                column: "approver_id");

            migrationBuilder.CreateIndex(
                name: "idx_approval_entity",
                table: "approval_logs",
                columns: new[] { "entity", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "idx_audit_entity",
                table: "audit_logs",
                columns: new[] { "entity", "entity_id" });

            migrationBuilder.CreateIndex(
                name: "IX_audit_logs_user_id",
                table: "audit_logs",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_brokerrequests_shipment",
                table: "broker_requests",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_rules_active",
                table: "import_export_rules",
                column: "active");

            migrationBuilder.CreateIndex(
                name: "idx_rules_country",
                table: "import_export_rules",
                column: "country_code");

            migrationBuilder.CreateIndex(
                name: "idx_rules_hs",
                table: "import_export_rules",
                column: "hs_code");

            migrationBuilder.CreateIndex(
                name: "idx_rules_key",
                table: "import_export_rules",
                column: "rule_key");

            migrationBuilder.CreateIndex(
                name: "IX_import_export_rules_created_by",
                table: "import_export_rules",
                column: "created_by");

            migrationBuilder.CreateIndex(
                name: "idx_payments_shipment",
                table: "payments",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_payments_status",
                table: "payments",
                column: "payment_status");

            migrationBuilder.CreateIndex(
                name: "idx_rcr_status",
                table: "rule_change_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_rule_change_requests_proposer_id",
                table: "rule_change_requests",
                column: "proposer_id");

            migrationBuilder.CreateIndex(
                name: "IX_rule_change_requests_reviewed_by",
                table: "rule_change_requests",
                column: "reviewed_by");

            migrationBuilder.CreateIndex(
                name: "IX_rule_change_requests_rule_id",
                table: "rule_change_requests",
                column: "rule_id");

            migrationBuilder.CreateIndex(
                name: "idx_items_hscode",
                table: "shipment_items",
                column: "hs_code");

            migrationBuilder.CreateIndex(
                name: "idx_items_shipment",
                table: "shipment_items",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_services_shipment",
                table: "shipment_services",
                column: "shipment_id");

            migrationBuilder.CreateIndex(
                name: "idx_tracking_shipment",
                table: "shipment_tracking",
                column: "shipment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_brokerreviews_broker",
                table: "broker_reviews",
                column: "broker_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_brokerreviews_shipment",
                table: "broker_reviews",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_notifications_user",
                table: "notifications",
                column: "user_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_compliance_shipment",
                table: "shipment_compliance",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_docs_shipment",
                table: "shipment_documents",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_msgs_sender",
                table: "shipment_messages",
                column: "sender_id",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "fk_msgs_shipment",
                table: "shipment_messages",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_packages_shipment",
                table: "shipment_packages",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_parties_shipment",
                table: "shipment_parties",
                column: "shipment_id",
                principalTable: "shipments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_shipments_createdby",
                table: "shipments",
                column: "created_by",
                principalTable: "users",
                principalColumn: "id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
