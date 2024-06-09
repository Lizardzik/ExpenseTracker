using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
