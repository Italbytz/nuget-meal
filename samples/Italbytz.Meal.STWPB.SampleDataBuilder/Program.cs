using System.Text.Json;
using System.Text.Json.Serialization;
using Italbytz.Meal.Abstractions;
using Italbytz.Meal.STWPB;
using Italbytz.Meal.STWPB.SampleShared;

var outputPath = args.Length > 0
    ? Path.GetFullPath(args[0])
    : Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../Italbytz.Meal.STWPB.Blazor.Sample/wwwroot/data/hamm-meals.json"));
var language = args.Length > 1 ? args[1] : "de";

var dataSource = new StwpbMealDataSource(language);
var meals = await dataSource.RetrieveAll() ?? [];
var orderedMeals = meals
    .OrderBy(meal => meal.Date == DateTime.MinValue ? DateTime.MaxValue : meal.Date)
    .ThenBy(meal => GetCategoryOrder(meal.Category))
    .ThenBy(meal => meal.Name, StringComparer.OrdinalIgnoreCase)
    .Select(meal => new StwpbMealSnapshotItem
    {
        Date = meal.Date == DateTime.MinValue ? null : meal.Date,
        Name = meal.Name,
        Image = string.IsNullOrWhiteSpace(meal.Image) ? null : meal.Image,
        Category = meal.Category,
        Badges = meal.Badges,
        Students = meal.Price.Students,
        Employees = meal.Price.Employees,
        Guests = meal.Price.Others,
    })
    .ToList();

var snapshot = new StwpbMealSnapshot
{
    GeneratedAtUtc = DateTime.UtcNow,
    MealDate = orderedMeals
        .Select(meal => meal.Date)
        .Where(date => date.HasValue)
        .OrderBy(date => date)
        .FirstOrDefault(),
    Meals = orderedMeals,
};

Directory.CreateDirectory(Path.GetDirectoryName(outputPath)!);

await File.WriteAllTextAsync(
    outputPath,
    JsonSerializer.Serialize(
        snapshot,
        new JsonSerializerOptions
        {
            WriteIndented = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
            Converters = { new JsonStringEnumConverter() }
        }));

static int GetCategoryOrder(Category category) => category switch
{
    Category.Dish => 0,
    Category.Soup => 1,
    Category.Sidedish => 2,
    Category.Dessert => 3,
    _ => 4,
};