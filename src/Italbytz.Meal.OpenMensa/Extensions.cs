using System;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.OpenMensa
{
    public static class Extensions
    {
        public static IMeal ToIMeal(this Italbytz.Meal.OpenMensa.Client.Meal meal)
        {
            var category = Category.Dish;
            switch (meal.Category)
            {
                case "Desserts": category = Category.Dessert; break;
                case "Beilagen": category = Category.Sidedish; break;
                default:
                    break;
            }
            return new Italbytz.Meal.Abstractions.Meal()
            {
                Name = meal.Name,
                Image = "",
                Price = new Italbytz.Meal.Abstractions.Price() { Employees = meal.Prices.Employees, Others = meal.Prices.Others, Pupils = meal.Prices.Pupils, Students = meal.Prices.Students },
                Allergens = Allergens.None,
                Additives = Additives.None,
                Category = category
            };
        }

    }
}
