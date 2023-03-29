using Animal_Adoption_Management_System_Backend.Models.Enums;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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


        [HttpGet("{enumName}")]
        public ActionResult<EnumDetails> GetEnum(string enumName)
        {
            EnumDetails enumDetails = _enumService.GetValuesOfEnum(enumName);
            return Ok(enumDetails);
        }
    }
}
