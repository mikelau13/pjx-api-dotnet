using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Pjx_Api.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CalendarDbContext _context;
        public ICalendarEventRepository CalendarEvents { get; private set; }

        public UnitOfWork(CalendarDbContext context)
        {
            _context = context;
            CalendarEvents = new CalendarEventRepository(_context);
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
