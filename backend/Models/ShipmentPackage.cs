using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PreClear.Api.Models
{
    // PackageType enum updated to match frontend requirements: Box, Pallet, Crate, Bag, Case, Envelope
    public enum PackageType { Box, Pallet, Crate, Bag, Case, Envelope }
    public enum DimensionUnit { cm, @in }
    public enum WeightUnit { kg, lb }

    [Table("shipment_packages")]
    public class ShipmentPackage
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("shipment_id")]
        [Required]
        public long ShipmentId { get; set; }

        [Column("package_type")]
        [MaxLength(20)]
        public string PackageType { get; set; } = "Box"; // Box, Envelope, Pallet, Other

        // Dimensions
        [Column("length")]
        public decimal? Length { get; set; }

        [Column("width")]
        public decimal? Width { get; set; }

        [Column("height")]
        public decimal? Height { get; set; }

        [Column("dimension_unit")]
        [MaxLength(10)]
        public string DimensionUnit { get; set; } = "cm"; // cm, in

        // Weight
        [Column("weight")]
        public decimal? Weight { get; set; }

        [Column("weight_unit")]
        [MaxLength(10)]
        public string WeightUnit { get; set; } = "kg"; // kg, lb

        [Column("stackable")]
        public bool Stackable { get; set; } = false;

        // Navigation
        [ForeignKey("ShipmentId")]
        [JsonIgnore]
        public Shipment? Shipment { get; set; }

        public ICollection<ShipmentProduct> Products { get; set; } = new List<ShipmentProduct>();
    }
}
