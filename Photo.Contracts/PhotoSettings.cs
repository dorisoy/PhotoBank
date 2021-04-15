using System;

namespace PhotoBank.Photo.Contracts
{
    public static class PhotoSettings
    {
        public static string Host { get; } = "PhotoService";

        public static string PhotoInputQueue { get; } = "PhotoQueue";

        public static string PhotoOutputQueue { get; } = "ResultQueue";

        public static string RootPhotoPath { get; set; }
    }
}
