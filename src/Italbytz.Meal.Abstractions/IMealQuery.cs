using System;

namespace Italbytz.Meal.Abstractions
{
    public interface IMealQuery
    {
        int Mensa { get; set; }
        DateTime Date { get; set; }
    }
}