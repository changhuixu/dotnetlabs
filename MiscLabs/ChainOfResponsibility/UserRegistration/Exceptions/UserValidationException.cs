using System;

namespace UserRegistration.Exceptions
{
    [Serializable]
    public class UserValidationException : ArgumentException
    {
        public UserValidationException(string message) : base(message)
        {

        }
    }
}
