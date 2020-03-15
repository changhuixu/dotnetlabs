namespace Demo
{
    public class ChemicalRequest
    {
        public bool IsUserAuthorized { get; set; }
        public bool IsChemicalAvailable { get; set; }
        public bool IsChemicalHazardous { get; set; }
        public bool IsRequesterTrained { get; set; }

        public bool AcceptRequest()
        {
            if (!IsUserAuthorized)
            {
                return false;
            }

            if (!IsChemicalAvailable)
            {
                return false;
            }

            if (!IsChemicalHazardous)
            {
                return true;
            }

            if (!IsRequesterTrained)
            {
                return false;
            }

            return true;
        }

        public bool ShouldAccept()
        {
            return (IsUserAuthorized, IsChemicalAvailable, IsChemicalHazardous, IsRequesterTrained) switch
            {
                (false, _, _, _) => false,
                (true, true, true, false) => false,
                (true, false, _, _) => false,
                (true, true, false, _) => true,
                (true, true, true, true) => true
            };
        }
    }
}
