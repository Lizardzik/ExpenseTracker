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
        public async Task<IActionResult> Login(string nickname, string password)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.Nickname == nickname);

            if (user != null && user.Password == password)
            {
                HttpContext.Session.SetString("Nickname", user.Nickname);
                return RedirectToAction("Index", "Dashboard");
            }

            ViewBag.LoginError = "Nieprawid³owa nazwa u¿ytkownika lub has³o.";

            var emptyUser = new User();
            ViewBag.IsChecked = true; 
            return View("Index", emptyUser);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
