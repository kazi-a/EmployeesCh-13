using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesCh12.Models;
using EmployeesCh12.ViewModel;


namespace EmployeesCh12.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly EmployeeContext _context;

        public EmployeesController(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string sortOrder, string currentFilter, string searchString, int? pageNumber)
        {
            // Preserve the current sort order
            ViewData["CurrentSort"] = sortOrder;
            ViewData["NameSortParm"] = String.IsNullOrEmpty(sortOrder) ? "name_desc" : "";
            ViewData["DateSortParm"] = sortOrder == "Date" ? "date_desc" : "Date";

            // Preserve the current filter for search string
            if (searchString != null)
            {
                pageNumber = 1;  // Reset to the first page when a new search is initiated
            }
            else
            {
                searchString = currentFilter;
            }
            ViewData["CurrentFilter"] = searchString;

            // Get the base query for employees, including department and benefits
            var employees = from e in _context.Employees
                            .Include(e => e.Department)
                            .Include(e => e.Benefits)
                            select e;

            // Apply the search filter if a search string is provided
            if (!String.IsNullOrEmpty(searchString))
            {
                employees = employees.Where(s => s.LastName.Contains(searchString) || s.FirstName.Contains(searchString));
            }

            // Apply sorting based on the sort order
            switch (sortOrder)
            {
                case "name_desc":
                    employees = employees.OrderByDescending(s => s.LastName);
                    break;
                case "Date":
                    employees = employees.OrderBy(s => s.HireDate);
                    break;
                case "date_desc":
                    employees = employees.OrderByDescending(s => s.HireDate);
                    break;
                default:
                    employees = employees.OrderBy(s => s.LastName);
                    break;
            }

            // Set the page size for pagination
            int pageSize = 3;

            // Create the paginated list asynchronously
            return View(await PaginatedList<Employee>.CreateAsync(
                employees.AsNoTracking(),
                pageNumber ?? 2,
                pageSize)
            );
        }

        public IActionResult DeptCount()
        {
            IQueryable<DepartmentGroup> data =
                from employee in _context.Employees.Include(e => e.Department)
                group employee by employee.DepartmentID into deptGroup
                select new DepartmentGroup()
                {
                    DepartmentID = (int)deptGroup.Key,
                    DepartmentCount = deptGroup.Count()
                };

            return View(data.ToList());
        }


        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Benefits)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {
            ViewData["BenefitsID"] = new SelectList(_context.Benefits, "ID", "ID");
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,FirstName,LastName,HireDate,DepartmentID,BenefitsID")] Employee employee)
        {
            if (ModelState.IsValid)
            {
                _context.Add(employee);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["BenefitsID"] = new SelectList(_context.Benefits, "ID", "ID", employee.BenefitsID);
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            ViewData["BenefitsID"] = new SelectList(_context.Benefits, "ID", "ID", employee.BenefitsID);
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", employee.DepartmentID);
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,FirstName,LastName,HireDate,DepartmentID,BenefitsID")] Employee employee)
        {
            if (id != employee.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["BenefitsID"] = new SelectList(_context.Benefits, "ID", "ID", employee.BenefitsID);
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", employee.DepartmentID);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Benefits)
                .Include(e => e.Department)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee != null)
            {
                _context.Employees.Remove(employee);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.ID == id);
        }
    }
}
