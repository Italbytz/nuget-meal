using System;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.OpenMensa
{
    public class Meal : IMeal
    {
        public Meal()
        {
        }

        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Allergens Allergens { get; set; } = Allergens.None;
        public Additives Additives { get; set; } = Additives.None;
        public Category Category { get; set; } = Category.None;
        public IPrice Price { get; set; } = new Price();
    }
}

