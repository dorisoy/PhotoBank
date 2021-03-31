using System;
using System.Collections.Generic;
using System.Text;

namespace PhotoBank.Auth.Contracts
{
    public static class AuthSettings
    {
        public static readonly string AuthInputQueue = "AuthQueue";
        public static readonly string AuthOutputQueue = "ResultQueue";
        public static readonly string UserPicturePath = @"D:\Projects\PhotoBank\Auth.Service\Database";
    }
}
