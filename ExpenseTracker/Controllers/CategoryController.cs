using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Data;
using ExpenseTracker.Models;

namespace ExpenseTracker.Controllers
{
    [RequireLogin]
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
     
        public async Task<IActionResult> Index()
        {
            return View(await _context.Categories.ToListAsync());
        }


        // GET: Category/AddOrEdit
        public IActionResult AddOrEdit(int id = 0)
        {
            if(id == 0)
            {
                return View(new Category());
            }
            else
            {
                return View(_context.Categories.Find(id));
            }
           
        }

        // POST: Category/AddOrEdit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AddOrEdit([Bind("CategoryId,Title,Icon,Type")] Category category)
        {
            var userNickname = HttpContext.Session.GetString("Nickname");
            var userId = _context.User.Where(u => u.Nickname == userNickname).Select(u => u.Id).FirstOrDefault();
            if (ModelState.IsValid)
            {
                category.UserId = userId;
                if(category.CategoryId == 0)
                {
                    _context.Add(category);
                }
                else
                {
                    _context.Update(category);
                }
               
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

     
      
        // POST: Category/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category != null)
            {
                _context.Categories.Remove(category);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

       
    }
}
