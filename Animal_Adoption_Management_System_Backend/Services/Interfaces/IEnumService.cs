using Animal_Adoption_Management_System_Backend.Models.Enums;

namespace Animal_Adoption_Management_System_Backend.Services.Interfaces
{
    public interface IEnumService
    {
        EnumDetails GetValuesOfEnum(string enumName);
    }
}
