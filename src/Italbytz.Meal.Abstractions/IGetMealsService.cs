using System;
using System.Collections.Generic;
using Italbytz.Common.Abstractions;

namespace Italbytz.Meal.Abstractions
{
    public interface IGetMealsService : IAsyncService<IMealQuery, List<IMealCollection>>
    {
    }
}

