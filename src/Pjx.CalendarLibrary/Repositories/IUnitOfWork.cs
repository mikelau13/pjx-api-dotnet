using System;

namespace Pjx.CalendarLibrary.Repositories
{
    public interface IUnitOfWork : IDisposable
    {
        ICalendarEventRepository CalendarEvents { get; }
        int Complete();
    }
}
