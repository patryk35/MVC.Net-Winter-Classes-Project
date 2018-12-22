using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly DataContext _context;

        public JobApplicationsController(DataContext context)
        {
            _context = context;
        }

        // GET: JobApplications
        public async Task<IActionResult> Index()
        {
            List<JobApplication> jobApplications = await _context.JobApplications.ToListAsync();
            List<JobOffer> applicationsOffers = await _context.JobOfers.ToListAsync();
            List<JobApplicationViews> jobApplicationViews = new List<JobApplicationViews>();
            foreach (JobApplication application in jobApplications) {
                if (application != null){
                    JobApplicationViews jobApplicationView = new JobApplicationViews(application);
                    var offer = applicationsOffers.FirstOrDefault(m => m.Id == application.OfferId);
                    if (offer != null)
                    {
                        jobApplicationView.OfferName = offer.JobTitle;
                    } else
                    {
                        jobApplicationView.OfferName = "Offer is not available"; // to be removed
                    }
                    jobApplicationViews.Add(jobApplicationView);
                }
            }
            return View(jobApplicationViews);
        }

        /*public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {

            if (String.IsNullOrEmpty(searchString))
            {
                return View(await _context.JobApplications
                    .Include(j => j.JobOffer)
                    .ToListAsync()
                    );
            }
            else
            {
                return View(await _context.JobOfers
                   .Where(s => s.JobTitle.Contains(searchString) || s.Description.Contains(searchString))
                   .Include(j => j.Company)
                   .ToListAsync()
                   );
            }

        }*/



        // GET: JobApplications/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobApplication jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            JobOffer applicationOffer = await _context.JobOfers
                .FirstOrDefaultAsync(m => m.Id == jobApplication.OfferId);
            if (jobApplication == null || applicationOffer == null)
            {
                return NotFound();
            }
            JobApplicationViews jobApplicationViews = new JobApplicationViews(jobApplication);
            jobApplicationViews.OfferName = applicationOffer.JobTitle;
            return View(jobApplicationViews);
        }

        // GET: JobApplications/Create
        public IActionResult Create(int OfferId, string OfferName)
        {
            return View(new JobApplicationViews() { OfferId = OfferId, OfferName = OfferName });
        }

        // POST: JobApplications/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,OfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement,CvUrl")] JobApplicationViews jobApplicationViews)
        {
            if (ModelState.IsValid)
            {
                _context.Add(jobApplicationViews);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            JobOffer applicationOffer = await _context.JobOfers
                .FirstOrDefaultAsync(m => m.Id == jobApplicationViews.OfferId);
            jobApplicationViews.OfferName = applicationOffer.JobTitle;
            return View(jobApplicationViews);
        }

        // GET: JobApplications/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobApplication jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            JobOffer applicationOffer = await _context.JobOfers
                .FirstOrDefaultAsync(m => m.Id == jobApplication.OfferId);
            if (jobApplication == null || applicationOffer == null)
            {
                return NotFound();
            }
            JobApplicationViews jobApplicationViews = new JobApplicationViews(jobApplication);
            jobApplicationViews.OfferName = applicationOffer.JobTitle;
            return View(jobApplicationViews);
        }

        // POST: JobApplications/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,OfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement,CvUrl")] JobApplicationViews jobApplicationViews)
        {
            if (id != jobApplicationViews.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplicationViews);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplicationViews.Id))
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
            JobOffer applicationOffer = await _context.JobOfers
                .FirstOrDefaultAsync(m => m.Id == jobApplicationViews.OfferId);
            jobApplicationViews.OfferName = applicationOffer.JobTitle;
            return View(jobApplicationViews);
        }

        // GET: JobApplications/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobApplication jobApplication = await _context.JobApplications
                .FirstOrDefaultAsync(m => m.Id == id);
            JobOffer applicationOffer = await _context.JobOfers
                .FirstOrDefaultAsync(m => m.Id == jobApplication.OfferId);
            if (jobApplication == null || applicationOffer == null)
            {
                return NotFound();
            }
            JobApplicationViews jobApplicationViews = new JobApplicationViews(jobApplication);
            jobApplicationViews.OfferName = applicationOffer.JobTitle;
            return View(jobApplicationViews);
        }

        // POST: JobApplications/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(jobApplication);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}
