using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IEnumService
    {
        Dictionary<string, Dictionary<string, int>> GetAllEnumsDictionary();
        EnumDetails GetValuesOfEnum(string enumName);
    }
}
