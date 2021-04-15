namespace PhotoBank.Logger.Service.Data
{
    public class LoggerServiceDBContextFactory
    {
        private readonly string _connectionString;

        public LoggerServiceDBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public LoggerServiceDBContext Make()
        {
            return new LoggerServiceDBContext(_connectionString);
        }
    }
}
