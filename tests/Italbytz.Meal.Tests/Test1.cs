using Italbytz.Meal.Abstractions;
using Italbytz.Meal.OpenMensa;
using Italbytz.Meal.Testing;
using ClientMeal = Italbytz.Meal.OpenMensa.Client.Meal;
using ClientPrices = Italbytz.Meal.OpenMensa.Client.Prices;

namespace Italbytz.Meal.Tests;

[TestClass]
public sealed class MealIntegrationTests
{
    [TestMethod]
    public async Task Mock_data_source_returns_seed_meals()
    {
        var dataSource = new MockMealDataSource();

        var meals = await dataSource.RetrieveAll();

        Assert.IsNotNull(meals);
        Assert.AreEqual(3, meals.Count);
    }

    [TestMethod]
    public async Task Mock_get_meals_service_groups_meals_by_category()
    {
        var service = new MockGetMealsService();

        var collections = await service.Execute(new MealQuery { Mensa = 42, Date = new DateTime(2026, 4, 10) });

        Assert.AreEqual(2, collections.Count);
        CollectionAssert.AreEquivalent(new[] { Category.Dish, Category.Dessert }, collections.Select(c => c.Category).ToArray());
    }

    [TestMethod]
    public void Openmensa_extension_maps_external_meal_to_domain_model()
    {
        var meal = new ClientMeal
        {
            Name = "Chocolate pudding",
            Category = "Desserts",
            Prices = new ClientPrices
            {
                Students = 1.2,
                Employees = 2.3,
                Pupils = 3.4,
                Others = 4.5
            },
            Notes = []
        };

        var mapped = meal.ToIMeal();

        Assert.AreEqual("Chocolate pudding", mapped.Name);
        Assert.AreEqual(Category.Dessert, mapped.Category);
        Assert.AreEqual(1.2, mapped.Price.Students);
        Assert.AreEqual(2.3, mapped.Price.Employees);
    }

    private sealed class MealQuery : IMealQuery
    {
        public int Mensa { get; set; }

        public DateTime Date { get; set; }
    }
}
