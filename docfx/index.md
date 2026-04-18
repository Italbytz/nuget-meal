# nuget-meal

`nuget-meal` is the target repository for the refactored `Italbytz.Meal.*` package family.

## Current stable package family

- `Italbytz.Meal.Abstractions`
- `Italbytz.Meal.OpenMensa`
- `Italbytz.Meal.OpenMensa.Client`
- `Italbytz.Meal.STWPB`
- `Italbytz.Meal.STWPB.Client`
- `Italbytz.Meal.Testing`

The repository now covers the core meal contracts, both OpenMensa and STWPB integrations, and reusable testing helpers in a stable `1.0.0` package line.

## Guide

Use `Guides > Integration clients` for a quick overview of how the former OpenMensa, STWPB, and testing repositories map onto the consolidated package family.

## Local validation

```bash
dotnet restore nuget-meal.sln
dotnet test nuget-meal.sln -v minimal
dotnet pack nuget-meal.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
