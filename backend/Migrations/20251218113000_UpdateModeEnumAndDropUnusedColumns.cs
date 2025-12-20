using Microsoft.EntityFrameworkCore.Migrations;

namespace PreClear.Api.Migrations
{
    public partial class UpdateModeEnumAndDropUnusedColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // 1) Migrate existing data: Ground -> Road
            migrationBuilder.Sql("UPDATE `shipments` SET `mode`='Road' WHERE `mode`='Ground';");

            // 2) Modify mode ENUM to Air, Sea, Road, Rail, Courier, Multimodal
            migrationBuilder.Sql(
                "ALTER TABLE `shipments` MODIFY COLUMN `mode` ENUM('Air','Sea','Road','Rail','Courier','Multimodal') NOT NULL DEFAULT 'Road';"
            );

            // 3) Drop unused columns safely if they exist
            // MySQL doesn't support IF EXISTS for DROP COLUMN in older versions; use INFORMATION_SCHEMA guard
            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` DROP COLUMN `incoterm`;') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'incoterm';
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` DROP COLUMN `insurance_required`;') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'insurance_required';
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` DROP COLUMN `bill_to`;') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'bill_to';
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Re-add columns (best-effort)
            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` ADD COLUMN `incoterm` VARCHAR(20) NULL;') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'incoterm' IS NULL;
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` ADD COLUMN `insurance_required` TINYINT(1) NULL DEFAULT 0;') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'insurance_required' IS NULL;
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");

            migrationBuilder.Sql(@"
                SET @sql := NULL;
                SELECT CONCAT('ALTER TABLE `shipments` ADD COLUMN `bill_to` VARCHAR(20) NULL DEFAULT \'Shipper\';') INTO @sql
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE() AND TABLE_NAME = 'shipments' AND COLUMN_NAME = 'bill_to' IS NULL;
                PREPARE stmt FROM @sql; EXECUTE stmt; DEALLOCATE PREPARE stmt;
            ");

            // Revert ENUM to original
            migrationBuilder.Sql(
                "ALTER TABLE `shipments` MODIFY COLUMN `mode` ENUM('Air','Sea','Ground') NOT NULL DEFAULT 'Ground';"
            );

            // Revert data Road -> Ground for compatibility
            migrationBuilder.Sql("UPDATE `shipments` SET `mode`='Ground' WHERE `mode`='Road';");
        }
    }
}
