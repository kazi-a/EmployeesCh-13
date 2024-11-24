using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using EmployeesCh12.Models;

namespace EmployeesCh12.Controllers
{
    public class DepartmentLocationsController : Controller
    {
        private readonly EmployeeContext _context;

        public DepartmentLocationsController(EmployeeContext context)
        {
            _context = context;
        }

        // GET: DepartmentLocations
        public async Task<IActionResult> Index()
        {
            var employeeContext = _context.DepartmentLocations.Include(d => d.Department).Include(d => d.Location);
            return View(await employeeContext.ToListAsync());
        }

        // GET: DepartmentLocations/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (departmentLocation == null)
            {
                return NotFound();
            }

            return View(departmentLocation);
        }

        // GET: DepartmentLocations/Create
        public IActionResult Create()
        {
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID");
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "ID");
            return View();
        }

        // POST: DepartmentLocations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("DepartmentID,LocationID")] DepartmentLocation departmentLocation)
        {
            if (ModelState.IsValid)
            {
                _context.Add(departmentLocation);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", departmentLocation.DepartmentID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "ID", departmentLocation.LocationID);
            return View(departmentLocation);
        }

        // GET: DepartmentLocations/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentLocation = await _context.DepartmentLocations.FindAsync(id);
            if (departmentLocation == null)
            {
                return NotFound();
            }
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", departmentLocation.DepartmentID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "ID", departmentLocation.LocationID);
            return View(departmentLocation);
        }

        // POST: DepartmentLocations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("DepartmentID,LocationID")] DepartmentLocation departmentLocation)
        {
            if (id != departmentLocation.DepartmentID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(departmentLocation);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DepartmentLocationExists(departmentLocation.DepartmentID))
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
            ViewData["DepartmentID"] = new SelectList(_context.Departments, "ID", "ID", departmentLocation.DepartmentID);
            ViewData["LocationID"] = new SelectList(_context.Locations, "ID", "ID", departmentLocation.LocationID);
            return View(departmentLocation);
        }

        // GET: DepartmentLocations/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var departmentLocation = await _context.DepartmentLocations
                .Include(d => d.Department)
                .Include(d => d.Location)
                .FirstOrDefaultAsync(m => m.DepartmentID == id);
            if (departmentLocation == null)
            {
                return NotFound();
            }

            return View(departmentLocation);
        }

        // POST: DepartmentLocations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var departmentLocation = await _context.DepartmentLocations.FindAsync(id);
            if (departmentLocation != null)
            {
                _context.DepartmentLocations.Remove(departmentLocation);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DepartmentLocationExists(int id)
        {
            return _context.DepartmentLocations.Any(e => e.DepartmentID == id);
        }
    }
}
