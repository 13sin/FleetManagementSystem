using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AuthServer.Areas.Identity.Data;
using AuthServer.Client;
using AuthServer.Models.DataContract;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.Controllers
{
    public class DriverController : Controller
    {
        private readonly UserManager<AuthServerUser> _userManager;
        public DriverController(UserManager<AuthServerUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: Driver
        public ActionResult Index()
        {
            return NoContent();
        }

        // GET: Driver/Details/5
        public ActionResult Details(int id)
        {
            return NoContent();
        }

        // GET: Driver/Create
        public ActionResult Create(string userid)
        {
            Debug.WriteLine("userid "+userid);
            ViewBag.userid = userid;
            return View();
        }

        // POST: Driver/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Debug.WriteLine("Driver Creating... ");
                Address address = APIClient.GetAddress(collection);
                Driver Driver = new Driver();
                Driver.Address = address;
                Driver.LicenseNumber = collection["LicenseNumber"];
                Driver.LicenseState = collection["LicenseState"];
                Driver.LicenseType = collection["LicenseType"];
                Driver.UserId = collection["UserId"];
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                Driver.CarrierId = authServerUser.carrierID;
                Driver driveradd = await APIClient.PostDriver(Driver);

                if (driveradd == null)
                {

                    ViewBag.success = "Employee as a Driver failed to create"; // linguistic fun  
                    return View();
                }
                else
                {
                    ViewBag.success = "Employee as a Driver Created"; // linguistic fun  
                    return View();
                }

            }
            catch(Exception e)
            {
                Debug.WriteLine("Exception " + e.Message);
                return View();
            }
}

        // GET: Broker/Edit/5
        public async Task<ActionResult> Edit(int id)
        {

            Driver driver = await APIClient.GetDriver(id);
            return View(driver);
        }

        // POST: Broker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            try
            {
                Debug.WriteLine("Driver Creating... ");
                Address address = APIClient.GetAddress(collection);
                Driver Driver = new Driver();
                Driver.Address = address;
                Driver.LicenseNumber = collection["LicenseNumber"];
                Driver.LicenseState = collection["LicenseState"];
                Driver.LicenseType = collection["LicenseType"];
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                Driver.CarrierId = authServerUser.carrierID;
                Driver driveradd = await APIClient.EditDriver(Driver);

                if (driveradd != null)
                {
                    return RedirectToRoute("EmployeeWeb");
                }
                else
                {

                    return RedirectToAction(nameof(Create));
                }

            }
            catch (Exception e)
            {
                Debug.WriteLine("Exception " + e.Message);
                return View();
            }
        }

        // GET: Broker/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            
            bool isdeleted = await APIClient.DeleteBroker(id);
            if (isdeleted)
            {
                Redirect("/");
                return RedirectToRoute("Home");
            }
            else
            {
                return RedirectToAction(nameof(Edit));
            }
        }

        // POST: Broker/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}