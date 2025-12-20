using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json;

namespace PreClear.Api.Models
{
    [Table("users")]
    public class User
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        // Authentication
        [Column("email")]
        [MaxLength(255)]
        [Required]
        public string Email { get; set; } = null!;

        [Column("password_hash")]
        [MaxLength(255)]
        [Required]
        public string PasswordHash { get; set; } = null!;

        // Basic Info
        [Column("first_name")]
        [MaxLength(150)]
        [Required]
        public string FirstName { get; set; } = null!;

        [Column("last_name")]
        [MaxLength(150)]
        [Required]
        public string LastName { get; set; } = null!;

        [Column("role")]
        [MaxLength(20)]
        public string Role { get; set; } = "shipper"; // shipper, broker, admin

        // Contact (basic)
        [Column("phone")]
        [MaxLength(50)]
        public string? Phone { get; set; }

        [Column("company")]
        [MaxLength(255)]
        public string? Company { get; set; }

        // Account Status
        [Column("tos_accepted")]
        public bool TosAccepted { get; set; }

        [Column("tos_accepted_at")]
        public DateTime? TosAcceptedAt { get; set; }

        [Column("email_verified")]
        public bool EmailVerified { get; set; }

        [Column("verification_token")]
        [MaxLength(255)]
        public string? VerificationToken { get; set; }

        [Column("is_active")]
        public bool IsActive { get; set; } = true;

        // Metadata
        [Column("metadata", TypeName = "json")]
        public string? MetadataJson { get; set; }

        // Timestamps
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        public ShipperProfile? ShipperProfile { get; set; }
        public BrokerProfile? BrokerProfile { get; set; }

        // Computed property
        [NotMapped]
        public string Name => $"{FirstName} {LastName}".Trim();
    }
}
