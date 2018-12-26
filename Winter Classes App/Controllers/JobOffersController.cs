using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class JobOffersController : Controller
    {
        private readonly DataContext _context;

        public JobOffersController(DataContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString) {

            if (String.IsNullOrEmpty(searchString))
            {
                return View(await _context.JobOffers
                    .Include(j => j.Company)
                    .ToListAsync()
                    );
            } else
            {
                return View(await _context.JobOffers
                   .Where(s => s.JobTitle.Contains(searchString) || s.Description.Contains(searchString))
                   .Include(j => j.Company)
                   .ToListAsync()
                   );
            }

        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        public IActionResult Create()
        {
            var model = new JobOfferCreateView
            {
                Companies = _context.Companies
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(JobOfferCreateView model)
        {
            if (!ModelState.IsValid)
            {
                model.Companies = _context.Companies;
                return View(model);
            }
  
            JobOffer jobOffer = new JobOffer
            {
                CompanyId = model.CompanyId,
                Company = _context.Companies.FirstOrDefault(c => c.Id == model.CompanyId),
                Description = model.Description,
                JobTitle = model.JobTitle,
                Location = model.Location,
                SalaryFrom = model.SalaryFrom,
                SalaryTo = model.SalaryTo,
                ValidUntil = model.ValidUntil,
                Created = DateTime.Now
            };
            _context.Add(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var jobOffer = await _context.JobOffers.Include(j => j.Company).FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            return View(jobOffer);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOffer model)
        {
            if (!ModelState.IsValid) return View();
            var jobOffer = await _context.JobOffers.FindAsync(model.Id);
            jobOffer.JobTitle = model.JobTitle;
            jobOffer.Description = model.Description;
            jobOffer.SalaryFrom = model.SalaryFrom;
            jobOffer.SalaryTo = model.SalaryTo;
            jobOffer.ValidUntil = model.ValidUntil;
            try
            {
                _context.Update(jobOffer);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!JobOfferExists(jobOffer.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return RedirectToAction("Details", new { id = model.Id });
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var jobOffer = await _context.JobOffers
                .Include(j => j.Company)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobOffer == null)
            {
                return NotFound();
            }

            return View(jobOffer);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobOffer = await _context.JobOffers.FindAsync(id);
            _context.JobOffers.Remove(jobOffer);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobOfferExists(int id)
        {
            return _context.JobOffers.Any(e => e.Id == id);
        }
    }
}
