using CityInfo.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public List<CityDTO> Cities { get; private set; }

        public CitiesDataStore()
        {
            this.Cities = new List<CityDTO>
            {
                new CityDTO
                {
                    Id = 1,
                    Name = "New York",
                    Description = "Banking city",
                    PointsOfInterest = new List<PointOfInterestDTO>
                    {
                        new PointOfInterestDTO { Id = 1, Name = "5th ave", Description = "5th ave" },
                        new PointOfInterestDTO { Id = 2, Name = "Central park", Description = "A huge park" },
                    }
                },
                new CityDTO
                {
                    Id = 2,
                    Name = "London",
                    Description = "Another banking city",
                    PointsOfInterest = new List<PointOfInterestDTO>
                    {
                        new PointOfInterestDTO { Id = 3, Name = "Westminster", Description = "Westminster abbey" },
                        new PointOfInterestDTO { Id = 4, Name = "London eye", Description = "London eye" },
                    }
                },
                new CityDTO
                {
                    Id = 3,
                    Name = "Zurich",
                    Description = "Yet another banking city",
                    PointsOfInterest = new List<PointOfInterestDTO>
                    {
                        new PointOfInterestDTO { Id = 5, Name = "Credit Suisse", Description = "A big bank" },
                        new PointOfInterestDTO { Id = 6, Name = "Lindt factory", Description = "Chocolate factory :)" },
                    }
                }
            };
        }
    }
}
