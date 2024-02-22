using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OpenAI_API.Audio;
using OpenAI_API.Images;
using TheCats.Application.Entities;
using TheCats.Application.Intefaces.OpenAI;
using TheCats.Core.Entities;

namespace TheCats.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OpenAIController : ControllerBase
    {
        private readonly IOpenAIService _openAIService;
        private readonly IMapper _mapper;

        public OpenAIController(IOpenAIService openAIService, IMapper mapper)
        {
            _openAIService = openAIService;
            _mapper = mapper;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public Task<string> AnsewerMyQuestion(TextTemplateDTO template, EnumAssistant assistant)
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == "userId").Value;
            return _openAIService.AnsewerMyQuestion(template, assistant, userId);
        }

    }
}