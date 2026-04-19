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

Use `Guides > STWPB Blazor sample` for the public GitHub Pages sample that mirrors the ISD Companion mensa layout.

## Use nuget-meal if you want to

- model meals and prices through reusable abstractions
- call OpenMensa or STWPB through ready-to-use clients and mapping layers
- validate meal-consuming applications with test-friendly sample data
- publish a static Blazor sample for the public STWPB Hamm menu on GitHub Pages

## Live sample

- Documentation: `https://italbytz.github.io/nuget-meal/`
- Blazor sample: `https://italbytz.github.io/nuget-meal/sample/`

The sample is published as a static GitHub Pages app. Its menu snapshot is generated during CI from the public STWPB Hamm endpoint because the endpoint currently does not expose browser-friendly CORS for direct GitHub Pages access.

## Local validation

```bash
dotnet restore nuget-meal.sln
dotnet test nuget-meal.sln -v minimal
dotnet pack nuget-meal.sln -c Release -v minimal
dotnet tool restore
dotnet tool run docfx docfx/docfx.json
```
