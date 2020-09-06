using Pjx.CalendarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjx_Api.Data
{
    public interface ICalendarEventRepository: IGenericRepository<CalendarEvent>
    {
        IEnumerable<CalendarEvent> GetAllBetweenByUser(string userId, DateTimeOffset start, DateTimeOffset end);
    }
}
