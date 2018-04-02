using CityInfo.API.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CityInfo.API.Controllers
{
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        private const string GetPointOfInterestRouteName = "GetPointOfInterest";

        [HttpGet("{cityId}/pointsOfInterest")]
        public IActionResult GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return this.NotFound();
            }

            return this.Ok(city.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointsOfInterest/{pointOfInterestId}", Name = GetPointOfInterestRouteName)]
        public IActionResult GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if (city == null)
            {
                return this.NotFound();
            }

            var poi = city.PointsOfInterest.SingleOrDefault(p => p.Id == pointOfInterestId);
            if (poi == null)
            {
                return this.NotFound();
            }

            return this.Ok(poi);
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

            var city = CitiesDataStore.Current.Cities.SingleOrDefault(c => c.Id == cityId);
            if(city == null)
            {
                return this.NotFound($"City does not exist for cityId={cityId}");
            }

            int nextPoiId = CitiesDataStore.Current.Cities.SelectMany(c => c.PointsOfInterest).Max(poi => poi.Id) + 1;
            var newPoi = new PointOfInterestDTO { Id = nextPoiId, Name = pointOfInterest.Name, Description = pointOfInterest.Description };
            city.PointsOfInterest.Add(newPoi);
            return this.CreatedAtRoute(GetPointOfInterestRouteName, new { cityId, pointOfInterestId = nextPoiId }, newPoi);
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
            return this.NoContent();
        }
    }
}
