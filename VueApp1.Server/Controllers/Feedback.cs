using Microsoft.AspNetCore.Mvc;
using VueApp1.Server.Models;
using VueApp1.Server.Services;
namespace VueApp1.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedbackController : ControllerBase
    {
        private readonly SuggestionService _suggestionService;
        public FeedbackController(SuggestionService suggestionService)
        {
            _suggestionService = suggestionService;
        }
        [HttpPost("submitMachineSuggestion")]
        async public Task<IActionResult> submitMachineSuggestion([FromBody] MachineSuggestionDto[] suggestions)
        {
            if (suggestions == null || suggestions.Length == 0)
            {
                return BadRequest("No suggestions provided");
            }
            var errors = await _suggestionService.submitMachineSuggestion(suggestions);
            if (errors.Count > 0)
            {
                return BadRequest(new { message = "Some suggestions failed to process", errors });
            }
            return Ok("Process finished succesfully");
        }
    }
}
