using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureChild.Areas.Identity.Data;
using SecureChild.Data;
using SecureChild.Models;
using System.Diagnostics;


namespace SecureChild.Controllers
{
    public class AdminController : Controller
    {
        private readonly SecureChildContext _context; // Database context
        private UserManager<SecureChildUser> _userManager;

        public AdminController(SecureChildContext context , UserManager<SecureChildUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Admin Dashboard
        public ActionResult Dashboard()
        {
            var viewModel = new AdminDashboardViewModel
            {
                StudentsCount = _context.Students.Count(),
                SecurityStaffCount = _context.SecurityStaff.Count(),
                RecentEntries = _context.Entries
                        .Include(e => e.Student)
                        .Include(e => e.Student.Parent)  
                        .OrderByDescending(e => e.Time)
                        .Take(10)
                        .ToList(),
                RecentExits = _context.Exits
                        .Include(e => e.Student)
                        .Include(e => e.Student.Parent)
                        .OrderByDescending(e => e.Time)
                        .Take(10)
                        .ToList(),
                Alerts = _context.Alerts.OrderByDescending(a => a.Time).Take(5).ToList()
            };


            return View(viewModel);
        }

        // GET: Students
        public async Task<IActionResult> ManageStudents()
        {
            var secureChildContext = _context.Students.Include(s => s.Parent);
            return View(await secureChildContext.ToListAsync());
        }

      

        // GET: Students/Details/5
        public async Task<IActionResult> DetailsStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult CreateStudent()
        {
            ViewData["ParentName"] = new SelectList(_context.Parents, "Id", "Name");
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateStudent([Bind("Id,Name,Age,ParentId")] Student student)
        {
            
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageStudents));
            
            //ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "Id", student.ParentId);
            //return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> EditStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "Id", student.ParentId);
            return View(student);
        }

        // POST: Students/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditStudent(int id, [Bind("Id,Name,Age,ParentId")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageStudents));
            
