using System;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.Testing
{
    public class Meal : IMeal
    {
        public Meal()
        {
        }

        public string Name { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;
        public Allergens Allergens { get; set; }
        public Additives Additives { get; set; }
        public Category Category { get; set; }
        public IPrice Price { get; set; } = new Price();
    }
}

