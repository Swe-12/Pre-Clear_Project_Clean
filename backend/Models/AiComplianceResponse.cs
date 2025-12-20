using System.Collections.Generic;

namespace PreClear.Api.Models
{
    public class AiComplianceResponse
    {
        public int AiComplianceScore { get; set; }
        public string AiComplianceStatus { get; set; } = "flagged"; // cleared | flagged | denied
        public string? AiValidationNotes { get; set; }
        public List<string> MissingDocuments { get; set; } = new List<string>();
        public string? SuggestedHsCode { get; set; }
        public decimal EstimatedDutyTax { get; set; }
        public string RiskLevel { get; set; } = "low"; // low | medium | high | critical
    }
}
