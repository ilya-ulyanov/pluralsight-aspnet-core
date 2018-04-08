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

        public bool CityExists(int cityId)
        {
            return this.cityInfoContext.Cities.Any(c => c.Id == cityId);
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
    }
}
