# nuget-meal

`nuget-meal` is the target repository for the refactored `Italbytz.Meal.*` package family.

## Current Phase 4 slice

- `Italbytz.Meal.Abstractions`
- `Italbytz.Meal.OpenMensa`
- `Italbytz.Meal.OpenMensa.Client`
- `Italbytz.Meal.Testing`

This first wave focuses on the core meal contracts, OpenMensa integration, and reusable testing helpers.

## Local validation

```bash
dotnet restore nuget-meal.sln
dotnet test nuget-meal.sln -v minimal
dotnet pack nuget-meal.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
