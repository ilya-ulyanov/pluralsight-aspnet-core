using CityInfo.API.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public static class CityInfoExtensions
    {
        public static void EnsureSeedDataForContext(this CityInfoContext context)
        {
            if (context.Cities.Any())
            {
                return;
            }

            var cities = new List<City>
            {
                new City
                {
                    Name = "New York",
                    Description = "Banking city",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "5th ave", Description = "5th ave" },
                        new PointOfInterest { Name = "Central park", Description = "A huge park" },
                    }
                },
                new City
                {
                    Name = "London",
                    Description = "Another banking city",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "Westminster", Description = "Westminster abbey" },
                        new PointOfInterest { Name = "London eye", Description = "London eye" },
                    }
                },
                new City
                {
                    Name = "Zurich",
                    Description = "Yet another banking city",
                    PointsOfInterest = new List<PointOfInterest>
                    {
                        new PointOfInterest { Name = "Credit Suisse", Description = "A big bank" },
                        new PointOfInterest { Name = "Lindt factory", Description = "Chocolate factory :)" },
                    }
                }
            };

            context.Cities.AddRange(cities);
            context.SaveChanges();
        }
    }
}
