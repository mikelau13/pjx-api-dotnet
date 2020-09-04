using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using IdentityServer4.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Pjx.CalendarLibrary.Model;
using Pjx_Api.Data;

namespace Pjx_Api.Controllers.Calendar
{
    /// <summary>
    /// Controller to handle CRUD of user calendar event(s).
    /// </summary>
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
            _logger.LogInformation("EventController.HealthCheck()");
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
            _logger.LogInformation("EventController.Create(EventCreateBindingModel model)");

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
            await _context.SaveChangesAsync();

            return new JsonResult(ce);
        }


        /// <summary>
        /// Update an existing user event.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [Route("event/update")]
        [HttpPut]
        [Authorize]
        public async Task<IActionResult> Update(EventUpdateBindingModel model)
        {
            _logger.LogInformation("EventController.Update(EventUpdateBindingModel model)");

            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            CalendarEvent ce = new CalendarEvent
            {
                EventId = model.Id,
                UserId = userId,
                Title = model.Title,
                Start = model.Start,
                End = model.End
            };

            //TODO: business logic here to validate event

            _context.Update(ce);
            await _context.SaveChangesAsync();

            return new JsonResult(ce);
        }


        /// <summary>
        /// Delete an existing user event.
        /// </summary>
        /// <param name="eventId">CalendarEvent.EventId</param>
        /// <returns></returns>
        [Route("event/delete")]
        [HttpDelete]
        [Authorize]
        public async Task<IActionResult> Delete(int eventId)
        {
            _logger.LogInformation("EventController.Delete({0})", eventId);

            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            CalendarEvent toDel = await _context.CalendarEvents.Where(x => 
                x.EventId == eventId
                && x.UserId == userId
            ).FirstOrDefaultAsync();

            _context.Remove(toDel);

            try {
                await _context.SaveChangesAsync();
                
                return new JsonResult(true);
            } catch {
                return new JsonResult(false);
            }
        }


        /// <summary>
        /// Return all events belonging to a particular user.
        /// </summary>
        /// <param name="start">Calendar start date, inclusive</param>
        /// <param name="end">Calendar end date, exclusive</param>
        /// <returns></returns>
        [Route("event/readAll")]
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> ReadAll(DateTimeOffset start, DateTimeOffset end)
        {
            _logger.LogInformation("EventController.ReadAll('{0}', '{1}')", start, end);

            ClaimsPrincipal currentUser = this.User;
            string userId = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;

            _logger.LogInformation(userId);

            List<CalendarEvent> results = await _context.CalendarEvents.Where(x => 
                x.UserId == userId
                && ((DateTimeOffset.Compare(x.Start, start) >= 0 && DateTimeOffset.Compare(x.Start, end) < 0)
                || (x.End.HasValue && (DateTimeOffset.Compare(x.End.Value, start) >= 0 && DateTimeOffset.Compare(x.End.Value, end) < 0)))
            ).ToListAsync();

            return new JsonResult(results);
        }
    }
}
