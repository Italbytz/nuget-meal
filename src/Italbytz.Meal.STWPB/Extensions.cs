using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.STWPB
{
    public static class Extensions
    {
        public static IMeal ToIMeal(this Italbytz.Meal.STWPB.Client.Meal self)
        {
            var name = self.NameDe ?? self.NameEn ?? string.Empty;
            var category = self.Category switch
            {
                Italbytz.Meal.STWPB.Client.Category.None => Category.None,
                Italbytz.Meal.STWPB.Client.Category.Dessert => Category.Dessert,
                Italbytz.Meal.STWPB.Client.Category.DessertCounter => Category.Dessert,
                Italbytz.Meal.STWPB.Client.Category.Dish => Category.Dish,
                Italbytz.Meal.STWPB.Client.Category.DishDefault => Category.Dish,
                Italbytz.Meal.STWPB.Client.Category.DishGrill => Category.Dish,
                Italbytz.Meal.STWPB.Client.Category.Empty => Category.None,
                Italbytz.Meal.STWPB.Client.Category.Sidedish => Category.Sidedish,
                Italbytz.Meal.STWPB.Client.Category.Soups => Category.Soup,
                _ => Category.None,
            };

            var allergens = Allergens.None;
            var additives = Additives.None;

            foreach (var allergen in self.Allergens)
            {
                switch (allergen)
                {
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z1: additives |= Additives.FoodColoring; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z2: additives |= Additives.Preservatives; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z3: additives |= Additives.Antioxidants; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z4: additives |= Additives.FlavorEnhancer; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z5: additives |= Additives.Phosphate; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z6: additives |= Additives.Sulphureted; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z7: additives |= Additives.Waxed; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z8: additives |= Additives.Blackend; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z9: additives |= Additives.Sweetener; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z10: additives |= Additives.Phenylalanine; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z11: additives |= Additives.Taurine; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z12: additives |= Additives.NitritePicklingSalt; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z13: additives |= Additives.Caffeinated; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z14: additives |= Additives.Quinine; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.Z15: additives |= Additives.MilkProtein; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A1: allergens |= Allergens.Gluten; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A2: allergens |= Allergens.Shellfish; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A3: allergens |= Allergens.Eggs; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A4: allergens |= Allergens.Fish; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A5: allergens |= Allergens.Peanuts; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A6: allergens |= Allergens.Soy; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A7: allergens |= Allergens.Milk; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A8: allergens |= Allergens.Nuts; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A9: allergens |= Allergens.Celery; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A10: allergens |= Allergens.Mustard; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A11: allergens |= Allergens.Sesame; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A12: allergens |= Allergens.Sulfur; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A13: allergens |= Allergens.Lupine; break;
                    case Italbytz.Meal.STWPB.Client.AllergenEnum.A14: allergens |= Allergens.Mollusk; break;
                }
            }

            var badges = self.Badges.Select(b => b switch
            {
                Italbytz.Meal.STWPB.Client.Badge.Nonfat => Italbytz.Meal.Abstractions.Badge.Nonfat,
                Italbytz.Meal.STWPB.Client.Badge.Vegan => Italbytz.Meal.Abstractions.Badge.Vegan,
                Italbytz.Meal.STWPB.Client.Badge.Vegetarian => Italbytz.Meal.Abstractions.Badge.Vegetarian,
                Italbytz.Meal.STWPB.Client.Badge.LowCalorie => Italbytz.Meal.Abstractions.Badge.LowCalorie,
                Italbytz.Meal.STWPB.Client.Badge.LactoseFree => Italbytz.Meal.Abstractions.Badge.LactoseFree,
                Italbytz.Meal.STWPB.Client.Badge.GlutenFree => Italbytz.Meal.Abstractions.Badge.GlutenFree,
                _ => (Italbytz.Meal.Abstractions.Badge?)null
            }).Where(b => b.HasValue).Select(b => b!.Value).ToArray();

            return new Italbytz.Meal.Abstractions.Meal
            {
                Date = self.Date.DateTime,
                Name = name,
                Image = self.Image ?? string.Empty,
                Category = category,
                Additives = additives,
                Allergens = allergens,
                Badges = badges,
                Price = new Italbytz.Meal.Abstractions.Price
                {
                    Others = self.PriceGuests,
                    Employees = self.PriceWorkers,
                    Students = self.PriceStudents,
                    Pupils = null
                }
            };
        }
    }
}
