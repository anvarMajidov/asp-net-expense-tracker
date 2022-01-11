using InAndOut.Data;
using InAndOut.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InAndOut.Controllers
{
    public class ExpenseController : Controller
    {
        private readonly ApplicationDbContext _db;

        public ExpenseController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            IEnumerable<Expense> expenses = _db.Expenses;
            IEnumerable<ExpenseType> expenseTypes = _db.ExpenseTypes;
            
            var expenseDict = new Dictionary<int, string>();
            foreach(var et in expenseTypes)
            {
                if (!expenseDict.ContainsKey(et.Id))
                {
                    expenseDict[et.Id] = et.Name;
                }
            }
            ViewBag.expenseDict = expenseDict;
            
            return View(expenses);
        }

        public IActionResult Create()
        {
            ViewBag.DropDown = _db.ExpenseTypes.Select(i => new SelectListItem
            {
                Text = i.Name,
                Value = i.Id.ToString()
            });
            return View();
        }

        public IActionResult Edit(int id)
        {
            var expense = _db.Expenses.Find(id);
            if (expense == null)
            {
                return NotFound();
            }
            ViewBag.DropDown = _db.ExpenseTypes.Select(i => new SelectListItem
            {
                Text = i.Name,
                Selected = i.Id == expense.ExpenseTypeId,
                Value = i.Id.ToString()
            });
            return View(expense);
        }

        #region "Api calls"
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Expense expense)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.DropDown = _db.ExpenseTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Selected = i.Id == expense.ExpenseTypeId,
                    Value = i.Id.ToString()
                });
                return View(expense);
            }
            _db.Expenses.Add(expense);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id)
        {
            var obj = _db.Expenses.Find(id);
            if(obj == null)
            {
                return NotFound();
            }
            _db.Expenses.Remove(obj);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Expense expense)
        {
            var expenseInDatabase = _db.Expenses.Find(expense.Id);

            if(expenseInDatabase == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewBag.DropDown = _db.ExpenseTypes.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Selected = i.Id == expense.ExpenseTypeId,
                    Value = i.Id.ToString()
                });
                return View(expense);
            }

            expenseInDatabase.Amount = expense.Amount;
            expenseInDatabase.Name = expense.Name;
            expenseInDatabase.ExpenseTypeId = expense.ExpenseTypeId;

            _db.SaveChanges();
            return RedirectToAction("Index");
        }
        #endregion
    }
}
