# nuget-meal

`nuget-meal` bundles the refactored `Italbytz.Meal.*` package family for mensa and meal-plan scenarios.

It is intended for developers who need reusable meal contracts, OpenMensa integration, and sample/testing helpers for apps, demos, and teaching material.

## Which package should I use?

- Use `Italbytz.Meal.Abstractions` for contracts such as `IMeal`, `IMealCollection`, `IMealQuery`, `IPrice`, and `IGetMealsService`.
- Use `Italbytz.Meal.OpenMensa.Client` when you need the raw OpenMensa API client and transport models.
- Use `Italbytz.Meal.OpenMensa` for the ready-to-use OpenMensa meal service, data source, and mapping helpers.
- Use `Italbytz.Meal.STWPB.Client` when you need the raw Studentenwerk Paderborn (STWPB) transport models and API access.
- Use `Italbytz.Meal.STWPB` for the ready-to-use STWPB meal data source and mapping helpers.
- Use `Italbytz.Meal.Testing` for mock meals, mock collections, and test-friendly example services.

## Documentation

API documentation is generated with `docfx` and can be published via GitHub Pages:

- `https://italbytz.github.io/nuget-meal/`

The doc site now also includes an integration guide that explains how OpenMensa, STWPB, and the testing helpers fit together in the consolidated package family.

## Quality checks

This repository includes:

- a `GitHub Actions` workflow in `.github/workflows/ci.yml`
- automated `restore`, `build`, `test`, `pack`, and docs generation
- a `docfx` setup under `docfx/`

## Release model

- the current `nuget-meal` line stays on `1.0.0-preview.*` while the `STWPB` follow-up wave is still pending
- a pushed tag such as `v1.0.0-preview.1` triggers the release-ready pipeline in GitHub Actions
- if the repository secret `NUGET_API_KEY` is configured, the workflow also publishes `.nupkg` and `.snupkg` files to NuGet

## Local validation

```bash
dotnet restore nuget-meal.sln
dotnet test nuget-meal.sln -v minimal
dotnet pack nuget-meal.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
