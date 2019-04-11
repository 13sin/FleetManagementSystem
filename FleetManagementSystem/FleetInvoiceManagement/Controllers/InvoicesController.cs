using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FleetInvoiceManagement.Data;
using FleetInvoiceManagement.Models;
using IronPdf;
using System.IO;

namespace FleetInvoiceManagement.Controllers
{
    public class InvoicesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public InvoicesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Invoices
        public async Task<IActionResult> Index()
        {
            return View(await _context.Invoice.ToListAsync());
        }

        // GET: Invoices/Details/5
        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // GET: Invoices/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Invoices/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,InvoiceTitle,Price,Tax,Sales,CreationDate")] Invoice invoice)
        {
            string startupPath = AppDomain.CurrentDomain.BaseDirectory+ "\\temp";


            var OutputPath = startupPath + "\\" + invoice.InvoiceTitle + ".pdf";


            if (ModelState.IsValid)
            {
                 invoice.ID = Guid.NewGuid();
                 _context.Add(invoice);
                 await _context.SaveChangesAsync();

                var Renderer = new IronPdf.HtmlToPdf();

                var html = "<div style=\"width:100%;height:400px;border:1px solid black\">" +
                    "<table><tr><td>Invoice Title</td><td>"+invoice.InvoiceTitle+"</td></tr>" +
                    "<tr><td>Price($)</td><td>" + invoice.Price+ "</td></tr>" +
                    "<tr><td>Sales($)</td><td>" + invoice.Sales+ "</td></tr>" +
                    "<tr><td>Date</td><td>" + invoice.CreationDate+ "</td></tr>" +
                    "</table>" +
                    "</div>";
                var PDF = Renderer.RenderHtmlAsPdf(html);

                Renderer.PrintOptions.MarginTop = 50;  //millimeters
                Renderer.PrintOptions.MarginBottom = 50;
                Renderer.PrintOptions.CssMediaType = PdfPrintOptions.PdfCssMediaType.Print;
                Renderer.PrintOptions.Header = new SimpleHeaderFooter()
                {
                    CenterText = "Invoice: "+invoice.InvoiceTitle,
                    DrawDividerLine = true,
                    FontSize = 16
                };                
                Renderer.PrintOptions.Footer = new SimpleHeaderFooter()
                {
                    LeftText = DateTime.Now.ToString(),
                    RightText = "Page {1} of {1}",
                    DrawDividerLine = true,
                    FontSize = 14
                };

              
                PDF.SaveAs(OutputPath);


                //return RedirectToAction(nameof(Index));
            }
            //return View(invoice);
            var fileStream = new FileStream("~/Content/files/" + OutputPath,
                                     FileMode.Open,
                                     FileAccess.Read
                                   );
            var fsResult = new FileStreamResult(fileStream, "application/pdf");
            return fsResult;
        }

        // GET: Invoices/Edit/5
        public async Task<IActionResult> Edit(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice.FindAsync(id);
            if (invoice == null)
            {
                return NotFound();
            }
            return View(invoice);
        }

        // POST: Invoices/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Guid id, [Bind("ID,InvoiceTitle,Price,Tax,Sales,CreationDate")] Invoice invoice)
        {
            if (id != invoice.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(invoice);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InvoiceExists(invoice.ID))
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
            return View(invoice);
        }

        // GET: Invoices/Delete/5
        public async Task<IActionResult> Delete(Guid? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var invoice = await _context.Invoice
                .FirstOrDefaultAsync(m => m.ID == id);
            if (invoice == null)
            {
                return NotFound();
            }

            return View(invoice);
        }

        // POST: Invoices/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(Guid id)
        {
            var invoice = await _context.Invoice.FindAsync(id);
            _context.Invoice.Remove(invoice);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool InvoiceExists(Guid id)
        {
            return _context.Invoice.Any(e => e.ID == id);
        }
    }
}