            ViewData["ParentId"] = new SelectList(_context.Parents, "Id", "Id", student.ParentId);
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> DeleteStudent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.Parent)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("DeleteStudent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedStudent(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageStudents));
        }

        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }

        // GET: Alert System Settings
        public ActionResult AlertSettings()
        {
            var alertSettings = _context.AlertSettings.SingleOrDefault();
            return View(alertSettings);
        }

        // POST: Save Alert Settings
        [HttpPost]
        public ActionResult SaveAlertSettings(AlertSettings settings)
        {
            if (!ModelState.IsValid)
            {
                return View("AlertSettings", settings);
            }

            var settingsInDb = _context.AlertSettings.Single();
            settingsInDb.SmsEnabled = settings.SmsEnabled;
            settingsInDb.EmailEnabled = settings.EmailEnabled;
            settingsInDb.EntryNotificationEnabled = settings.EntryNotificationEnabled;
            settingsInDb.ExitNotificationEnabled = settings.ExitNotificationEnabled;

            _context.SaveChanges();
            return RedirectToAction("Dashboard");
        }

        protected override void Dispose(bool disposing)
        {
            _context.Dispose();
        }

        // GET: Parents
        public async Task<IActionResult> ManageParents()
        {
            return View(await _context.Parents.ToListAsync());
        }

        // GET: Parents/Details/5
        public async Task<IActionResult> DetailsParent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // GET: Parents/Create
        public IActionResult CreateParent()
        {
            return View();
        }

        // POST: Parents/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateParent([Bind("Id,Name,ContactInfo , apiKey")] Parent parent)
        {
            if (ModelState.IsValid)
            {
                _context.Add(parent);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageParents));
            }
            return View(parent);
        }

        // GET: Parents/Edit/5
        public async Task<IActionResult> EditParent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents.FindAsync(id);
            if (parent == null)
            {
                return NotFound();
            }
            return View(parent);
        }

        // POST: Parents/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditParent(int id, [Bind("Id,Name,ContactInfo , apiKey")] Parent parent)
        {
            if (id != parent.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parent);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentExists(parent.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageParents));
            }
            return View(parent);
        }

        // GET: Parents/Delete/5
        public async Task<IActionResult> DeleteParent(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parent = await _context.Parents
                .FirstOrDefaultAsync(m => m.Id == id);
            if (parent == null)
            {
                return NotFound();
            }

            return View(parent);
        }

        // POST: Parents/Delete/5
        [HttpPost, ActionName("DeleteParent")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedParent(int id)
        {
            var parent = await _context.Parents.FindAsync(id);
            if (parent != null)
            {
                _context.Parents.Remove(parent);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageParents));
        }

        private bool ParentExists(int id)
        {
            return _context.Parents.Any(e => e.Id == id);
        }

        // GET: SchoolTimes 1
        public async Task<IActionResult> ManageSchoolTime()
        {
            var latestSchoolTime = _context.SchoolTime
                 .OrderByDescending(st => st.Date)
                 .FirstOrDefault();

            if (latestSchoolTime == null)
            {
                return NotFound();
            }

            return View(latestSchoolTime);
        }

        public IActionResult UpdateSchoolTime()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateSchoolTime([Bind("Id,SchoolStartingTime,SchoolEndingTime")] SchoolTime schoolTime)
        {
            schoolTime.Date = DateTime.Now;

            if (ModelState.IsValid)
            {
                _context.Add(schoolTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageSchoolTime));
            }
            return View(schoolTime);
        }

        // GET: SchoolTimes/Details/5
        public async Task<IActionResult> DetailsSchoolTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolTime = await _context.SchoolTime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolTime == null)
            {
                return NotFound();
            }

            return View(schoolTime);
        }

        // GET: SchoolTimes/Create
        public IActionResult CreateSchoolTime()
        {
            return View();
        }

        // POST: SchoolTimes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSchoolTime([Bind("Id,SchoolStartingTime,SchoolEndingTime")] SchoolTime schoolTime)
        {
            if (ModelState.IsValid)
            {
                _context.Add(schoolTime);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ManageSchoolTime));
            }
            return View(schoolTime);
        }

        // GET: SchoolTimes/Edit/5
        public async Task<IActionResult> EditSchoolTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolTime = await _context.SchoolTime.FindAsync(id);
            if (schoolTime == null)
            {
                return NotFound();
            }
            return View(schoolTime);
        }

        // POST: SchoolTimes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSchoolTime(int id, [Bind("Id,SchoolStartingTime,SchoolEndingTime")] SchoolTime schoolTime)
        {
            if (id != schoolTime.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(schoolTime);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SchoolTimeExists(schoolTime.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ManageSchoolTime));
            }
            return View(schoolTime);
        }

        // GET: SchoolTimes/Delete/5
        public async Task<IActionResult> DeleteSchoolTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var schoolTime = await _context.SchoolTime
                .FirstOrDefaultAsync(m => m.Id == id);
            if (schoolTime == null)
            {
                return NotFound();
            }

            return View(schoolTime);
        }

        // POST: SchoolTimes/Delete/5
        [HttpPost, ActionName("DeleteSchoolTime")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedSchoolTime(int id)
        {
            var schoolTime = await _context.SchoolTime.FindAsync(id);
            if (schoolTime != null)
            {
                _context.SchoolTime.Remove(schoolTime);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageSchoolTime));
        }

        private bool SchoolTimeExists(int id)
        {
            return _context.SchoolTime.Any(e => e.Id == id);
        }


        public async Task<IActionResult> ManageSecurityStaff()
        {
            return View(await _context.SecurityStaff.ToListAsync());
        }

        // GET: SecurityStaffs/Details/5
        public async Task<IActionResult> SecurityStaffDetails(int? id)
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

        // GET: SecurityStaffs/Create
        public IActionResult CreateSecurityStaff()
        {
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateSecurityStaff([Bind("Id,Name,Gate,ContactNumber,Password")] SecurityStaff securityStaff)
        {

            EmailGenerator generator = new EmailGenerator();
            securityStaff.Email = generator.GenerateEmail(securityStaff.Name);

            try
            {
                var user = new SecureChildUser
                {
                    UserName = securityStaff.Email,
                    Email = securityStaff.Email,
                    PhoneNumber = securityStaff.ContactNumber, // Assuming this is PhoneNumber, not CellphoneNumber
                    EmailConfirmed = true
                };

                var securitystaff = new SecurityStaff
                {
                    Email = securityStaff.Email,
                    Gate = securityStaff.Gate,
                    Name = securityStaff.Name,
                    ContactNumber = securityStaff.ContactNumber,
                    Password = securityStaff.Password
                };

                var result = await _userManager.CreateAsync(user, securityStaff.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(user, "Security");

                    if (!roleResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Failed to add user to Security role.");
                        return View(securityStaff);
                    }

                    _context.Add(securityStaff);
                    _context.SecurityStaff.Add(securitystaff);
                    await _context.SaveChangesAsync();

                    return RedirectToAction(nameof(ManageSecurityStaff));
                }

            }
            catch (Exception ex)
            {

            }

            _context.Add(securityStaff);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageSecurityStaff));

            return View(securityStaff);
        }

        // GET: SecurityStaffs/Edit/5
        public async Task<IActionResult> EditSecurityStaff(int? id)
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

        // POST: SecurityStaffs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditSecurityStaff(int id, [Bind("Id,Email,Name,Gate,ContactNumber,Password")] SecurityStaff securityStaff)
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
                return RedirectToAction(nameof(ManageSecurityStaff));
            }
            return View(securityStaff);
        }

        // GET: SecurityStaffs/Delete/5
        public async Task<IActionResult> DeleteSecurityStaff(int? id)
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

        // POST: SecurityStaffs/Delete/5
        [HttpPost, ActionName("DeleteSecurityStaff")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SecurityStaffDeleteConfirmed(int id)
        {
            var securityStaff = await _context.SecurityStaff.FindAsync(id);
            if (securityStaff != null)
            {
                _context.SecurityStaff.Remove(securityStaff);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(ManageSecurityStaff));
        }

        private bool SecurityStaffExists(int id)
        {
            return _context.SecurityStaff.Any(e => e.Id == id);
        }

    }




}
