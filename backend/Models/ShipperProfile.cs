using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreClear.Api.Models
{
    [Table("shipper_profiles")]
    public class ShipperProfile
    {
        [Key]
        [Column("user_id")]
        public long UserId { get; set; }

        // Address
        [Column("address_line_1")]
        [MaxLength(255)]
        public string? AddressLine1 { get; set; }

        [Column("address_line_2")]
        [MaxLength(255)]
        public string? AddressLine2 { get; set; }

        [Column("city")]
        [MaxLength(100)]
        public string? City { get; set; }

        [Column("state")]
        [MaxLength(100)]
        public string? State { get; set; }

        [Column("postal_code")]
        [MaxLength(20)]
        public string? PostalCode { get; set; }

        [Column("country_code")]
        [MaxLength(3)]
        public string CountryCode { get; set; } = "US";

        // Preferences
        [Column("timezone")]
        [MaxLength(50)]
        public string Timezone { get; set; } = "America/New_York";

        [Column("language")]
        [MaxLength(10)]
        public string Language { get; set; } = "en";

        // Company Details
        [Column("company_role")]
        [MaxLength(100)]
        public string? CompanyRole { get; set; }

        // Timestamps
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("UserId")]
        public User? User { get; set; }
    }
}
