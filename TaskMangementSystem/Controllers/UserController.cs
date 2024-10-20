using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Threading.Tasks;
using TaskMangementSystem.Models;
using TaskMangementSystem.Repositories;

namespace TaskMangementSystem.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;

        public UserController(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        // GET: Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(UserModel userModel)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = await _userRepository.GetUserByEmailAsync(userModel.Email);
                    if (user == null)
                    {
                        Console.WriteLine("User saved successfully.");
                        await _userRepository.AddUserAsync(userModel);
                        return RedirectToAction("Login");
                    }
                    ModelState.AddModelError(string.Empty, "User already exist! Please login...");
                    return View(user);
                }
                return View();
            } catch(Exception e)
            {
                ModelState.AddModelError(string.Empty, "Internal Error !!!");
                return View();
            }
        }

        // GET: Account/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string email, string password)
        {
            if (ModelState.IsValid)
            {
                var user = await _userRepository.GetUserByEmailAsync(email);
                if (user != null)
                {
                    if (user.Password == password)
                    {
                        HttpContext.Session.SetString("UserID", user.UserID.ToString());
                        Console.WriteLine("Session UserID Set: " + HttpContext.Session.GetString("UserID"));
                        return RedirectToAction("Index", "Home");
                    }
                    ModelState.AddModelError(string.Empty, "Incorrect password");
                    return View();
                }
                ModelState.AddModelError(string.Empty, "User not found !");
                return View();
            }
            return View();
        }

        // Logout action
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
