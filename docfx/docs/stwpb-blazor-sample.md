# STWPB Blazor sample

The repository publishes a public Blazor sample for the Hamm menu at `https://italbytz.github.io/nuget-meal/sample/`.

## What the sample shows

- grouped meal cards inspired by the mensa page in ISD Companion
- student, staff, and guest prices
- badges such as vegan and vegetarian when they are present in the STWPB data
- the next available meal day if the requested date has no entries

## Why the sample uses a snapshot

The public STWPB Hamm endpoint is suitable for CI-side retrieval, but it currently does not expose the browser CORS headers required for a static GitHub Pages app to call it directly from WebAssembly.

Because of that, the sample is published with a fresh JSON snapshot that is generated during CI from the public endpoint and then bundled into the static site.

## Local workflow

Generate the snapshot and publish the sample locally:

```bash
dotnet run --project samples/Italbytz.Meal.STWPB.SampleDataBuilder/Italbytz.Meal.STWPB.SampleDataBuilder.csproj -- samples/Italbytz.Meal.STWPB.Blazor.Sample/wwwroot/data/hamm-meals.json
dotnet publish samples/Italbytz.Meal.STWPB.Blazor.Sample/Italbytz.Meal.STWPB.Blazor.Sample.csproj -c Release
```

The GitHub Actions workflow also refreshes the snapshot on weekdays so the public sample stays aligned with the current menu.