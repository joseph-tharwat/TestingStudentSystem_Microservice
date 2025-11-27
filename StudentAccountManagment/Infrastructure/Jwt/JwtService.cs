using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using StudentAccountManagment.Infrastructure.Database;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace StudentAccountManagment.Infrastructure.Jwt
{
    public class JwtService
    {
        private readonly IOptions<JwtOptions> jwtOptions;
        private readonly UserManager<ApplicationUser> userManager;

        public JwtService(IOptions<JwtOptions> jwtOptions, UserManager<ApplicationUser> userManager)
        {
            this.jwtOptions = jwtOptions;
            this.userManager = userManager;
        }

        public async Task<string> GenerateToken(ApplicationUser user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Sid, user.Id),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            var roles = await userManager.GetRolesAsync(user);
            claims.Add(new Claim(ClaimTypes.Role, roles[0]));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.Value.Key));
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: jwtOptions.Value.Issuer,
                audience: jwtOptions.Value.Audiance,
                claims: claims,
                signingCredentials: cred,
                expires: DateTime.Now.AddHours(100));

            var jwtToken = (new JwtSecurityTokenHandler()).WriteToken(token);
            return jwtToken;
        }
    }
}
