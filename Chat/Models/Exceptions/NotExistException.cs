using System.Runtime.Serialization;

namespace ChatWS.Models.Exceptions
{
    public class NotExistException : Exception
    {
        public NotExistException()
        {
        }

        public NotExistException(string? message) : base(message)
        {
        }

        public NotExistException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
