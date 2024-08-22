using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using dhub.Users;
using dhub.Models;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace dhub.Controllers
{
    [Route("account")]
    
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<AccountController> _logger;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILogger<AccountController> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = logger;
        }

        [HttpGet("register")]
        [AllowAnonymous]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("Registering a new user.");
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            IdentityResult result = await _userManager.CreateAsync(user, model.Password); 


            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Owner");
                await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation("Successfully registered a new user.");
                return RedirectToAction("Login", "Account");
            }

            foreach (var error in result.Errors)
            {
                _logger.LogError("Error: {Error}", error.Description);
                ModelState.AddModelError(string.Empty, error.Description);
            }

            return View(model);
        }

        [HttpGet("login")]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            _logger.LogInformation("User attempting to log in.");

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                var roles = await _userManager.GetRolesAsync(user);
                _logger.LogInformation("User successfully logged in.");
                _logger.LogInformation("User roles after login: {Roles}", string.Join(",", roles));
                return RedirectToAction("Index", "Survey");
            }

            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            return View(model);
        }

        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
           
            await _signInManager.SignOutAsync();
            _logger.LogInformation("User logged out.");
            return RedirectToAction("Index", "Home");
        }


        [HttpGet("accessdenied")]
        public IActionResult AccessDenied()
        {
            var userId = _userManager.GetUserId(User);
            var user = _userManager.FindByIdAsync(userId).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            _logger.LogInformation("User roles on Access Denied page: {Roles}", string.Join(", ", roles));

            return View();
        }
    }
}
