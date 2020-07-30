using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;


namespace Pjx_Api
{
    [Authorize]
    public class CountryController : ControllerBase
    {
        [Route("api/country/getall")]
        [HttpGet]
        public IActionResult GetAll()
        {
            return new JsonResult(new string[] { "USA", "CA" });
        }
    }
}
