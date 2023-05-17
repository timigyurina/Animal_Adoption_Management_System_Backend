using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Animal_Adoption_Management_System_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnumController : ControllerBase
    {
        private readonly IEnumService _enumService;

        public EnumController(IEnumService enumService)
        {
            _enumService = enumService;
        }
        

        [HttpGet]
        public ActionResult<Dictionary<string, Dictionary<string, int>>> GetAllEnums()
        {
            Dictionary<string, Dictionary<string, int>> allEnums = _enumService.GetAllEnumsDictionary();
            return Ok(allEnums);
        }
        
        [HttpGet("{enumName}")]
        public ActionResult<EnumDetails> GetEnum(string enumName)
        {
            EnumDetails enumDetails = _enumService.GetValuesOfEnum(enumName);
            return Ok(enumDetails);
        }
    }
}
