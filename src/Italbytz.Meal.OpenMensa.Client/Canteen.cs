using System;
using System.Text.Json.Serialization;

namespace Italbytz.Meal.OpenMensa.Client
{
    public partial class Canteen
    {
        [JsonPropertyName("id")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; } = string.Empty;

        [JsonPropertyName("city")]
        public string City { get; set; } = string.Empty;

        [JsonPropertyName("address")]
        public string Address { get; set; } = string.Empty;

        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; set; } = Array.Empty<double>();
    }
}

