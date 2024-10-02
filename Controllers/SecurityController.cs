using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureChild.Areas.Identity.Data;
using SecureChild.Data;
using SecureChild.Models;
using System.Web;

namespace SecureChild.Controllers
{
    public class SecurityController : Controller
    {

        private readonly SecureChildContext _context;
        private UserManager<SecureChildUser> _userManager;
        Messaging messaging = new Messaging();

        public SecurityController(SecureChildContext context , UserManager<SecureChildUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // Profile page for current logged-in user
        public async Task<IActionResult> Profile()
        {
            var secureChildUser = await _userManager.GetUserAsync(User);

            var security = await _context.SecurityStaff
               .FirstOrDefaultAsync(e => e.Email == User.Identity.Name);

            if (secureChildUser == null)
            {
                return NotFound("User not found.");
            }

            // Map SecureChildUser to SecurityStaff
            var securityStaff = new SecurityStaff
            {
                Email = secureChildUser.Email,
                Name = secureChildUser.UserName, // Or any field that represents the name
                ContactNumber = security.ContactNumber,
                Gate = security.Gate
                // Add other mappings if necessary
            };

            return View(securityStaff);
        }



        // GET: Entries
        public async Task<IActionResult> ViewEntries()
        {
            var secureChildContext = _context.Entries.Include(e => e.Student);
            return View(await secureChildContext.ToListAsync());
        }

        public async Task<IActionResult> ViewExits()
        {
            var secureChildContext = _context.Exits.Include(e => e.Student);
            return View(await secureChildContext.ToListAsync());
        }

        public async Task<IActionResult> EntriesandExit()
        {
            return View(await _context.Students.ToListAsync());
        }

        // GET: Entries/Create
        public IActionResult Entries(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Entries([Bind("StudentId,EntryGate")] Entry entry)
        {
            string username = User.Identity.Name;
            var security = _context.SecurityStaff.FirstOrDefault(s => s.Email == username || s.Name == username);

            // Check if an entry already exists for the student on the current date
            var existingEntry = await _context.Entries
                .FirstOrDefaultAsync(e => e.StudentId == entry.StudentId
                    && e.Time.Date == DateTime.Now.Date);

            if (existingEntry != null)
            {
                ModelState.AddModelError("", "An entry for this student already exists today.");
                return View(entry);
            }

            entry.Time = DateTime.Now;
            entry.EntryGate = security.Gate;
            _context.Add(entry);

            var student = await _context.Students.Include(s => s.Parent).FirstOrDefaultAsync(s => s.Id == entry.StudentId);

            messaging.sendWhatsappMessage(student.Parent.ContactInfo, "Good Morning , You child " + student.Name + " has just entered the school premises", student.Parent.apiKey);

            /* Check for lateness
            var schoolTime = await _context.SchoolTime.FirstOrDefaultAsync(s => s.Date == DateTime.Today);
            if (schoolTime != null && entry.Time.TimeOfDay > schoolTime.SchoolStartingTime)
            {

                messaging.sendWhatsappMessage(student.Parent.ContactInfo, $"{student.Name} was late for school today.", student.Parent.apiKey);
            }
            else
            {
                messaging.sendWhatsappMessage(student.Parent.ContactInfo, "Good Morning , You child " + student.Name + " has just entered the school premises",  student.Parent.apiKey);
            }
            */

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EntriesandExit));
        }

        // GET: Entries/Create
        public IActionResult Exit(int id)
        {
            ViewBag.Id = id;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Exit([Bind("StudentId,ExitGate")] Exit exit)
        {
            string username = User.Identity.Name;
            var security = _context.SecurityStaff.FirstOrDefault(s => s.Email == username || s.Name == username);

            // Check if an entry already exists for the student on the current date
            var existingEntry = await _context.Exits
                .FirstOrDefaultAsync(e => e.StudentId == exit.StudentId
                    && e.Time.Date == DateTime.Now.Date);

            if (existingEntry != null)
            {
                ModelState.AddModelError("", "An exit for this student already exists today.");
                return View(exit);
            }

            exit.Time = DateTime.Now;
            exit.ExitGate = security.Gate;
            _context.Add(exit);

            // Check for early leaving
            var schoolTime = await _context.SchoolTime.FirstOrDefaultAsync(s => s.Date == DateTime.Today);
            if (schoolTime != null && exit.Time.TimeOfDay < schoolTime.SchoolEndingTime)
            {
                var student = await _context.Students.Include(s => s.Parent).FirstOrDefaultAsync(s => s.Id == exit.StudentId);
                messaging.sendWhatsappMessage(student.Parent.ContactInfo, $"{student.Name} left the school premises early.", student.Parent.apiKey);
            }
            else
            {
                var student = await _context.Students.Include(s => s.Parent).FirstOrDefaultAsync(s => s.Id == exit.StudentId);
                messaging.sendWhatsappMessage(student.Parent.ContactInfo, "Good Evening , You child " + student.Name + " has just exited the school premises", student.Parent.apiKey);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(EntriesandExit));
        }


      

    }
}
