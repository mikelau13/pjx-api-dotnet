using Pjx.CalendarLibrary.Models;
using System;
using System.Collections.Generic;

namespace Pjx.CalendarLibrary.Repositories
{
    public interface ICalendarEventRepository: IGenericRepository<CalendarEvent>
    {
        IEnumerable<CalendarEvent> GetAllBetweenByUser(string userId, DateTimeOffset start, DateTimeOffset end);
    }
}
