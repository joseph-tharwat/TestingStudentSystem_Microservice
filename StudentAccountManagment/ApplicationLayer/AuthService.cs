using Microsoft.AspNetCore.Identity;
using StudentAccountManagment.Infrastructure.Database;
using StudentAccountManagment.Infrastructure.Jwt;
using StudentAccountManagment.Shared;

namespace StudentAccountManagment.ApplicationLayer
{
    public class AuthService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly JwtService jwtService;

        public AuthService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, JwtService jwtService) 
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.jwtService = jwtService;
        }

        public async Task<string> Login(LoginUser loginUser)
        {
            var user = await userManager.FindByEmailAsync(loginUser.Email);
            if (user == null)
            {
                throw new Exception("Invalid credentials");
            }

            //var result = await signInManager.PasswordSignInAsync(user, loginUser.Password, false, false);
            var result = await signInManager.CheckPasswordSignInAsync(user, loginUser.Password, false);
            if (!result.Succeeded)
            {
                throw new Exception("Invalid credentials");
            }
            return await jwtService.GenerateToken(user);
        }

        public async Task LogOut()
        {
            //await signInManager.SignOutAsync();
            //need to put in blacklist 
        }

        public async Task<string> RegisterTeacher(RegisterUser registerUser)
        {
            var existedUser = await userManager.FindByEmailAsync(registerUser.Email);
            if (existedUser != null)
            {
                throw new Exception("Email already existed.");
            }

            var user = new ApplicationUser() 
            {
                UserName= registerUser.Name,
                Email= registerUser.Email
            };

            var result = await userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(",",result.Errors.Select(e=>e.Description)));
            }

            await userManager.AddToRoleAsync(user, "Teacher");

            return await jwtService.GenerateToken(user);
        }

        public async Task<string> RegisterStudent(RegisterUser registerUser)
        {
            var existedUser = await userManager.FindByEmailAsync(registerUser.Email);
            if (existedUser != null)
            {
                throw new Exception("Email already existed.");
            }

            var user = new ApplicationUser()
            {
                UserName = registerUser.Name,
                Email = registerUser.Email
            };

            var result = await userManager.CreateAsync(user, registerUser.Password);
            if (!result.Succeeded)
            {
                throw new Exception(string.Join(",", result.Errors.Select(e => e.Description)));
            }

            await userManager.AddToRoleAsync(user, "Student"); 

            return await jwtService.GenerateToken(user);
        }

    }
}
