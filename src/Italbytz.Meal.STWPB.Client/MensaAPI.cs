using System;
using System.Linq;
using System.Collections.Generic;
using System.Globalization;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Italbytz.Meal.STWPB.Client
{
    public class MensaAPI
    {
        private const string DateFormat = "yyyy-MM-dd";
        private const string HammVenue = "mensa-hamm";
        private readonly string _id;
        private readonly HttpClient _httpClient;

        public MensaAPI(string id, string acceptLanguage, HttpClient? httpClient = null)
        {
            _id = id;
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress ??= new Uri("https://stwpb.de");

            if (!_httpClient.DefaultRequestHeaders.Accept.Any(header => header.MediaType == "application/json"))
            {
                _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            }

            if (!_httpClient.DefaultRequestHeaders.AcceptLanguage.Any(header => string.Equals(header.Value, acceptLanguage, StringComparison.OrdinalIgnoreCase)))
            {
                _httpClient.DefaultRequestHeaders.AcceptLanguage.Add(new StringWithQualityHeaderValue(acceptLanguage));
            }
        }

        public async Task<List<Meal>> GetMeals()
        {
            return await _httpClient.GetFromJsonAsync<List<Meal>>($"fileadmin/shareddata/access2.php?id={_id}", Converter.Options)
                ?? [];
        }

        public async Task<List<Meal>> GetTodaysHammMeals(DateTime? date = null)
        {
            var requestedDate = (date ?? DateTime.Now).Date;
            var response = await _httpClient.GetFromJsonAsync<MealPlanResponse>($"/wp-json/stwk-pb/v1/meals?venue={HammVenue}&start_date={requestedDate.ToString(DateFormat, CultureInfo.InvariantCulture)}&end_date={requestedDate.AddDays(6).ToString(DateFormat, CultureInfo.InvariantCulture)}")
                ?? new MealPlanResponse();

            var meals = response.Meals
                .Where(meal => !string.IsNullOrWhiteSpace(meal.Title))
                .Where(meal => !meal.Category.Contains("restanten", StringComparison.OrdinalIgnoreCase))
                .Select(MapMeal)
                .OrderBy(meal => meal.Date)
                .ToList();

            var targetDate = meals
                .Select(meal => meal.Date.Date)
                .Where(mealDate => mealDate >= requestedDate)
                .Distinct()
                .OrderBy(mealDate => mealDate)
                .FirstOrDefault();

            if (targetDate == default)
            {
                return [];
            }

            return meals.Where(meal => meal.Date.Date == targetDate).ToList();
        }

        private static Meal MapMeal(RestMeal meal)
        {
            var category = ParseCategory(meal.Category);

            return new Meal
            {
                Date = DateTimeOffset.ParseExact(meal.Date, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal),
                NameDe = meal.Title,
                Category = category,
                CategoryDe = category switch
                {
                    Category.Dessert => CategoryDe.Dessert,
                    Category.Sidedish => CategoryDe.Beilagen,
                    Category.Soups => CategoryDe.Suppen,
                    Category.None => CategoryDe.Keine,
                    _ => CategoryDe.Essen,
                },
                CategoryEn = category switch
                {
                    Category.Dessert => CategoryEn.Dessert,
                    Category.Sidedish => CategoryEn.SideDish,
                    Category.Soups => CategoryEn.Soups,
                    Category.None => CategoryEn.None,
                    _ => CategoryEn.Dish,
                },
                PriceStudents = ParsePrice(meal.PriceStudents),
                PriceWorkers = ParsePrice(meal.PriceStaff),
                PriceGuests = ParsePrice(meal.PriceGuests),
                Allergens = ParseAllergens(meal.AllergensRaw),
                Restaurant = Restaurant.MensaHamm,
                Pricetype = Pricetype.Fixed,
                Image = meal.ImageJpeg ?? meal.ImageWebp,
                Thumbnail = meal.ImageJpegThumb ?? meal.ImageWebpThumb,
            };
        }

        private static Category ParseCategory(string category)
        {
            var normalizedCategory = category.ToLowerInvariant();

            if (normalizedCategory.Contains("eintopf") || normalizedCategory.Contains("suppe"))
            {
                return Category.Soups;
            }

            if (normalizedCategory.Contains("beilage") || normalizedCategory.Contains("sättigungbeil") || normalizedCategory.Contains("gemüsebeil") || normalizedCategory.Contains("beilagensalat"))
            {
                return Category.Sidedish;
            }

            if (normalizedCategory.Contains("dessert"))
            {
                return Category.Dessert;
            }

            if (normalizedCategory.Contains("fleisch") || normalizedCategory.Contains("fisch") || normalizedCategory.Contains("vegan") || normalizedCategory.Contains("vegetarisch") || normalizedCategory.Contains("aktions"))
            {
                return Category.Dish;
            }

            return Category.DishDefault;
        }

        private static double ParsePrice(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            return double.TryParse(value.Replace(',', '.'), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out var price)
                ? price
                : 0;
        }

        private static AllergenEnum[] ParseAllergens(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return [];
            }

            return value
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(ParseAllergen)
                .Where(allergen => allergen.HasValue)
                .Select(allergen => allergen!.Value)
                .Distinct()
                .ToArray();
        }

        private static AllergenEnum? ParseAllergen(string value)
        {
            var normalizedValue = value.Trim().ToUpperInvariant();
            return Enum.TryParse<AllergenEnum>(normalizedValue.StartsWith('A') ? normalizedValue : $"Z{normalizedValue}", out var allergen)
                ? allergen
                : null;
        }

        private sealed class MealPlanResponse
        {
            [JsonPropertyName("meals")]
            public List<RestMeal> Meals { get; set; } = [];
        }

        private sealed class RestMeal
        {
            [JsonPropertyName("date")]
            public string Date { get; set; } = string.Empty;

            [JsonPropertyName("title")]
            public string Title { get; set; } = string.Empty;

            [JsonPropertyName("category")]
            public string Category { get; set; } = string.Empty;

            [JsonPropertyName("allergens_raw")]
            public string? AllergensRaw { get; set; }

            [JsonPropertyName("price_students")]
            public string? PriceStudents { get; set; }

            [JsonPropertyName("price_staff")]
            public string? PriceStaff { get; set; }

            [JsonPropertyName("price_guests")]
            public string? PriceGuests { get; set; }

            [JsonPropertyName("image_jpeg")]
            public string? ImageJpeg { get; set; }

            [JsonPropertyName("image_jpeg_thumb")]
            public string? ImageJpegThumb { get; set; }

            [JsonPropertyName("image_webp")]
            public string? ImageWebp { get; set; }

            [JsonPropertyName("image_webp_thumb")]
            public string? ImageWebpThumb { get; set; }
        }
    }
}
