using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Italbytz.Common.Abstractions;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.Testing
{
    public class MockMealDataSource : IDataSource<int, IMeal>
    {
        public MockMealDataSource()
        {
        }

        public Task<IMeal?> Retrieve(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<IMeal>?> RetrieveAll()
        {
            var list = new List<IMeal>();
            list.AddRange(Mocks.Dishes);
            list.AddRange(Mocks.Desserts);
            return Task.FromResult<List<IMeal>?>(list);
        }

    }

}

