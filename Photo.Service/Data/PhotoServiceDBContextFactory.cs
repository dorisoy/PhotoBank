namespace PhotoBank.Photo.Service.Data
{
    public class PhotoServiceDBContextFactory
    {
        private readonly string _connectionString;

        public PhotoServiceDBContextFactory(string connectionString)
        {
            _connectionString = connectionString;
        }

        public PhotoServiceDBContext Make()
        {
            return new PhotoServiceDBContext(_connectionString);
        }
    }
}
