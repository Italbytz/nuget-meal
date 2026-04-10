using System;
using System.Collections.Generic;
using Italbytz.Meal.Abstractions;

namespace Italbytz.Meal.OpenMensa
{
    public class OpenMensaMealCollection : IMealCollection
    {
        public OpenMensaMealCollection()
        {
        }

        public Category Category { get; set; }
        public List<IMeal> Meals { get; set; } = new List<IMeal>();
    }
}

