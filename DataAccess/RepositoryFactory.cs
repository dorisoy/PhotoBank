using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.DataAccess
{
    public class RepositoryFactory : IRepositoryFactory
    {
        public TRepository Get<TRepository>() where TRepository : IRepository
        {
            throw new NotImplementedException();
        }
    }
}
