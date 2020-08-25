using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Pjx.CalendarLibrary.Model;
using Pjx_Api.Data;

namespace Pjx_Api.Controllers.Calendar
{
    [Route("api/calendar")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly CalendarDbContext _context;
        private readonly ILogger<EventController> _logger;

        public EventController(CalendarDbContext context, ILogger<EventController> logger)
        {
            _logger = logger;
            _context = context;
        }

        [Route("api/calendar/healthcheck")]
        [HttpGet]
        [AllowAnonymous]
        public IActionResult HealthCheck()
        {
            _logger.LogInformation("HealthCheck()");
            return new JsonResult("okay");
        }

        /// <summary>
        /// Create an user event.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("api/calendar/event/create")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EventCreateBindingModel model)
        {
            _logger.LogInformation("Create(EventCreateBindingModel model)");

            string userId = User.GetSubjectId();

            CalendarEvent ce = new CalendarEvent
            {
                UserId = userId,
                Title = model.Title,
                Start = model.Start,
                End = model.End
            };

            //TODO: business logic here to validate event

            _context.Add(ce);
            _context.SaveChanges();

            return new JsonResult(ce);
        }
    }
}
