using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Italbytz.Meal.OpenMensa.Client
{
    public class OpenMensaAPI
    {
        private const string MediaTypeJSON = "application/json";
        private static readonly string format = "yyyy-MM-dd";

        private static readonly HttpClient HttpClient;

        static OpenMensaAPI()
        {
            HttpClient = new HttpClient()
            {
                BaseAddress = new Uri("https://openmensa.org")
            };
            var media = new MediaTypeWithQualityHeaderValue(MediaTypeJSON);
            HttpClient.DefaultRequestHeaders.Accept.Add(media);
        }

        public async Task<List<Canteen>> GetCanteens()
        {
            return await HttpClient.GetFromJsonAsync<List<Canteen>>("/api/v2/canteens") ?? new List<Canteen>();
        }

        public async Task<List<Day>> GetCanteenDays(long id)
        {
            return await HttpClient.GetFromJsonAsync<List<Day>>($"/api/v2/canteens/{id}/days") ?? new List<Day>();
        }

        public async Task<List<Meal>> GetMeals(long id, DateTime date)
        {
            var days = await GetCanteenDays(id);
            var requestedDate = date.ToString(format);
            foreach (var day in days)
            {
                if (day.Date.ToString(format).Equals(requestedDate))
                {
                    if (day.Closed)
                    {
                        throw new MensaClosedException();
                    }
                    return await HttpClient.GetFromJsonAsync<List<Meal>>($"/api/v2/canteens/{id}/days/{requestedDate}/meals") ?? new List<Meal>();
                }
            }
            throw new NoMealsForDateException();
        }

        public async Task<List<Meal>> GetTodaysMeals(long id)
        {
            return await GetMeals(id, DateTime.Now);
        }
    }

    [Serializable]
    public class NoMealsForDateException : Exception
    {
        public NoMealsForDateException()
        {
        }

        public NoMealsForDateException(string message) : base(message)
        {
        }

        public NoMealsForDateException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NoMealsForDateException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class MensaClosedException : Exception
    {
        public MensaClosedException()
        {
        }

        public MensaClosedException(string message) : base(message)
        {
        }

        public MensaClosedException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected MensaClosedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}

