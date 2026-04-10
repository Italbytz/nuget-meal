using Italbytz.Meal.Abstractions;
using Italbytz.Meal.OpenMensa;
using Italbytz.Meal.STWPB;
using Italbytz.Meal.Testing;
using ClientMeal = Italbytz.Meal.OpenMensa.Client.Meal;
using ClientPrices = Italbytz.Meal.OpenMensa.Client.Prices;
using ClientStwpbAllergen = Italbytz.Meal.STWPB.Client.AllergenEnum;
using ClientStwpbCategory = Italbytz.Meal.STWPB.Client.Category;
using ClientStwpbMeal = Italbytz.Meal.STWPB.Client.Meal;

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
        Assert.HasCount(3, meals);
    }

    [TestMethod]
    public async Task Mock_get_meals_service_groups_meals_by_category()
    {
        var service = new MockGetMealsService();

        var collections = await service.Execute(new MealQuery { Mensa = 42, Date = new DateTime(2026, 4, 10) });

        Assert.HasCount(2, collections);
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

    [TestMethod]
    public void Stwpb_json_deserialization_supports_named_categories()
    {
        var json = """
                   [{"name_de":"Pommes","name_en":"Fries","category":"sidedish","category_de":"Beilagen","category_en":"Side Dish","priceStudents":2.5,"priceWorkers":3.5,"priceGuests":4.5,"allergens":["1","A1"],"badges":[],"restaurant":"mensa-hamm","pricetype":"fixed","image":""}]
                   """;

        var meals = Italbytz.Meal.STWPB.Client.Deserialize.ToMeals(json);

        Assert.HasCount(1, meals);
        Assert.AreEqual(ClientStwpbCategory.Sidedish, meals[0].Category);
    }

    [TestMethod]
    public async Task Stwpb_data_source_maps_transport_meals_to_domain_models()
    {
        var transportMeals = new List<ClientStwpbMeal>
        {
            new()
            {
                NameDe = "Pommes",
                NameEn = "Fries",
                Category = ClientStwpbCategory.Sidedish,
                PriceStudents = 2.5,
                PriceWorkers = 3.5,
                PriceGuests = 4.5,
                Allergens = [ClientStwpbAllergen.Z1, ClientStwpbAllergen.A1],
                Image = "https://example.invalid/fries.png"
            }
        };

        var dataSource = new StwpbMealDataSource("demo-id", "de", () => Task.FromResult(transportMeals));

        var meals = await dataSource.RetrieveAll();

        Assert.IsNotNull(meals);
        Assert.HasCount(1, meals);
        Assert.AreEqual("Pommes", meals[0].Name);
        Assert.AreEqual(Category.Sidedish, meals[0].Category);
        Assert.AreEqual(Additives.FoodColoring, meals[0].Additives);
        Assert.AreEqual(Allergens.Gluten, meals[0].Allergens);
        Assert.AreEqual(2.5, meals[0].Price.Students);
        Assert.AreEqual(3.5, meals[0].Price.Employees);
        Assert.AreEqual(4.5, meals[0].Price.Others);
    }

    private sealed class MealQuery : IMealQuery
    {
        public int Mensa { get; set; }

        public DateTime Date { get; set; }
    }
}
