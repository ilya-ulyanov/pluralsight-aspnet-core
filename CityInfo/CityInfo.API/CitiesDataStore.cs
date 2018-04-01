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
                new CityDTO { Id = 1, Name = "New York", Description = "Banking city" },
                new CityDTO { Id = 2, Name = "London", Description = "Another banking city" },
                new CityDTO { Id = 3, Name = "Zurich", Description = "Yet another banking city" }
            };
        }
    }
}
