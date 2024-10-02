using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SecureChild.Areas.Identity.Data;
using SecureChild.Data;
using SecureChild.Models;

namespace SecureChild.Controllers
{
    public class SecurityStaffsController : Controller
    {
        private readonly SecureChildContext _context;
        private UserManager<SecureChildUser> _userManager;

        public SecurityStaffsController(SecureChildContext context , UserManager<SecureChildUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: SecurityStaffs
        
    }
}
