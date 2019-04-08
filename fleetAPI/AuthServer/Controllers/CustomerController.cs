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
    public class CustomerController : Controller
    {
        private readonly UserManager<AuthServerUser> _userManager;
        public CustomerController(UserManager<AuthServerUser> userManager)
        {
            _userManager = userManager;
        }
        // GET: Customer
        public async Task<ActionResult> Index()
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            return RedirectToAction(nameof(Details), authServerUser.customerID);
        }

        // GET: Customer/Details/5
        public async Task<ActionResult> Details(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Customer customer = await APIClient.GetCustomer(authServerUser.customerID);
            return View(customer);
        }

        // GET: Customer/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Customer/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(IFormCollection collection)
        {
            try
            {
                Address address = APIClient.GetAddress(collection);
                Customer customer = new Customer();
                customer.Address = address;
                Customer customeradd = await APIClient.PostCustomer(customer);
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                authServerUser.customerID = customeradd.Id;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Customer Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach(var Error in result.Errors) {
                        Debug.WriteLine("Customer Error " + Error.Description);
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

        // GET: Customer/Edit/5
        public async Task<ActionResult> Edit(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            Customer customer = await APIClient.GetCustomer(authServerUser.customerID);
            return View(customer);
        }

        // POST: Customer/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(int id, IFormCollection collection)
        {
            //try
            //{
                // TODO: Add update logic here
                Address address = APIClient.GetAddress(collection);
                AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
                Customer customer = await APIClient.GetCustomer(authServerUser.customerID);
            address.Id = customer.Address.Id; 
                customer.Address = address;
            customer.AddressId = customer.Address.Id;
            Customer customeredit = await APIClient.EditCustomer(customer);
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    Debug.WriteLine("Customer Details " + collection.Keys.Count);
                    return RedirectToAction(nameof(Details), 1);
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Customer Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Create));
                }
            //}
            //catch
            //{
            //    return View();
            //}
        }

        // GET: Customer/Delete/5
        public async Task<ActionResult> Delete(int id)
        {
            AuthServerUser authServerUser = await _userManager.GetUserAsync(User);
            bool isdeleted = await APIClient.DeleteCustomer(authServerUser.customerID);
            if (isdeleted)
            {
                authServerUser.customerID = 0;
                IdentityResult result = await _userManager.UpdateAsync(authServerUser);
                if (result.Succeeded)
                {
                    RedirectToRoute("Home");
                    return RedirectToRoute("Home");
                }
                else
                {
                    foreach (var Error in result.Errors)
                    {
                        Debug.WriteLine("Customer Error " + Error.Description);
                    }
                    return RedirectToAction(nameof(Edit));
                }

            }
            else
            {
                return RedirectToAction(nameof(Edit));
            }
        }

        // POST: Customer/Delete/5
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