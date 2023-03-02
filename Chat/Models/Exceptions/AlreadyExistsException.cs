﻿using System.Runtime.Serialization;

namespace ChatWS.Models.Exceptions
{
    public class AlreadyExistsException : Exception
    {
        public AlreadyExistsException()
        {

        }

        public AlreadyExistsException(string? message) : base(message)
        {
        }

        public AlreadyExistsException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
