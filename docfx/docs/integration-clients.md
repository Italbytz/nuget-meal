# Integration clients and testing helpers

This guide collects the practical orientation that used to be spread across the older meal-related repositories and maps it to the consolidated `Italbytz.Meal.*` package family.

## Which package should I use?

| Need | Package |
| --- | --- |
| shared meal contracts | `Italbytz.Meal.Abstractions` |
| raw OpenMensa HTTP client and transport types | `Italbytz.Meal.OpenMensa.Client` |
| ready-to-use OpenMensa meal service and mappings | `Italbytz.Meal.OpenMensa` |
| raw STWPB transport models and API access | `Italbytz.Meal.STWPB.Client` |
| ready-to-use STWPB meal data source | `Italbytz.Meal.STWPB` |
| mocks and demo-friendly sample services | `Italbytz.Meal.Testing` |

## OpenMensa quick start

If you want direct OpenMensa API access, start with the client package:

```csharp
using Italbytz.Meal.OpenMensa.Client;

var api = new OpenMensaAPI();
var canteens = await api.GetCanteens();
var meals = await api.GetTodaysMeals(canteens[0].Id);
```

If you prefer to stay at the service layer, use the consolidated service implementation:

```csharp
using Italbytz.Meal.Abstractions;
using Italbytz.Meal.OpenMensa;

IGetMealsService service = new OpenMensaGetMealsService();
var collections = await service.Execute(query);
```

Here `query` is your application-specific implementation of `IMealQuery`.

## STWPB quick start

For the Studentenwerk Paderborn integration path, the consolidated data source is the simplest entry point:

```csharp
using Italbytz.Meal.STWPB;

var dataSource = new StwpbMealDataSource("de");
var meals = await dataSource.RetrieveAll();
```

This default path uses the public Hamm WordPress endpoint and no longer depends on the legacy access id.

If you still have a legacy STWPB id-based integration, you can keep using the compatibility constructor:

```csharp
using Italbytz.Meal.STWPB;

var dataSource = new StwpbMealDataSource("legacy-id", "de");
var meals = await dataSource.RetrieveAll();
```

You can also inject your own fetch delegate when you want to test or mock the transport layer explicitly.

## Testing helpers

For tests, demos, or classroom examples, use the mock service from `Italbytz.Meal.Testing`:

```csharp
using Italbytz.Meal.Abstractions;
using Italbytz.Meal.Testing;

IGetMealsService service = new MockGetMealsService();
var collections = await service.Execute(query);
```

This returns stable example meal collections without requiring a live external service.

## Historical mapping

The older `nuget-ports-meal`, `nuget-adapters-meal-openmensa`, `nuget-infrastructure-openmensa`, `nuget-adapters-meal-stwpb`, `nuget-infrastructure-stwpb`, and `nuget-adapters-meal-mock` repositories are now represented together by the consolidated `Italbytz.Meal.*` family.