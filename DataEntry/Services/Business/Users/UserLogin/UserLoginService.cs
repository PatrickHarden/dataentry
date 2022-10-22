using dataentry.Repository;
using dataentry.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace dataentry.Services.Business.Users
{

    public class UserLoginService : IUserLoginService
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly ApplicationUserManager _userManager;
        private readonly IdentitySettings _identitySettings;

        public UserLoginService(
            SignInManager<IdentityUser> signInManager,
            ApplicationUserManager userManager,
            IOptions<IdentitySettings> identitySettings)
        {
            _signInManager = signInManager;
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _identitySettings = identitySettings?.Value ?? throw new ArgumentNullException(nameof(identitySettings));
        }

        public async Task<string> Authenticate(string username, string password, string nonce, string clientId)
        {
            var signInResult = await _signInManager.PasswordSignInAsync(username, password, isPersistent: false, lockoutOnFailure: false);

            // return null if user not found
            if (!signInResult.Succeeded)
                return null;

            var user = await _userManager.FindByNameAsync(username);
            var userClaims = await _userManager.GetClaimsAsync(user);

            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_identitySettings.Key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.UserName.ToString()),
                    new Claim("nonce", nonce),
                    // Audience
                    new Claim("aud", clientId),
                    // UserName
                    new Claim("upn", user.UserName),
                    new Claim("email", user.Email),
                    userClaims.FirstOrDefault(x => x.Type == "given_name"),
                    userClaims.FirstOrDefault(x => x.Type == "family_name"),
                    new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier", user.Id)
                }),
                Expires = DateTime.UtcNow.AddMinutes(60),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
