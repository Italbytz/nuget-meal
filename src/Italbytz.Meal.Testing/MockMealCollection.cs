using System;
using System.Collections.Generic;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.Testing
{
    public class MockMealCollection : IMealCollection
    {
        public MockMealCollection()
        {
        }

        public Category Category { get; set; }
        public List<IMeal> Meals { get; set; } = new List<IMeal>();
    }
}

