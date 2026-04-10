using System;
using System.Text.Json.Serialization;

namespace Italbytz.Meal.OpenMensa.Client
{
    public partial class Meal
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("category")]
        public string Category { get; set; } = string.Empty;

        [JsonPropertyName("prices")]
        public Prices Prices { get; set; } = new Prices();

        [JsonPropertyName("notes")]
        public string[] Notes { get; set; } = Array.Empty<string>();
    }

    public partial class Prices
    {
        [JsonPropertyName("students")]
        public double? Students { get; set; }

        [JsonPropertyName("employees")]
        public double? Employees { get; set; }

        [JsonPropertyName("pupils")]
        public double? Pupils { get; set; }

        [JsonPropertyName("others")]
        public double? Others { get; set; }
    }
}