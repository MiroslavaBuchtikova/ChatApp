using ChattingApp.Model;
using ChattingApp.Services.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace ChattingApp.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            if (!ModelState.IsValid) return ProcessBadRequest(ModelState);

            var result = await _userService.LoginAsync(model);
            return Ok(result);
        }

        [HttpPost("/[action]")]
        public async Task<IActionResult> Register([FromBody] RegisterModel model)
        {
            if (!ModelState.IsValid) return ProcessBadRequest(ModelState);

            var result = await _userService.RegisterUser((RegisterModel)model);
            return result.Success 
                ? Ok(result) 
                : BadRequest(result);
        }

        private IActionResult ProcessBadRequest(ModelStateDictionary state) => BadRequest(state.Values
            .First(x => x.ValidationState == ModelValidationState.Invalid)
            .Errors.First().ErrorMessage);
    }
}
