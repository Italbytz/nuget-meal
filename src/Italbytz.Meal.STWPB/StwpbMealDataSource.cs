using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Italbytz.Common.Abstractions;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.STWPB
{
    public class StwpbMealDataSource : IDataSource<int, IMeal>
    {
        private readonly Func<Task<List<Italbytz.Meal.STWPB.Client.Meal>>> _fetchMeals;

        public string? Id { get; }

        public string Language { get; }

        public StwpbMealDataSource(string language, Func<Task<List<Italbytz.Meal.STWPB.Client.Meal>>>? fetchMeals = null)
            : this(null, language, fetchMeals)
        {
        }

        public StwpbMealDataSource(string? id, string language, Func<Task<List<Italbytz.Meal.STWPB.Client.Meal>>>? fetchMeals = null)
        {
            Id = id;
            Language = language;
            _fetchMeals = fetchMeals ?? (() => new Italbytz.Meal.STWPB.Client.MensaAPI(language).GetTodaysHammMeals());
        }

        public Task<IMeal?> Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IMeal>?> RetrieveAll()
        {
            var meals = await _fetchMeals();
            return meals.Select(meal => meal.ToIMeal()).ToList();
        }
    }
}
