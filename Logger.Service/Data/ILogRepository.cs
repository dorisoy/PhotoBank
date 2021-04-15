using System;
using System.Collections.Generic;
using System.Text;
using PhotoBank.DataAccess;

namespace PhotoBank.Logger.Service.Data
{
    public interface ILogRepository : IRepository
    {
        void SaveLog(LogPoco log);
    }
}
