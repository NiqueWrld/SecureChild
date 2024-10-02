using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureChild.Data;
using SecureChild.Models;

namespace SecureChild.Controllers
{
    public class SecurityStaffs1Controller : Controller
    {
        private readonly SecureChildContext _context;

        public SecurityStaffs1Controller(SecureChildContext context)
        {
            _context = context;
        }

        // GET: SecurityStaffs1
        public async Task<IActionResult> Index()
        {
            return View(await _context.SecurityStaff.ToListAsync());
        }

        // GET: SecurityStaffs1/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityStaff = await _context.SecurityStaff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityStaff == null)
            {
                return NotFound();
            }

            return View(securityStaff);
        }

        // GET: SecurityStaffs1/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SecurityStaffs1/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Email,Name,Gate,ContactNumber,Password")] SecurityStaff securityStaff)
        {
            if (ModelState.IsValid)
            {
                _context.Add(securityStaff);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(securityStaff);
        }

        // GET: SecurityStaffs1/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityStaff = await _context.SecurityStaff.FindAsync(id);
            if (securityStaff == null)
            {
                return NotFound();
            }
            return View(securityStaff);
        }

        // POST: SecurityStaffs1/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Email,Name,Gate,ContactNumber,Password")] SecurityStaff securityStaff)
        {
            if (id != securityStaff.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(securityStaff);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SecurityStaffExists(securityStaff.Id))
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
            return View(securityStaff);
        }

        // GET: SecurityStaffs1/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var securityStaff = await _context.SecurityStaff
                .FirstOrDefaultAsync(m => m.Id == id);
            if (securityStaff == null)
            {
                return NotFound();
            }

            return View(securityStaff);
        }

        // POST: SecurityStaffs1/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var securityStaff = await _context.SecurityStaff.FindAsync(id);
            if (securityStaff != null)
            {
                _context.SecurityStaff.Remove(securityStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SecurityStaffExists(int id)
        {
            return _context.SecurityStaff.Any(e => e.Id == id);
        }
    }
}
