using UserRegistration.Exceptions;
using UserRegistration.Models;

namespace UserRegistration.Handlers.UserValidation
{
    public class DepartmentValidationHandler : Handler<User>
    {
        public override void Handle(User user)
        {
            if (user.Department == "NO")
            {
                throw new UserValidationException("We currently do not support Department [NO].");
            }

            base.Handle(user);
        }
    }
}
