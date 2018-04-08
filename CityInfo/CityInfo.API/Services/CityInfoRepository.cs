using System.Collections.Generic;
using System.Linq;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext cityInfoContext;

        public CityInfoRepository(CityInfoContext cityInfoContext)
        {
            this.cityInfoContext = cityInfoContext;
        }

        public void AddPointOfInterestForCity(int cityId, PointOfInterest pointOfInterest)
        {
            var city = this.GetCity(cityId, false);
            city.PointsOfInterest.Add(pointOfInterest);
        }

        public bool CityExists(int cityId)
        {
            return this.cityInfoContext.Cities.Any(c => c.Id == cityId);
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            this.cityInfoContext.PointsOfInterest.Remove(pointOfInterest);
        }

        public IEnumerable<City> GetCities()
        {
            return this.cityInfoContext.Cities.ToList();
        }

        public City GetCity(int cityId, bool includePointsOfInterest)
        {
            IQueryable<City> query = this.cityInfoContext.Cities;
            if (includePointsOfInterest)
            {
                query = query.Include(c => c.PointsOfInterest);
            }

            return query.SingleOrDefault(c => c.Id == cityId);
        }

        public PointOfInterest GetPointOfInterest(int cityId, int pointOfInterestId)
        {
            return this.cityInfoContext.PointsOfInterest.SingleOrDefault(poi => poi.CityId == cityId && poi.Id == pointOfInterestId);
        }

        public IEnumerable<PointOfInterest> GetPointsOfInterest(int cityId)
        {
            return this.cityInfoContext.PointsOfInterest.Where(poi => poi.CityId == cityId).ToList();
        }

        public bool Save()
        {
            return this.cityInfoContext.SaveChanges() > 0;
        }
    }
}
