using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TheCats.Application.Entities;
using TheCats.Application.Intefaces.Assistant;
using TheCats.Application.Intefaces.User;
using TheCats.Core.Entities;

namespace TheCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AssistantController : ControllerBase
    {
        private readonly IAssistantService _assistantService;
        private readonly IMapper _mapper;

        public AssistantController(IAssistantService assistantService, IMapper mapper)
        {
            _assistantService = assistantService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public void SaveUser(AssistantDTO assistantDTO)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "userId").Value;
            _assistantService.Save(assistantDTO, userId);
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public AssistantDTO AssistantConfig()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "userId").Value;
            return _assistantService.GetByUserId(userId);
        }


    }
}