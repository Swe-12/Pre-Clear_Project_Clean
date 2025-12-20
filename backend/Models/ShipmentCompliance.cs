using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PreClear.Api.Models
{
    [Table("shipment_compliance")]
    public class ShipmentCompliance
    {
        [Key]
        [Column("shipment_id")]
        public long ShipmentId { get; set; }

        // AI Scores
        [Column("overall_score")]
        public decimal? OverallScore { get; set; }

        [Column("risk_level")]
        [MaxLength(20)]
        public string? RiskLevel { get; set; } // low, medium, high

        // Flags
        [Column("dangerous_goods")]
        public bool DangerousGoods { get; set; } = false;

        [Column("lithium_battery")]
        public bool LithiumBattery { get; set; } = false;

        [Column("food_pharma_flag")]
        public bool FoodPharmaFlag { get; set; } = false;

        [Column("export_license_required")]
        public bool ExportLicenseRequired { get; set; } = false;

        // AI Suggestions
        [Column("suggested_hs_code")]
        [MaxLength(20)]
        public string? SuggestedHsCode { get; set; }

        [Column("estimated_duty")]
        public decimal? EstimatedDuty { get; set; }

        [Column("estimated_tax")]
        public decimal? EstimatedTax { get; set; }

        // Required Documents (JSON arrays)
        [Column("required_documents", TypeName = "json")]
        public string? RequiredDocumentsJson { get; set; }

        [Column("missing_documents", TypeName = "json")]
        public string? MissingDocumentsJson { get; set; }

        // Timestamps
        [Column("evaluated_at")]
        public DateTime EvaluatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ShipmentId")]
        [JsonIgnore]
        public Shipment? Shipment { get; set; }

        // Helper properties for JSON arrays (not mapped to DB)
        [NotMapped]
        public List<string> RequiredDocuments
        {
            get => string.IsNullOrEmpty(RequiredDocumentsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(RequiredDocumentsJson) ?? new List<string>();
            set => RequiredDocumentsJson = JsonSerializer.Serialize(value);
        }

        [NotMapped]
        public List<string> MissingDocuments
        {
            get => string.IsNullOrEmpty(MissingDocumentsJson) 
                ? new List<string>() 
                : JsonSerializer.Deserialize<List<string>>(MissingDocumentsJson) ?? new List<string>();
            set => MissingDocumentsJson = JsonSerializer.Serialize(value);
        }
    }
}
