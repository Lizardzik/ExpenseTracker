using ExpenseTracker.Data;
using ExpenseTracker.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using System.Transactions;

namespace ExpenseTracker.Controllers
{
    [RequireLogin]
    public class DashboardController : Controller
    {
        private readonly ApplicationDbContext _context;

        public DashboardController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<ActionResult> Index()
        {
            //DATA
            var userNickname = HttpContext.Session.GetString("Nickname");
            var userId = _context.User.Where(u => u.Nickname == userNickname).Select(u => u.Id).FirstOrDefault();
            DateTime StartDate = DateTime.Today.AddDays(-7);
            DateTime EndDate = DateTime.Today;
            CultureInfo culture = CultureInfo.CreateSpecificCulture("pl-PL");
            culture.NumberFormat.CurrencyNegativePattern = 1;

            List<Models.Transaction> transactions = await _context.Transactions.Where(t => t.UserId == userId).Include(x => x.Category).ToListAsync();

            //Income
            int Income = transactions.Where(i => i.Category.Type == "Income").Sum(j => j.Amount);
            ViewBag.Income = Income.ToString("C0");

            //Expense
            int Expense = transactions.Where(i => i.Category.Type == "Expense").Sum(j => j.Amount);
            ViewBag.Expense = Expense.ToString("C0");

            //Balance
            int Balance = Income - Expense;        
            ViewBag.Balance = String.Format(culture, "{0:C0}", Balance);

            //Chars - Expenses
            ViewBag.CharData = transactions.Where(i => i.Category.Type == "Expense").GroupBy(j => j.Category.CategoryId).Select(k => new
            {
                categoryTitleWithIcon = k.First().Category.Icon+ " " + k.First().Category.Title,
                amount = k.Sum(j => j.Amount),
                formattedAmount = k.Sum(j => j.Amount).ToString("C0"),
            }).OrderByDescending(l => l.amount).ToList();

            //Chars - Income VS Expense

            //Income
            List<ChartData> incomeData = transactions.Where(i => i.Category.Type =="Income").GroupBy(j => j.Date).Select(k => new ChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                income = k.Sum(l => l.Amount)
            }).ToList();

            //Expense
            List<ChartData> expenseData = transactions.Where(i => i.Category.Type =="Expense").GroupBy(j => j.Date).Select(k => new ChartData()
            {
                day = k.First().Date.ToString("dd-MMM"),
                expense = k.Sum(l => l.Amount)
            }).ToList();

            //Combine
            string[] lastWeek = Enumerable.Range(0, 7).Select(i => StartDate.AddDays(i).ToString("dd-MMM")).ToArray();
            ViewBag.ChartData = from day in lastWeek
                                join income in incomeData on day equals income.day into dayIncome
                                from income in dayIncome.DefaultIfEmpty()
                                join expense in expenseData on day equals expense.day into dayExpense
                                from expense in dayExpense.DefaultIfEmpty()
                                select new
                                {
                                    day = day,
                                    income = income == null ? 0 : income.income,
                                    expense = expense == null ? 0 : expense.expense,
                                };

            //Recent Transactions
            ViewBag.RecentTransactions = await _context.Transactions.Include(i => i.Category).OrderByDescending(j => j.Date).Take(5).ToListAsync();

            //User Actions
            User user = await _context.User.FirstOrDefaultAsync();

            ViewBag.RecentUsers = await _context.User.OrderByDescending(i => i.Id).Take(3).ToListAsync();

         
            var userImage = _context.User.FirstOrDefault(u => u.Nickname == userNickname)?.DisplayImage;
            HttpContext.Session.SetString("UserImage", userImage ?? "");

            return View(user);
        }
    }

    public class ChartData
    {
        public string day;
        public int income;
        public int expense;
    }
}
