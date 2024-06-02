using ExpenseTracker.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;

namespace ExpenseTracker.Controllers
{
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //Stats from a month
            DateTime StartDate = DateTime.Today.AddDays(-30);
            DateTime EndDate = DateTime.Today;

            List<Models.Transaction> transactions = await _context.Transactions.Include(x => x.Category).Where(y => y.Date >= StartDate && y.Date <= EndDate).ToListAsync();

            //Income
            int Income = transactions.Where(i => i.Category.Type == "Income").Sum(j => j.Amount);
            ViewBag.Income = Income.ToString("C0");

            //Expense
            int Expense = transactions.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);
            ViewBag.Expense = Expense.ToString("C0");

            //Balance
            int Balance = Income - Expense;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pl-PL");
            culture.NumberFormat.CurrencyNegativePattern = 1;
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

            //Chars - Expenses
            decimal totalExpenses = transactions.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);
            ViewBag.CharData = transactions.Where(i => i.Category.Type == "Expense").GroupBy(j => j.Category.CategoryId).Select(k => new
            {
                categoryTitleWithIcon = k.First().Category.Icon+ " " + k.First().Category.Title,
                amount = k.Sum(j => j.Amount),
                formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
            }).ToList();

            return View();
        }
    }
}
