using System;

namespace Pjx_Api.Data
{
    public interface IUnitOfWork : IDisposable
    {
        ICalendarEventRepository CalendarEvents { get; }
        int Complete();
    }
}
