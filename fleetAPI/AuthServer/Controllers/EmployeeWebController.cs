using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AuthServer.Models;
using AuthServer.Areas.Identity.Data;
using AuthServer.Models.DataContract;

namespace SecurityWeb.Controllers
{
    [Authorize]
    public class EmployeeWebController : Controller
    {
        private readonly AuthServerContext _context;
        private readonly SignInManager<AuthServerUser> _signInManager;
        private readonly UserManager<AuthServerUser> _userManager;

        public EmployeeWebController(AuthServerContext context, UserManager<AuthServerUser> userManager,
            SignInManager<AuthServerUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: EmployeeWeb
        public async Task<IActionResult> Index()
        {
            var signeduser = await _userManager.GetUserAsync(User);
            List<AuthServerUser> authuser = new List<AuthServerUser>();
            List<AuthServerUser> finalauthuser = new List<AuthServerUser>();
            var employee = await _context.EmployeeInfo.ToListAsync();
            foreach (string userid in employee.Select(x=>x.UserId))
            {
                var user = await _userManager.FindByIdAsync(userid);
                authuser.Add(user);
            }
            if(signeduser.brokerID !=0)
            {
                finalauthuser.AddRange(authuser.Where(x => x.brokerID == signeduser.brokerID ).ToList());
            }
            if (signeduser.carrierID != 0)
            {
                finalauthuser.AddRange(authuser.Where(x => x.carrierID == signeduser.carrierID).ToList());
            }
            if (signeduser.customerID != 0)
            {
                finalauthuser.AddRange(authuser.Where(x => x.customerID == signeduser.customerID).ToList());
            }
            List<EmployeeInfo>  companyemployee = new List<EmployeeInfo>();
            foreach (AuthServerUser user in finalauthuser)
            {
                companyemployee.Add(employee.Where(x => x.UserId == user.Id).FirstOrDefault());
            }
            return View(companyemployee);
        }

        // GET: EmployeeWeb/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeInfo = await _context.EmployeeInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            return View(employeeInfo);
        }

        // GET: EmployeeWeb/Create
        public async Task<IActionResult> Create()
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            ViewData["customerID"] = authServerUser.customerID;
            ViewData["brokerID"] = authServerUser.brokerID;
            ViewData["carrierID"] = authServerUser.carrierID;
            return View();
        }

        // POST: EmployeeWeb/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,Email,Password,FirstName,LastName,Address,Role")] EmployeeInfo employeeInfo)
        {
            if (ModelState.IsValid)
            {
                var user = new AuthServerUser { UserName = employeeInfo.Email, Email = employeeInfo.Email };
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                if (employeeInfo.Role == "Customer")
                {
                    user.customerID = authServerUser.customerID;
                }
                if (employeeInfo.Role == "Broker")
                {
                    user.brokerID = authServerUser.brokerID;
                }
                if (employeeInfo.Role == "Carrier")
                {
                    user.carrierID = authServerUser.carrierID;
                }
                if (employeeInfo.Role == "Driver")
                {
                    user.carrierID = authServerUser.carrierID;
                }
                var result = await _userManager.CreateAsync(user, employeeInfo.Password);
                if (result.Succeeded)
                {
                    employeeInfo.UserId = user.Id;
                    _context.Add(employeeInfo);
                    await _context.SaveChangesAsync();
                    if(employeeInfo.Role == "Driver")
                    {
                        return RedirectToAction("Create","Driver", new { userid=employeeInfo.UserId, name = employeeInfo.FirstName+" "+employeeInfo.LastName });
                    }
                    else
                    {
                        return RedirectToAction(nameof(Index));
                    }
                    
                }
            }
            return View(employeeInfo);
        }

        // GET: EmployeeWeb/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeInfo = await _context.EmployeeInfo.FindAsync(id);
            if (employeeInfo == null)
            {
                return NotFound();
            }
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            ViewData["customerID"] = authServerUser.customerID;
            ViewData["brokerID"] = authServerUser.brokerID;
            ViewData["carrierID"] = authServerUser.carrierID;
            return View(employeeInfo);
        }

        // POST: EmployeeWeb/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,FirstName,LastName,Address,Role")] EmployeeInfo employeeInfo)
        {
            if (id != employeeInfo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(employeeInfo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeInfoExists(employeeInfo.Id))
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
            return View(employeeInfo);
        }

        // GET: EmployeeWeb/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employeeInfo = await _context.EmployeeInfo
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employeeInfo == null)
            {
                return NotFound();
            }

            return View(employeeInfo);
        }

        // POST: EmployeeWeb/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employeeInfo = await _context.EmployeeInfo.FindAsync(id);
            var authuser = await _userManager.FindByIdAsync(employeeInfo.UserId);
            await _userManager.DeleteAsync(authuser);
            _context.EmployeeInfo.Remove(employeeInfo);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeInfoExists(int id)
        {
            return _context.EmployeeInfo.Any(e => e.Id == id);
        }
    }
}
