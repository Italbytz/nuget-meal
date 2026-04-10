using System;
using System.Linq;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Italbytz.Meal.STWPB.Client
{
    public class MensaAPI
    {
        private const string DateFormat = "yyyy-MM-dd";
        private readonly string _id;
        private readonly HttpClient _httpClient;

        public MensaAPI(string id, string acceptLanguage, HttpClient? httpClient = null)
        {
            _id = id;
            _httpClient = httpClient ?? new HttpClient();
            _httpClient.BaseAddress ??= new Uri("https://www.studentenwerk-pb.de");

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
            var dateString = (date ?? DateTime.Now).ToString(DateFormat);
            return await _httpClient.GetFromJsonAsync<List<Meal>>($"fileadmin/shareddata/access2.php?id={_id}&restaurant=mensa-hamm&date={dateString}", Converter.Options)
                ?? [];
        }
    }
}
