using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PreClear.Api.Models
{
    [Table("shipment_products")]
    public class ShipmentProduct
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("package_id")]
        [Required]
        public long PackageId { get; set; }

        [Column("shipment_id")]
        [Required]
        public long ShipmentId { get; set; }

        // Product Info
        [Column("name")]
        [MaxLength(255)]
        [Required]
        public string Name { get; set; } = null!;

        [Column("description")]
        public string? Description { get; set; }

        [Column("category")]
        [MaxLength(100)]
        public string? Category { get; set; }

        // HS Classification
        [Column("hs_code")]
        [MaxLength(20)]
        public string? HsCode { get; set; }

        // Quantity & Value
        [Column("quantity")]
        public decimal Quantity { get; set; } = 1;

        [Column("unit")]
        [MaxLength(20)]
        public string Unit { get; set; } = "pcs";

        [Column("unit_price")]
        public decimal? UnitPrice { get; set; }

        [Column("total_value")]
        public decimal? TotalValue { get; set; }

        // Origin
        [Column("origin_country")]
        [MaxLength(3)]
        public string? OriginCountry { get; set; }

        [Column("export_reason")]
        [MaxLength(50)]
        public string ExportReason { get; set; } = "Sale";

        // Navigation
        [ForeignKey("PackageId")]
        public ShipmentPackage? Package { get; set; }

        [ForeignKey("ShipmentId")]
        [JsonIgnore]
        public Shipment? Shipment { get; set; }
    }
}
