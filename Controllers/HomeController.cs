using Employees_Test.Models;
using Employees_Test.Views.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;   

using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using System.IO;
using PagedList.Mvc;
using PagedList;
using System.Text;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Xml.Linq;
using System.Dynamic;


namespace Employees.Controllers
{
    public class HomeController : Controller
    {
        private ApplicationContext db;
        public HomeController(ApplicationContext context)
        {
            db = context;
        }

        public IActionResult Index(int? page, string SearchString)
        {
            IEnumerable<Employee> employees = db.Employees_Test.ToList();
            IEnumerable<Employee> allEmployees = db.Employees_Test.ToList();

            if (!String.IsNullOrEmpty(SearchString))
            {
                employees = employees.Where(p => p.FIO.Contains(SearchString) || p.Phone.Contains(SearchString));
            }

            int pageSize = 3;
            int pageNumber = (page ?? 1);

            dynamic mymodel = new ExpandoObject();

            mymodel.SearchString = SearchString;
            mymodel.AllEmployees = allEmployees;
            mymodel.EmployeesPaging = employees.ToPagedList(pageNumber, pageSize);
            
            return View(mymodel);

        }
        
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create(EmployeeViewModel employeeViewModel)
        {
            Employee employee = new Employee
            {
                FIO = employeeViewModel.FIO,
                department = employeeViewModel.department,
                Phone = employeeViewModel.Phone,
                Image = $"data:image/png;base64,{employeeViewModel.Image.FileToBase64()}"
            };

            db.Employees_Test.Add(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id != null)
            {
                Employee employee = await db.Employees_Test.FirstOrDefaultAsync(p => p.Id == id);
                if (employee != null)
                    return View(employee);
            }
            return NotFound();
        }
        [HttpPost]
        public async Task<IActionResult> Edit(EmployeeViewModel employeeViewModel, string oldImg)
        {
            Employee employee = new Employee
            {
                Id = employeeViewModel.Id,
                FIO = employeeViewModel.FIO,
                department = employeeViewModel.department,
                Phone = employeeViewModel.Phone,
            };

            if (employeeViewModel.Image != null)
            {
                employee.Image = $"data:image/png;base64,{employeeViewModel.Image.FileToBase64()}";

            }
            else
            {
                employee.Image = oldImg;
            }

            db.Employees_Test.Update(employee);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int? id)
        {
            if (id != null)
            {
                Employee employee = await db.Employees_Test.FirstOrDefaultAsync(p => p.Id == id);
                if (employee != null)
                {
                    db.Employees_Test.Remove(employee);
                    await db.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
            }
            return NotFound();
        }
    }
}