using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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

        [Route("healthcheck")]
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
        [Route("event/create")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create(EventCreateBindingModel model)
        {
            _logger.LogInformation("Create(EventCreateBindingModel model)");

            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

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


        /// <summary>
        /// Return all events belonging to a particular user.
        /// </summary>
        /// <param name="start">Calendar start date, inclusive</param>
        /// <param name="end">Calendar end date, exclusive</param>
        /// <returns></returns>
        [Route("event/readall")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReadAll(DateTimeOffset start, DateTimeOffset end)
        {
            _logger.LogInformation("ReadAll('{0}', '{1}')", start, end);

            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation(userId);

            List<CalendarEvent> results = _context.CalendarEvents.Where(x => 
                x.UserId == userId
                && ((DateTimeOffset.Compare(x.Start, start) >= 0 && DateTimeOffset.Compare(x.Start, end) < 0)
                || (x.End.HasValue && (DateTimeOffset.Compare(x.End.Value, start) >= 0 && DateTimeOffset.Compare(x.End.Value, end) < 0)))
            ).ToList();

            return new JsonResult(results);
        }
    }
}
