using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace PreClear.Api.Models
{
    [Table("broker_reviews")]
    public class BrokerReview
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }
        
        [Column("shipment_id")]
        public long ShipmentId { get; set; }
        
        [Column("broker_id")]
        public long? BrokerId { get; set; }
        
        [Column("action")]
        [MaxLength(50)]
        public string? Action { get; set; }
        
        [Column("comments")]
        public string? Comments { get; set; }
        
        [Column("requested_documents")]
        public string? RequestedDocumentsJson { get; set; }
        
        [Column("reviewed_at")]
        public DateTime? ReviewedAt { get; set; }
        
        // Navigation properties
        public Shipment? Shipment { get; set; }
        public User? Broker { get; set; }
        
        // Helper property to work with JSON
        [NotMapped]
        public List<string>? RequestedDocuments
        {
            get => string.IsNullOrEmpty(RequestedDocumentsJson) 
                ? null 
                : JsonSerializer.Deserialize<List<string>>(RequestedDocumentsJson);
            set => RequestedDocumentsJson = value == null ? null : JsonSerializer.Serialize(value);
        }
    }
}
