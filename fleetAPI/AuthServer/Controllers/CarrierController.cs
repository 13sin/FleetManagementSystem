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
    public class CarrierController : Controller
    {
        private readonly UserManager<AuthServerUser> _userManager;
        public CarrierController(UserManager<AuthServerUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: Broker
        public async Task<ActionResult> Index()
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            return RedirectToAction(nameof(Details), authServerUser.carrierID);
        }

        // GET: Carrier/Details/5
        public async Task<ActionResult> Details(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Carrier carrier = await APIClient.GetCarrier(authServerUser.carrierID);
            return View(carrier);
        }

        // GET: Carrier/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrier/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Debug.WriteLine("Carrier Creating... ");
                Address address = APIClient.GetAddress(collection);
                Carrier carrier = new Carrier();
                carrier.Address = address;
                carrier.Ctpat = collection["Ctpat"];
                carrier.Usdot = collection["Usdot"];
                carrier.Mc = collection["Mc"];
                carrier.Cvor = collection["Cvor"];
                Carrier carrieradd = await APIClient.PostCarrier(carrier);
                Debug.WriteLine("Carrier Created id " + carrieradd.Id);
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                authServerUser.carrierID = carrieradd.Id;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Carrier Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach(var Error in result.Errors) {
                        Debug.WriteLine("Carrier Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Create));
                }

        }
            catch(Exception e)
            {
                Debug.WriteLine("Exception " + e.Message);
                return View();
    }
}

        // GET: carrier.Mc = collection["Mc"];/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Carrier carrier = await APIClient.GetCarrier(authServerUser.carrierID);
            return View(carrier);
        }

        // POST: Broker/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            //try
            //{
                // TODO: Add update logic here
            Address address = APIClient.GetAddress(collection);
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Carrier carrier = await APIClient.GetCarrier(authServerUser.carrierID);
            address.Id = carrier.Address.Id;
            carrier.Address = address;
            carrier.AddressId = carrier.Address.Id;
            carrier.Mc = collection["Mc"];
            carrier.Cvor = collection["Cvor"];
            carrier.Usdot = collection["Usdot"];
            carrier.Ctpat = collection["Ctpat"];
            Carrier carrieredit = await APIClient.EditCarrier(carrier);
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Carrier Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Carrier Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Create));
                }
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: Carrier/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            bool isdeleted = await APIClient.DeleteCarrier(authServerUser.carrierID);
            if (isdeleted)
            {
                authServerUser.carrierID = 0;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Carrier succesfuly deleted");
                    RedirectToRoute("Home");
                    return RedirectToRoute("Home");
                }
                else
                {
                    Debug.WriteLine("Carrier failed to deleted");
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Carrier Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Details));
                }
                
            }
            else
            {
                Debug.WriteLine("Carrier failed to deleted");
                return RedirectToAction(nameof(Details));
            }
        }

        // POST: Carrier/Delete/5
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