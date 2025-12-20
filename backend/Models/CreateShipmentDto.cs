namespace PreClear.Api.Models
{
    public class CreateShipmentDto
    {
        public string? ShipmentName { get; set; }
        public ShipmentMode Mode { get; set; } = ShipmentMode.Air;
        public ShipmentType ShipmentType { get; set; } = ShipmentType.International;
    }
}
