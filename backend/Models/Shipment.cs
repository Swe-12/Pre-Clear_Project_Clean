using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PreClear.Api.Models
{
    // Mode enum updated per requirements: Air, Sea, Road, Rail, Courier, Multimodal
    public enum ShipmentMode { Air, Sea, Road, Rail, Courier, Multimodal }
    public enum ShipmentType { Domestic, International }
    public enum AiApprovalStatus { [Display(Name = "not-started")] NotStarted, [Display(Name = "pending")] Pending, [Display(Name = "approved")] Approved, [Display(Name = "rejected")] Rejected }
    public enum BrokerApprovalStatus { [Display(Name = "not-started")] NotStarted, [Display(Name = "pending")] Pending, [Display(Name = "approved")] Approved, [Display(Name = "rejected")] Rejected, [Display(Name = "documents-requested")] DocumentsRequested }

    [Table("shipments")]
    public class Shipment
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("reference_id")]
        [MaxLength(120)]
        [Required]
        public string ReferenceId { get; set; } = null!;

        // Ownership (MANDATORY - NO NULLS)
        [Column("created_by")]
        [Required]
        public long CreatedBy { get; set; }

        [Column("assigned_broker_id")]
        public long? AssignedBrokerId { get; set; }

        // Basic Info
        [Column("shipment_name")]
        [MaxLength(255)]
        public string? ShipmentName { get; set; }

        [Column("mode")]
        [MaxLength(50)]
        public string Mode { get; set; } = "Air";

        [Column("shipment_type")]
        [MaxLength(50)]
        public string ShipmentType { get; set; } = "International";

        // Service Details
        [Column("service_level")]
        [MaxLength(50)]
        public string? ServiceLevel { get; set; }


        // Financial
        [Column("currency")]
        [MaxLength(3)]
        public string? Currency { get; set; } = "USD";

        [Column("customs_value")]
        public decimal? CustomsValue { get; set; }

        // removed: insurance_required

        // Status Tracking
        [Column("status")]
        [MaxLength(50)]
        public string Status { get; set; } = "draft";

        [Column("ai_approval_status")]
        [MaxLength(20)]
        public string AiApprovalStatus { get; set; } = "not-started";

        [Column("ai_compliance_score")]
        public decimal? AiComplianceScore { get; set; }

        [Column("broker_approval_status")]
        [MaxLength(30)]
        public string BrokerApprovalStatus { get; set; } = "not-started";

        // Token (generated after full approval)
        [Column("preclear_token")]
        [MaxLength(50)]
        public string? PreclearToken { get; set; }

        [Column("token_generated_at")]
        public DateTime? TokenGeneratedAt { get; set; }

        // Timestamps
        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Column("updated_at")]
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Navigation Properties
        [ForeignKey("CreatedBy")]
        public User? Creator { get; set; }

        [ForeignKey("AssignedBrokerId")]
        public User? AssignedBroker { get; set; }

        public ICollection<ShipmentParty> Parties { get; set; } = new List<ShipmentParty>();
        public ICollection<ShipmentPackage> Packages { get; set; } = new List<ShipmentPackage>();
        public ICollection<ShipmentDocument> Documents { get; set; } = new List<ShipmentDocument>();
        public ShipmentCompliance? Compliance { get; set; }
    }
}
