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
    public class BrokerController : Controller
    {
        private readonly UserManager<AuthServerUser> _userManager;
        public BrokerController(UserManager<AuthServerUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: Broker
        public async Task<ActionResult> Index()
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            return RedirectToAction(nameof(Details), authServerUser.brokerID);
        }

        // GET: Customer/Details/5
        public async Task<ActionResult> Details(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Broker broker = await APIClient.GetBroker(authServerUser.brokerID);
            return View(broker);
        }

        // GET: Broker/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Broker/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Debug.WriteLine("Broker Creating... ");
                Address address = APIClient.GetAddress(collection);
                Broker broker = new Broker();
                broker.Address = address;
                broker.Mc = collection["Mc"];
                Broker brokeradd = await APIClient.PostBroker(broker);
                Debug.WriteLine("Broker Created id "+ brokeradd.Id);
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                authServerUser.brokerID = brokeradd.Id;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Broker Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach(var Error in result.Errors) {
                        Debug.WriteLine("Broker Error " + Error.Description);
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

        // GET: Broker/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Broker broker = await APIClient.GetBroker(authServerUser.brokerID);
            return View(broker);
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
            Broker broker = await APIClient.GetBroker(authServerUser.brokerID);
            address.Id = broker.Address.Id; 
            broker.Address = address;
            broker.AddressId = broker.Address.Id;
            broker.Mc = collection["Mc"];
            Broker brokeredit = await APIClient.EditBroker(broker);
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Broker Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Broker Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Create));
                }
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: Broker/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            bool isdeleted = await APIClient.DeleteBroker(authServerUser.brokerID);
            if (isdeleted)
            {
                authServerUser.brokerID = 0;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Redirect("/");
                    return RedirectToRoute("Home");
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Broker Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Edit));
                }
                
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