using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Logger.Service.Data
{
    public class LogRepository : ILogRepository
    {
        private LoggerServiceDBContext _context;

        public LogRepository(LoggerServiceDBContextFactory contextFactory)
        {
            _context = contextFactory.Make();
        }

        public void SaveLog(LogPoco log)
        {
            _context.Logs.Add(log);
            _context.SaveChanges();
        }

        public void Dispose()
        {
            if (_context != null) _context.Dispose();
        }
    }
}
