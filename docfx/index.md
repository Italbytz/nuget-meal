# nuget-meal

`nuget-meal` provides reusable meal contracts, provider integrations, and testing helpers for .NET applications.

This documentation is intended for package consumers who want to integrate OpenMensa or STWPB meal data without rebuilding the surrounding transport and mapping logic.

## Packages at a glance

- `Italbytz.Meal.Abstractions`
- `Italbytz.Meal.OpenMensa`
- `Italbytz.Meal.OpenMensa.Client`
- `Italbytz.Meal.STWPB`
- `Italbytz.Meal.STWPB.Client`
- `Italbytz.Meal.Testing`

These packages cover core meal contracts, both OpenMensa and STWPB integrations, and reusable testing helpers.

## Guide

Use `Guides > Integration clients` for a quick overview of how the former OpenMensa, STWPB, and testing repositories map onto the consolidated package family.

## Use nuget-meal if you want to

- model meals and prices through reusable abstractions
- call OpenMensa or STWPB through ready-to-use clients and mapping layers
- validate meal-consuming applications with test-friendly sample data

## Local validation

```bash
dotnet restore nuget-meal.sln
dotnet test nuget-meal.sln -v minimal
dotnet pack nuget-meal.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
