using System.Collections.Generic;

namespace Italbytz.Meal.Abstractions
{
    public interface IMealCollection
    {
        Category Category { get; set; }
        List<IMeal> Meals { get; set; }
    }
}