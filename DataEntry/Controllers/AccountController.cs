using dataentry.Extensions;
using dataentry.Services.Business.Users;
using dataentry.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;

namespace dataentry.Controllers
{
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AccountController : Controller
    {
        private IUserLoginService _userLoginService;
        private readonly AzureAdOptions _azureOptions;

        public AccountController(
            IUserLoginService userLoginService,
            IOptions<AzureAdOptions> azureOptions)
        {
            _userLoginService = userLoginService ?? throw new ArgumentNullException(nameof(userLoginService));
            _azureOptions = azureOptions.Value;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody]LoginViewModel loginViewModel)
        {
            var bearerToken = await _userLoginService.Authenticate(loginViewModel.Username, loginViewModel.Password, loginViewModel.Nonce, _azureOptions.ClientId);

            if (bearerToken == null)
                return Unauthorized(new { message = "Username or password is incorrect" });

            return Ok(new { Token = bearerToken, SessionState = Guid.NewGuid(), ClientId = _azureOptions.ClientId });
        }

        [HttpGet("Login")]
        public IActionResult Login()
        {
            return View();
        }
    }
}