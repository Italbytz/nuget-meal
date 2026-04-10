using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Italbytz.Meal.OpenMensa.Client;
using Italbytz.Common.Abstractions;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.OpenMensa
{
    public class OpenMensaMealDataSource : IDataSource<int, IMeal>
    {
        int Mensa { get; set; }
        DateTime Date { get; set; }

        readonly OpenMensaAPI api = new OpenMensaAPI();

        public OpenMensaMealDataSource(int mensa, DateTime date)
        {
            Mensa = mensa;
            Date = date;
        }

        public Task<IMeal?> Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<List<IMeal>?> RetrieveAll()
        {
            var meals = await api.GetMeals(Mensa, Date);
            return meals.Select(meal => meal.ToIMeal()).ToList();
        }

    }

}