using System;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.Testing
{
    internal class Price : IPrice
    {
        public Price()
        {
        }

        public double? Students { get; set; }
        public double? Employees { get; set; }
        public double? Pupils { get; set; }
        public double? Others { get; set; }
    }
}

