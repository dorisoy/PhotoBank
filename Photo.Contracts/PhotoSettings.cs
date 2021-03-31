using System;

namespace PhotoBank.Photo.Contracts
{
    public static class PhotoSettings
    {
        public static readonly string PhotoInputQueue = "PhotoQueue";
        public static readonly string PhotoOutputQueue = "ResultQueue";
        public static readonly string PhotoDatabasePath = @"D:\Projects\PhotoBank\Photo.Service\Database\Photos";
    }
}
