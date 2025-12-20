using System.Collections.Generic;

namespace PreClear.Api.Models
{
    /// <summary>
    /// Clean DTO for HS code suggestion - ensures no reference metadata in JSON
    /// </summary>
    public class HsSuggestionDto
    {
        public string hscode { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public double score { get; set; } = 0d;
    }

    /// <summary>
    /// Response wrapper for HS suggestions - frontend-friendly format
    /// </summary>
    public class HsSuggestionsResponse
    {
        public List<HsSuggestionDto> suggestions { get; set; } = new();
    }

    /// <summary>
    /// Clean DTO for document prediction response
    /// </summary>
    public class DocumentPredictionDto
    {
        public string status { get; set; } = string.Empty;
        public string? message { get; set; }
        public List<string> requiredDocuments { get; set; } = new();
        public List<string> recommendations { get; set; } = new();
    }

    /// <summary>
    /// Response wrapper for document predictions
    /// </summary>
    public class DocumentPredictionResponse
    {
        public DocumentPredictionDto prediction { get; set; } = new();
        public float? confidence { get; set; }
    }
}
