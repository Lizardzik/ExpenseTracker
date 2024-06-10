using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.Diagnostics;

namespace ExpenseTracker.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
       
        public HomeController(ApplicationDbContext context)
        {
            _context = context;
        }
        public IActionResult Index(bool isChecked = false, bool? isRegistered = false)
        {
            ViewBag.isChecked = isChecked;
            ViewBag.isRegistered = isRegistered;
            return View(new User());
        }


        // POST: Home/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(User user, IFormFile profileImage)
        {
            ModelState.Remove("ProfileImage");
            ModelState.Remove("NicknameLogin");
            ModelState.Remove("PasswordLogin");
            if (ModelState.IsValid)
            {
                if (profileImage != null && profileImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await profileImage.CopyToAsync(memoryStream);
                        user.ProfileImage = memoryStream.ToArray();
                    }
                }
                else
                {
                    user.ProfileImage = null;
                }

                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index", new { isChecked = true, isRegistered = true });
            }
 
            return View("Index", user);
        }

        // POST: Home/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(string nicknameLogin, string passwordLogin)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Nickname == nicknameLogin);

            if (user != null && user.Password == passwordLogin)
            {
                HttpContext.Session.SetString("Nickname", user.Nickname);
                HttpContext.Session.SetInt32("UserId", user.Id);
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.LoginError = "Invalid username or password!";

            var emptyUser = new User();
            ViewBag.IsChecked = true;
            return View("Index", emptyUser);
        }

        // GET: Home/Logout
        public IActionResult Logout()
        {       
            HttpContext.Session.Clear();
            return RedirectToAction("Index");
        }

        // GET: Home/User
        public async Task<IActionResult> User()
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var user = await _context.User.FindAsync(userId);
            ViewData["isUserPage"] = true;
            return View(user);
        }

        // POST: Home/User
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> User(User user, IFormFile profileImage)
        {
            var userId = HttpContext.Session.GetInt32("UserId");
            var currentUser = await _context.User.FindAsync(userId);

            ModelState.Remove("ProfileImage");
            ModelState.Remove("NewPassword");
            ModelState.Remove("NicknameLogin");
            ModelState.Remove("PasswordLogin");

            //if the password valid perform the rest
            if (!currentUser.Password.Equals(user.Password))
            {
                ViewBag.PasswordError = "Please provide a correct password to save changes.";
            }
            else
            {
                // Update the password if newPassword is provided
                if (!string.IsNullOrEmpty(user.NewPassword))
                {
                    currentUser.Password = user.NewPassword;
                }

                // Convert and update profile image
                if (profileImage != null && profileImage.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await profileImage.CopyToAsync(memoryStream);
                        currentUser.ProfileImage = memoryStream.ToArray();
                    }
                }
                //update the rest
                currentUser.FirstName = user.FirstName;
                currentUser.LastName = user.LastName;
                currentUser.Nickname = user.Nickname;
            }

            if (ModelState.IsValid)
            {
                HttpContext.Session.SetString("Nickname", user.Nickname);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "User has been updated successfully.";
                return RedirectToAction("Index", "Dashboard");
            }

            ViewData["isUserPage"] = true;
            return View(currentUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
