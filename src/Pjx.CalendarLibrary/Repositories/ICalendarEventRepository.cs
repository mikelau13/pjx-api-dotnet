using Pjx.CalendarLibrary.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Pjx.CalendarLibrary.Repositories
{
    public interface ICalendarEventRepository
    {
        IEnumerable<CalendarEvent> ReadAllCalendarEvent(DateTimeOffset start, DateTimeOffset end);
        CalendarEvent ReadCalendarEvent(int id);
        void CreateCalendarEvent(int id);
        void UpdateCalendarEvent(int id);
        void DeleteCalendarEvent(int id);
    }
}
