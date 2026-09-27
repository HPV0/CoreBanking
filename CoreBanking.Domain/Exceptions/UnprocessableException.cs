using System;
using System.Collections.Generic;
using System.Text;

namespace CoreBanking.Application.Exceptions
{
    public class UnprocessableException : Exception
    {
        public UnprocessableException()
            : base("Unprocessable argument in buissness logic.")
        {
        }

        public UnprocessableException(string message)
            : base(message)
        {
        }

        public UnprocessableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

    }
}
