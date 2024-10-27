using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace mvc_app.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        public UserController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }
        [HttpGet]

        public IActionResult Register()
        {
            return View();
        }
		[HttpGet]
         public ViewResult CreateRole()
        {
            return View();
        }
		[HttpPost]
        public async Task<IActionResult> Register(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Password or email is invalid");
            }
            var user = new IdentityUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };
            var result = await _userManager.CreateAsync(user, password);
            if (result.Succeeded)
            {
                return Ok("User registred");
            }
            foreach (var item in result.Errors)
            {
                Console.WriteLine(item);
            }
            return BadRequest(Json(result.Errors));
        }
        [HttpGet]

        public IActionResult Auth()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Auth(string email, string password)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                return BadRequest("Password or email is nessesary!");
            }
           var result=await _signInManager.PasswordSignInAsync(
               email,
               password,
               isPersistent:false,
               lockoutOnFailure:false
               );
            if (result.Succeeded)
            {
                return RedirectToAction("Index", "Home");
                //return Ok("Auth OK");
            }
            return BadRequest("Password or email is invalid");
        }
        [HttpPost]
		public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }
        [HttpPost]
		public async Task<IActionResult>CreateRole(string roleName)
        {
			if (string.IsNullOrEmpty(roleName) )
			{
				return BadRequest("Role name is nessesary!");
			}
            var roleExists= await _roleManager.RoleExistsAsync(roleName);
            if (roleExists)
            {
				return BadRequest($"Role {roleName} already exusts!");
			}
            var role= new IdentityRole { Name = roleName };
            var result= await _roleManager.CreateAsync(role);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
				//return Ok("Auth OK");
			}
			return BadRequest(Json(result.Errors));
		}
        [HttpGet]
		public ViewResult AssignRole()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> AssignRole(string userId, string roleName)
		{
			if (string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(roleName))
			{
				return BadRequest("Id or role is nessesary!");
			}
            var user=await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return NotFound("User not found");
            }
			
			var roleExists = await _roleManager.RoleExistsAsync(roleName);
			if (!roleExists)
			{
				return BadRequest($"Role {roleName} already exusts!");
			}
            var result=await _userManager.AddToRoleAsync(user, roleName);
			if (result.Succeeded)
			{
				return RedirectToAction("Index", "Home");
				
			}
            return BadRequest(Json(result.Errors));
		}

	}
}
