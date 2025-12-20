using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PreClear.Api.Models
{
    public enum PartyType { shipper, consignee, third_party }

    [Table("shipment_parties")]
    public class ShipmentParty
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("shipment_id")]
        [Required]
        public long ShipmentId { get; set; }

        [Column("party_type")]
        [MaxLength(20)]
        [Required]
        public string PartyType { get; set; } = null!; // shipper, consignee, third_party

        // Contact
        [Column("company_name")]
        [MaxLength(255)]
        [Required]
        public string CompanyName { get; set; } = null!;

        [Column("contact_name")]
        [MaxLength(150)]
        public string? ContactName { get; set; }

        [Column("phone")]
        [MaxLength(50)]
        public string? Phone { get; set; }

        [Column("email")]
        [MaxLength(255)]
        public string? Email { get; set; }

        // Address
        [Column("address_1")]
        [MaxLength(255)]
        public string? Address1 { get; set; }

        [Column("address_2")]
        [MaxLength(255)]
        public string? Address2 { get; set; }

        [Column("city")]
        [MaxLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [MaxLength(100)]
        public string? State { get; set; }

        [Column("postal_code")]
        [MaxLength(50)]
        public string? PostalCode { get; set; }

        [Column("country")]
        [MaxLength(100)]
        public string? Country { get; set; }

        [Column("tax_id")]
        [MaxLength(100)]
        public string? TaxId { get; set; }

        // Navigation
        [ForeignKey("ShipmentId")]
        [JsonIgnore]
        public Shipment? Shipment { get; set; }
    }
}
