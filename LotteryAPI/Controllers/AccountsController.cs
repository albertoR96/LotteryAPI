using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using LotteryAPI.DTOs;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;

namespace LotteryAPI.Controllers
{
    [ApiController]
    [Route("accounts")]
    public class AccountsController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public AccountsController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("register")]
        public async Task<ActionResult<AuthResponse>> Register(UserCredentials credentials)
        {
            var user = new IdentityUser { UserName = credentials.Email, Email = credentials.Email };
            var result = await userManager.CreateAsync(user, credentials.Password);
            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest(result.Errors);
            }
        }

        private async Task<AuthResponse> BuildToken(UserCredentials credentials)
        {
            var claims = new List<Claim>
            {
                new Claim("email", credentials.Email)
            };

            var user = await userManager.FindByEmailAsync(credentials.Email);
            var claimsDB = await userManager.GetClaimsAsync(user);
            claims.AddRange(claimsDB);

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["keyjwt"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddMinutes(60);
            var securityToken = new JwtSecurityToken(issuer: null, audience: null, claims: claims, expires: expiration, signingCredentials: creds);
            return new AuthResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(securityToken),
                Expiration = expiration
            };
        }

        [HttpPost("login")]
        public async Task<ActionResult<AuthResponse>> Login(UserCredentials credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, isPersistent: false, lockoutOnFailure: false);
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return BadRequest("User not fount");
            }

            await userManager.AddClaimAsync(user, new Claim("ItsParticipant", "1"));
            try
            {
                await userManager.RemoveClaimAsync(user, new Claim("ItsAdmin", "1"));
            } catch (Exception exc)
            {
                //
            }

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Bad login");
            }
        }

        [HttpPost("loginAsAdmin")]
        public async Task<ActionResult<AuthResponse>> LoginAsAdmin(UserCredentials credentials)
        {
            var result = await signInManager.PasswordSignInAsync(credentials.Email, credentials.Password, isPersistent: false, lockoutOnFailure: false);
            var user = await userManager.FindByEmailAsync(credentials.Email);
            if (user == null)
            {
                return BadRequest("User not fount");
            }

            await userManager.AddClaimAsync(user, new Claim("ItsAdmin", "1"));

            if (result.Succeeded)
            {
                return await BuildToken(credentials);
            }
            else
            {
                return BadRequest("Bad login");
            }
        }
    }
}
