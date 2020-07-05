using System;

namespace PhotoBank.DataAccess
{
    public interface IRepositoryFactory
    {
        TRepository Get<TRepository>() where TRepository : IRepository;
    }
}
