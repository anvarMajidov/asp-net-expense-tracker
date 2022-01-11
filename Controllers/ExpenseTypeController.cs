using InAndOut.Data;
using InAndOut.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class ExpenseTypeController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpenseTypeController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<ExpenseType> expenses = _db.ExpenseTypes;
            return View(expenses);
        }

        public IActionResult Create()
        {
            return View();
        }

        public IActionResult Edit(int id)
        {
            var obj = _db.ExpenseTypes.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        #region "Api calls"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(ExpenseType expense)
        {
            if (!ModelState.IsValid)
            {
                return View(expense);
            }
            _db.ExpenseTypes.Add(expense);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _db.ExpenseTypes.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _db.ExpenseTypes.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ExpenseType expense)
        {
            var expenseTypeInDatabase = _db.ExpenseTypes.Find(expense.Id);
            if(expenseTypeInDatabase == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return View(expense);
            }
            expenseTypeInDatabase.Name = expense.Name;
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
