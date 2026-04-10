using System;
using System.Text.Json.Serialization;

namespace Italbytz.Meal.OpenMensa.Client
{
    public partial class Day
    {
        [JsonPropertyName("date")]
        public DateTimeOffset Date { get; set; }

        [JsonPropertyName("closed")]
        public bool Closed { get; set; }
    }
}