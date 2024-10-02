using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureChild.Data;
using SecureChild.Models;

namespace SecureChild.Controllers
{

    public class AlertsController : Controller
    {
        private readonly SecureChildContext _context;
        Messaging messaging = new Messaging();

        public AlertsController(SecureChildContext context)
        {
            _context = context;
        }

        // GET: Alerts
        public async Task<IActionResult> Index()
        {
            var secureChildContext = _context.Alerts.Include(a => a.Student);
            return View(await secureChildContext.ToListAsync());
        }

        // GET: Alerts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alert = await _context.Alerts
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alert == null)
            {
                return NotFound();
            }

            return View(alert);
        }

        // GET: Alerts/Create
        public IActionResult Create()
        {
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Name");
            return View();
        }

        // POST: Alerts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Message,AlertType,StudentId")] Alert alert)
        {
            
            alert.Time = DateTime.Now;

            var student = await _context.Students
                .Include(a => a.Parent)
                .FirstOrDefaultAsync(m => m.Id == alert.StudentId);
                
                

            try
            {
                messaging.sendWhatsappMessage(student.Parent.ContactInfo, alert.AlertType + ":" + alert.Message, student.Parent.apiKey);
                alert.SentSuccessfully = true;
            }
            catch (Exception ex)
            {
                alert.SentSuccessfully = false;
            }

                _context.Add(alert);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            
            ViewData["StudentId"] = new SelectList(_context.Students, "Id", "Id", alert.StudentId);
            return View(alert);
        }
 
        // GET: Alerts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var alert = await _context.Alerts
                .Include(a => a.Student)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (alert == null)
            {
                return NotFound();
            }

            return View(alert);
        }

        // POST: Alerts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var alert = await _context.Alerts.FindAsync(id);
            if (alert != null)
            {
                _context.Alerts.Remove(alert);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AlertExists(int id)
        {
            return _context.Alerts.Any(e => e.Id == id);
        }
    }
}
