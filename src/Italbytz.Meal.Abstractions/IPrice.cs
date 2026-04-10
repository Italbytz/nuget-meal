using System;
namespace Italbytz.Meal.Abstractions
{
    public interface IPrice
    {
        double? Students { get; set; }
        double? Employees { get; set; }        
        double? Pupils { get; set; }
        double? Others { get; set; }
    }
}
