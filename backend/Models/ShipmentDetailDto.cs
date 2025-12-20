using System.Collections.Generic;
using PreClear.Api.Interfaces;

namespace PreClear.Api.Models
{
    public class ShipmentDetailDto
    {
        public Shipment Shipment { get; set; } = null!;
        public List<ShipmentParty> Parties { get; set; } = new List<ShipmentParty>();
        public List<ShipmentPackage> Packages { get; set; } = new List<ShipmentPackage>();
        public List<ShipmentProduct> Items { get; set; } = new List<ShipmentProduct>();
        public ShipmentServiceData? Services { get; set; }
    }
}
