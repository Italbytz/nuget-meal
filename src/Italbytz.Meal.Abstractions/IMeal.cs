using System;
namespace Italbytz.Meal.Abstractions
{
    public interface IMeal
    {
        DateTime Date { get; set; }
        string Name { get; set; }
        string Image { get; set; }
        IPrice Price { get; set; }
        Allergens Allergens { get; set; }
        Additives Additives { get; set; }
        Category Category { get; set; }
        Badge[] Badges { get; set; }
    }
}
