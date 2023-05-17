using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Models.Exceptions;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;

namespace Animal_Adoption_Management_System_Backend.Services.Implementations
{
    public class EnumService : IEnumService
    {
        private readonly IEnumerable<EnumDetails> _enums;
        private readonly Dictionary<string, Dictionary<string, int>> _enumDictionary;

        public EnumService()
        {
            _enums = GenerateEnumDetailslList();
            _enumDictionary = GenerateEnumDictionary();
        }

        public Dictionary<string, Dictionary<string, int>> GetAllEnumsDictionary()
        {
            return _enumDictionary;
        }

        public EnumDetails GetValuesOfEnum(string enumName)
        {
            EnumDetails? enumDetails = _enums.FirstOrDefault(x => x.Name == enumName);
            if (enumDetails == null)
                throw new BadRequestException($"Enum with type {enumName} does not exist");
            return enumDetails;
        }


        private static Dictionary<string, Dictionary<string, int>> GenerateEnumDictionary()
        {
            Dictionary<string, Dictionary<string, int>> enumDictionary = new()
            {
                { "animalColor", GetDictionaryOfEnum<AnimalColor>() },
                { "animalSize", GetDictionaryOfEnum<AnimalSize>() },
                { "animalStatus", GetDictionaryOfEnum<AnimalStatus>() },
                { "animalType", GetDictionaryOfEnum<AnimalType>() },
                { "applicationStatus", GetDictionaryOfEnum<ApplicationStatus>() },
                { "donationStatus", GetDictionaryOfEnum<DonationStatus>() },
                { "gender", GetDictionaryOfEnum<Gender>() },
            };
            return enumDictionary;
        }

        private static IEnumerable<EnumDetails> GenerateEnumDetailslList()
        {
            EnumDetails animalColorEnumDetails = new("AnimalColor", GetDictionaryOfEnum<AnimalColor>());
            EnumDetails animalSizeEnumDetails = new("AnimalSize", GetDictionaryOfEnum<AnimalSize>());
            EnumDetails animalStatusEnumDetails = new("AnimalStatus", GetDictionaryOfEnum<AnimalStatus>());
            EnumDetails animalTypeEnumDetails = new("AnimalType", GetDictionaryOfEnum<AnimalType>());
            EnumDetails applicationStatusEnumDetails = new("ApplicationStatus", GetDictionaryOfEnum<ApplicationStatus>());
            EnumDetails donationStatusEnumDetails = new("DonationStatus", GetDictionaryOfEnum<DonationStatus>());
            EnumDetails genderEnumDetails = new("Gender", GetDictionaryOfEnum<Gender>());

            return new EnumDetails[] { animalColorEnumDetails, animalSizeEnumDetails, animalStatusEnumDetails, animalTypeEnumDetails, applicationStatusEnumDetails, donationStatusEnumDetails, genderEnumDetails };
        }

        private static Dictionary<string, int> GetDictionaryOfEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            IEnumerable<TEnum> enumValues = GetValuesOfEnum<TEnum>();
            Dictionary<string, int> enumDictionary = enumValues.ToDictionary(x => x.ToString()!, x => Convert.ToInt32(x));
            return enumDictionary;
        }
        private static IEnumerable<TEnum> GetValuesOfEnum<TEnum>() where TEnum : struct, IComparable, IFormattable, IConvertible
        {
            Type? enumType = typeof(TEnum);

            if (!enumType.IsEnum)
                throw new BadRequestException($"Enum with type {enumType} does not exist");

            return Enum.GetValues(enumType).Cast<TEnum>();
        }
    }
}
