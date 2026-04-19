using Italbytz.Meal.Abstractions;
using Italbytz.Meal.OpenMensa;
using Italbytz.Meal.STWPB;
using Italbytz.Meal.Testing;
using System.Net;
using System.Net.Http;
using System.Text;
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

    [TestMethod]
    public async Task Stwpb_data_source_supports_public_hamm_flow_without_legacy_id()
    {
        var transportMeals = new List<ClientStwpbMeal>
        {
            new()
            {
                NameDe = "Currywurst vegan",
                Category = ClientStwpbCategory.Dish,
                PriceStudents = 3.8,
                PriceWorkers = 5.6,
                PriceGuests = 6.8,
                Allergens = [],
                Badges = [Italbytz.Meal.STWPB.Client.Badge.Vegan]
            }
        };

        var dataSource = new StwpbMealDataSource("de", () => Task.FromResult(transportMeals));

        var meals = await dataSource.RetrieveAll();

        Assert.IsNotNull(meals);
        Assert.HasCount(1, meals);
        Assert.AreEqual("Currywurst vegan", meals[0].Name);
        CollectionAssert.AreEquivalent(new[] { Italbytz.Meal.Abstractions.Badge.Vegan }, meals[0].Badges);
    }

    [TestMethod]
    public async Task Stwpb_api_client_reads_wordpress_meal_plan_payload()
    {
        const string json = """
            {
                "meals": [
                    {
                        "date": "2026-04-20",
                        "title": "Lahmacun mit Rotkraut und Dip",
                        "category": "Vegan 1",
                        "allergens_raw": "A1, a, A6, A10",
                        "price_students": "3,80",
                        "price_staff": "5,60",
                        "price_guests": "6,80",
                        "image_jpeg": "https://example.invalid/lahmacun.jpg",
                        "image_jpeg_thumb": "https://example.invalid/lahmacun-thumb.jpg"
                    },
                    {
                        "date": "2026-04-21",
                        "title": "Späteres Gericht",
                        "category": "Fleisch/Fisch",
                        "allergens_raw": "A3",
                        "price_students": "4,20",
                        "price_staff": "6,10",
                        "price_guests": "7,40"
                    }
                ]
            }
            """;

        var handler = new StubHttpMessageHandler(json);
        var api = new Italbytz.Meal.STWPB.Client.MensaAPI("de", new HttpClient(handler)
        {
            BaseAddress = new Uri("https://stwpb.de")
        });

        var meals = await api.GetTodaysHammMeals(new DateTime(2026, 4, 20));

        Assert.HasCount(1, meals);
        Assert.AreEqual("/wp-json/stwk-pb/v1/meals?venue=mensa-hamm&start_date=2026-04-20&end_date=2026-04-26", handler.LastRequestUri?.PathAndQuery);
        Assert.AreEqual("Lahmacun mit Rotkraut und Dip", meals[0].NameDe);
        Assert.AreEqual(ClientStwpbCategory.Dish, meals[0].Category);
        CollectionAssert.AreEquivalent(new[] { ClientStwpbAllergen.A1, ClientStwpbAllergen.A6, ClientStwpbAllergen.A10 }, meals[0].Allergens);
        CollectionAssert.AreEquivalent(new[] { Italbytz.Meal.STWPB.Client.Badge.Vegan }, meals[0].Badges);
        Assert.AreEqual(3.8, meals[0].PriceStudents);
        Assert.AreEqual(5.6, meals[0].PriceWorkers);
        Assert.AreEqual(6.8, meals[0].PriceGuests);
    }

    [TestMethod]
    public void Stwpb_api_client_requires_legacy_id_for_legacy_endpoint()
    {
        var api = new Italbytz.Meal.STWPB.Client.MensaAPI("de");

        try
        {
            api.GetMeals().GetAwaiter().GetResult();
            Assert.Fail("Expected InvalidOperationException.");
        }
        catch (InvalidOperationException)
        {
        }
    }

    [TestMethod]
    public async Task Stwpb_api_client_falls_forward_to_next_available_day()
    {
        const string json = """
            {
                "meals": [
                    {
                        "date": "2026-04-20",
                        "title": "Apfel-Joghurt-Quark",
                        "category": "Stamm Dessert 0,80€",
                        "allergens_raw": "15, A7",
                        "price_students": "0,80",
                        "price_staff": "1,30",
                        "price_guests": "2,35"
                    }
                ]
            }
            """;

        var api = new Italbytz.Meal.STWPB.Client.MensaAPI("de", new HttpClient(new StubHttpMessageHandler(json))
        {
            BaseAddress = new Uri("https://stwpb.de")
        });

        var meals = await api.GetTodaysHammMeals(new DateTime(2026, 4, 18));

        Assert.HasCount(1, meals);
        Assert.AreEqual(new DateTime(2026, 4, 20), meals[0].Date.Date);
        Assert.AreEqual(ClientStwpbCategory.Dessert, meals[0].Category);
    }

    private sealed class MealQuery : IMealQuery
    {
        public int Mensa { get; set; }

        public DateTime Date { get; set; }
    }

    private sealed class StubHttpMessageHandler(string json) : HttpMessageHandler
    {
        public Uri? LastRequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(json, Encoding.UTF8, "application/json")
            });
        }
    }
}
