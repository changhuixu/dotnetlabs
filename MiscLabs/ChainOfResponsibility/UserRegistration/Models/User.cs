using System;
using UserRegistration.Exceptions;
using UserRegistration.Handlers.UserValidation;

namespace UserRegistration.Models
{
    public class User
    {
        public string Name { get; }
        public DateTime DateOfBirth { get; }
        public string Department { get; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        public User(string name, string department, DateTime dateOfBirth)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Department = department;
        }

        public bool Register()
        {
            try
            {
                var handler = new AgeValidationHandler();
                handler.SetNext(new NameValidationHandler())
                    .SetNext(new DepartmentValidationHandler());
                handler.Handle(this);
            }
            catch (UserValidationException)
            {
                return false;
            }

            return true;
        }
    }
}
