using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("/api/cities")]
    public class CitiesController : Controller
    {
        [HttpGet]
        public IActionResult GetCities()
        {
            return this.Ok(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id)
        {
            Models.CityDTO city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == id);
            return city == null ? this.NotFound() : (IActionResult) this.Ok(city);
        }
    }
}
