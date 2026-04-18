using System;
namespace Italbytz.Meal.Abstractions
{
    public class Meal : IMeal
    {
        public DateTime Date { get; set; } = DateTime.MinValue;

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public IPrice Price { get; set; } = new Price();

        public Allergens Allergens { get; set; } = Allergens.None;

        public Additives Additives { get; set; } = Additives.None;

        public Category Category { get; set; } = Category.None;
    }
}