using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Contracts
{
    public static class AuthSettings
    {
        public static string Host { get; } = "AuthService";

        public static string AuthInputQueue { get; } = "AuthQueue";

        public static string AuthOutputQueue { get; } = "ResultQueue";

        public static string RootUserPictures { get; set; }
    }
}
