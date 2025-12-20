using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PreClear.Api.Models
{
    [Table("shipment_messages")]
    public class ShipmentMessage
    {
        [Key]
        [Column("id")]
        public long Id { get; set; }

        [Column("shipment_id")]
        [Required]
        public long ShipmentId { get; set; }

        [Column("sender_id")]
        [Required]
        public long SenderId { get; set; }

        [Column("message")]
        [Required]
        public string Message { get; set; } = null!;

        [Column("created_at")]
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // Navigation
        [ForeignKey("ShipmentId")]
        [JsonIgnore]
        public Shipment? Shipment { get; set; }

        [ForeignKey("SenderId")]
        public User? Sender { get; set; }
    }
}
