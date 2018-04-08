using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("/api/cities")]
    public class CitiesController : Controller
    {
        private readonly ICityInfoRepository cityInfoRepository;

        public CitiesController(ICityInfoRepository cityInfoRepository)
        {
            this.cityInfoRepository = cityInfoRepository;
        }

        [HttpGet]
        public IActionResult GetCities()
        {
            var cities = this.cityInfoRepository.GetCities();
            var result = new List<CityWithoutPointsOfInterestDTO>();
            foreach (var city in cities)
            {
                result.Add(new CityWithoutPointsOfInterestDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description
                });
            }

            return this.Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = this.cityInfoRepository.GetCity(id, includePointsOfInterest);
            return city == null 
                ? this.NotFound() 
                : (IActionResult)this.Ok(new CityDTO
                {
                    Id = city.Id,
                    Name = city.Name,
                    Description = city.Description,
                    PointsOfInterest = city.PointsOfInterest.Select(poi => new PointOfInterestDTO { Id = poi.Id, Name = poi.Name, Description = poi.Description }).ToList()
                });
        }
    }
}
