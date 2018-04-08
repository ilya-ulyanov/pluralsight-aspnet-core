using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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
            var result = Mapper.Map<IEnumerable<CityWithoutPointsOfInterestDTO>>(cities);
            return this.Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetCity(int id, bool includePointsOfInterest = false)
        {
            var city = this.cityInfoRepository.GetCity(id, includePointsOfInterest);
            return city == null ? 
                this.NotFound() : 
                includePointsOfInterest ? 
                    this.Ok(Mapper.Map<CityDTO>(city)) : 
                    (IActionResult)this.Ok(Mapper.Map<CityWithoutPointsOfInterestDTO>(city));
        }
    }
}
