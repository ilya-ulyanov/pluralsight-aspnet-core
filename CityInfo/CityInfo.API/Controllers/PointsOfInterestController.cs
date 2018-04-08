using AutoMapper;
using CityInfo.API.Entities;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private readonly ILogger<PointsOfInterestController> logger;
        private readonly IMailService mailService;
        private readonly ICityInfoRepository cityInfoRepository;

        public PointsOfInterestController(ILogger<PointsOfInterestController> logger, IMailService mailService, ICityInfoRepository cityInfoRepository)
        {
            this.logger = logger;
            this.mailService = mailService;
            this.cityInfoRepository = cityInfoRepository;
        }

        private const string GetPointOfInterestRouteName = "GetPointOfInterest";

        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            this.logger.LogInformation($"Got request for city Id= {cityId}");
            if (!this.cityInfoRepository.CityExists(cityId))
            {
                this.logger.LogWarning($"City Id={cityId} cannot be found");
                return this.NotFound();
            }

            var pois = this.cityInfoRepository.GetPointsOfInterest(cityId);
            return this.Ok(Mapper.Map<IEnumerable<PointOfInterestDTO>>(pois));
        }

        [HttpGet("{cityId}/pointsOfInterest/{pointOfInterestId}", Name = GetPointOfInterestRouteName)]
        public IActionResult GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            if (!this.cityInfoRepository.CityExists(cityId))
            {
                this.logger.LogWarning($"City Id={cityId} cannot be found");
                return this.NotFound();
            }

            var poi = this.cityInfoRepository.GetPointsOfInterest(cityId).SingleOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return this.NotFound();
            }

            return this.Ok(Mapper.Map<PointOfInterestDTO>(poi));
        }

        [HttpPost("{cityId}/pointsOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointOfInterestForCreationDTO pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return this.BadRequest(this.ModelState);
            }

            if (pointOfInterest.Name.Equals(pointOfInterest.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                this.ModelState.AddModelError(nameof(pointOfInterest.Description), "Provided description should be different from name.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if(!this.cityInfoRepository.CityExists(cityId))
            {
                return this.NotFound($"City does not exist for cityId={cityId}");
            }

            var newPoi = Mapper.Map<PointOfInterest>(pointOfInterest);
            this.cityInfoRepository.AddPointOfInterestForCity(cityId, newPoi);
            if (!this.cityInfoRepository.Save())
            {
                return this.StatusCode(500, "A problem happened");
            }

            var result = Mapper.Map<PointOfInterestDTO>(newPoi);
            return this.CreatedAtRoute(GetPointOfInterestRouteName, new { cityId, pointOfInterestId = newPoi.Id }, result);
        }

        [HttpPut("{cityId}/pointsOfInterest/{pointOfInterestId}")]
        public IActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, [FromBody] PointOfInterestForUpdateDTO pointOfInterest)
        {
            if (pointOfInterest == null)
            {
                return this.BadRequest(this.ModelState);
            }

            if (pointOfInterest == null)
            {
                return this.BadRequest(this.ModelState);
            }

            if (pointOfInterest.Name.Equals(pointOfInterest.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                this.ModelState.AddModelError(nameof(pointOfInterest.Description), "Provided description should be different from name.");
            }

            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return this.NotFound($"City does not exist for cityId={cityId}");
            }

            var poi = city.PointsOfInterest.SingleOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return this.NotFound($"Points of interest does not exist for pointOfInterestId={pointOfInterestId}");
            }

            poi.Name = pointOfInterest.Name;
            poi.Description = pointOfInterest.Description;

            return this.NoContent();
        }

        [HttpPatch("{cityId}/pointsOfInterest/{pointOfInterestId}")]
        public IActionResult PartiallyUpdatePointOfInterest(int cityId, int pointOfInterestId, [FromBody] JsonPatchDocument<PointOfInterestForUpdateDTO> patchDocument)
        {
            if (patchDocument == null)
            {
                return this.BadRequest();
            }

            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return this.NotFound($"City does not exist for cityId={cityId}");
            }

            var poi = city.PointsOfInterest.SingleOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return this.NotFound($"Points of interest does not exist for pointOfInterestId={pointOfInterestId}");
            }

            var poiToPatch = new PointOfInterestForUpdateDTO
            {
                Name = poi.Name,
                Description = poi.Description
            };

            patchDocument.ApplyTo(poiToPatch, this.ModelState);
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            if (poiToPatch.Name.Equals(poiToPatch.Description, StringComparison.InvariantCultureIgnoreCase))
            {
                this.ModelState.AddModelError(nameof(poiToPatch.Description), "Provided description should be different from name.");
            }

            this.TryValidateModel(poiToPatch);
            if (!this.ModelState.IsValid)
            {
                return this.BadRequest(this.ModelState);
            }

            poi.Name = poiToPatch.Name;
            poi.Description = poiToPatch.Description;

            return this.NoContent();
        }

        [HttpDelete("{cityId}/pointsOfInterest/{pointOfInterestId}")]
        public IActionResult DeletePointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return this.NotFound($"City does not exist for cityId={cityId}");
            }

            var poi = city.PointsOfInterest.SingleOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return this.NotFound($"Points of interest does not exist for pointOfInterestId={pointOfInterestId}");
            }

            city.PointsOfInterest.Remove(poi);
            this.mailService.Send("POI was removed", $"{poi.Name} was removed from {city.Name}");
            return this.NoContent();
        }
    }
}
