using System;

namespace AgeCalculator
{
    public class AgeCalculator
    {
        public static int Calculate(DateTime birthday, DateTime now)
        {
            if (birthday == DateTime.MinValue)
            {
                throw new ArgumentException("Date of birth is invalid.", nameof(birthday));
            }

            if (birthday >= now) return 0;

            var age = now.Year - birthday.Year;
            if (birthday.DayOfYear > now.DayOfYear)
            {
                age--;
            }
            return age;
        }

        public int Calculate2(DateTime birthday)
        {
            var today = DateTime.Today;
            var age = today.Year - birthday.Year;
            if (birthday.DayOfYear > today.DayOfYear)
            {
                age--;
            }
            return age;
        }
    }
}
