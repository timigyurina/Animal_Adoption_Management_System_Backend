namespace Animal_Adoption_Management_System_Backend.Models.Enums
{
    public class EnumDetails
    {
        private readonly string _name;
        private readonly Dictionary<string, int> _values;
        public EnumDetails(string name, Dictionary<string, int> values)
        {
            _name = name;
            _values = values;
        }

        public string Name => _name;
        public Dictionary<string, int> Values => _values;
    }
}
