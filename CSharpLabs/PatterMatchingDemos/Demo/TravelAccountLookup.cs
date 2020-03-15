using System;
using System.Collections.Generic;

namespace Demo
{
    public class TravelAccountLookup
    {
        public bool GroupTravel { get; set; }
        public TravelerType TravelerType { get; set; }
        public string TravelPurposeCode { get; set; }
        public string Destination { get; set; } // I -> InState; O -> OutOfState/Domestic; F -> Foreign/International

        public string GetAccountNumber()
        {
            static bool IsRecruit(string purposeCode) => new List<string> { "AR", "SR", "FR" }.Contains(purposeCode);
            return (GroupTravel, TravelerType, IsRecruit(TravelPurposeCode), Destination) switch
            {
                (true, _, _, "I") => "6034",
                (true, _, _, "O") => "6035",
                (true, _, _, "F") => "6036",
                (false, TravelerType.FacultyStaff, _, "I") => "6025",
                (false, TravelerType.FacultyStaff, _, "O") => "6026",
                (false, TravelerType.FacultyStaff, _, "F") => "6027",
                (false, TravelerType.Student, _, "I") => "6030",
                (false, TravelerType.Student, _, "O") => "6031",
                (false, TravelerType.Student, _, "F") => "6032",
                (false, TravelerType.Other, true, "I") => "6050",
                (false, TravelerType.Other, true, "O") => "6051",
                (false, TravelerType.Other, true, "F") => "6057",
                (false, TravelerType.Other, _, "I") => "6055",
                (false, TravelerType.Other, _, "O") => "6056",
                (false, TravelerType.Other, _, "F") => "6067",
                (false, _, _, "I") => "6045",
                (false, _, _, "O") => "6046",
                (false, _, _, "F") => "6047",
                _ => throw new ArgumentException("")
            };
        }
        public string GetAccountNumber2()
        {
            static bool IsRecruit(string purposeCode) => new List<string> { "AR", "SR", "FR" }.Contains(purposeCode);
            return (GroupTravel, TravelerType, Destination) switch
            {
                (true, _, "I") => "6034",
                (true, _, "O") => "6035",
                (true, _, "F") => "6036",
                (false, TravelerType.FacultyStaff, "I") => "6025",
                (false, TravelerType.FacultyStaff, "O") => "6026",
                (false, TravelerType.FacultyStaff, "F") => "6027",
                (false, TravelerType.Student, "I") => "6030",
                (false, TravelerType.Student, "O") => "6031",
                (false, TravelerType.Student, "F") => "6032",
                (false, TravelerType.Other, "I") when IsRecruit(TravelPurposeCode) => "6050",
                (false, TravelerType.Other, "O") when IsRecruit(TravelPurposeCode) => "6051",
                (false, TravelerType.Other, "F") when IsRecruit(TravelPurposeCode) => "6057",
                (false, TravelerType.Other, "I") => "6055",
                (false, TravelerType.Other, "O") => "6056",
                (false, TravelerType.Other, "F") => "6067",
                (false, _, "I") => "6045",
                (false, _, "O") => "6046",
                (false, _, "F") => "6047",
                _ => throw new ArgumentException("")
            };
        }
    }

    public enum TravelerType
    {
        FacultyStaff,
        Student,
        Other
    }
}
