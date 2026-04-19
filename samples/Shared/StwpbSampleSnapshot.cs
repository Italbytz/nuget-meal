using System;
using System.Collections.Generic;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.STWPB.SampleShared;

public sealed class StwpbMealSnapshot
{
    public DateTime GeneratedAtUtc { get; set; }

    public DateTime? MealDate { get; set; }

    public List<StwpbMealSnapshotItem> Meals { get; set; } = [];
}

public sealed class StwpbMealSnapshotItem
{
    public DateTime? Date { get; set; }

    public string Name { get; set; } = string.Empty;

    public string? Image { get; set; }

    public Category Category { get; set; }

    public Badge[] Badges { get; set; } = [];

    public double? Students { get; set; }

    public double? Employees { get; set; }

    public double? Guests { get; set; }
}