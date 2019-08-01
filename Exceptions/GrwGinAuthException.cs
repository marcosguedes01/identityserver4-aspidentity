using System;
using Serilog;

namespace Grw.Gin.Auth.Exceptions
{
    public class GrwGinAuthException : Exception
    {
        public GrwGinAuthException(string message)
            : base(message)
        {
            Log.Error(this, message);
        }
    }
}
