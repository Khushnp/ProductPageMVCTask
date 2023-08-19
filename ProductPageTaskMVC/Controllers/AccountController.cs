using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductPageTaskMVC.Models;

namespace ProductPageTaskMVC.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly IConfiguration _configuration;
        public AccountController(IConfiguration configuration)
        {
            _configuration= configuration;
        }
        public IActionResult Index()
        {
            return View();
        }
        
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(AccountLoginModel viewModel)
        {
            try
            {
                if (viewModel==null || !ModelState.IsValid) return View("Index", viewModel);

                string encryptedPwd = viewModel.Password;
                var userPassword = Convert.ToString("admin");
                var Username = Convert.ToString("Admin");
                if((viewModel.Password !=null ? encryptedPwd.Equals(userPassword):false) && (viewModel.username != null ? viewModel.username.Equals(Username):false))
                {
                    var roles = new string[] { "SuperAdmin", "Admin" };
                    var jwtSecurityToken = Authentication.GenerateJetToken(Username, roles.ToList());
                    //Session["LoginedIn"] = Username;
                    var validUserName = Authentication.ValidateToken(jwtSecurityToken);
                    return RedirectToAction("index", "Home", new { token = jwtSecurityToken });//thid is where specific index of homecontroller is called 
                }

                ModelState.AddModelError("", "Invalid username or password.");
            }
            catch(Exception ex)
            {
                ModelState.AddModelError("", "Invalid username or password.");
            }
            return View("Index",viewModel);
        }
    }
}
