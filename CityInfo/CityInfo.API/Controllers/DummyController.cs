using CityInfo.API.Entities;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class DummyController : Controller
    {
        private readonly CityInfoContext cityInfoContext;

        public DummyController(CityInfoContext cityInfoContext)
        {
            this.cityInfoContext = cityInfoContext;
        }

        [HttpGet, Route("api/testdatabase")]
        public IActionResult TestDatabase()
        {
            return Ok();
        }
    }
}
