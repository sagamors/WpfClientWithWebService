using System;
using System.Runtime.Serialization;
using JetBrains.Annotations;

namespace WpfClient.CustomException
{
    public class RestException : Exception
    {
        public RestException()
        {
        }

        public RestException(string message) : base(message)
        {
        }

        public RestException(string message, System.Exception innerException) : base(message, innerException)
        {
        }

        protected RestException([NotNull] SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}