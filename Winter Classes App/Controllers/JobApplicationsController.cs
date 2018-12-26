using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using CommunityCertForT;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class JobApplicationsController : Controller
    {
        private readonly DataContext _context;
        private IConfiguration _configuration;
        private AppSettings AppSettings { get; set; }
        public JobApplicationsController(IConfiguration Configuration, DataContext context)
        {
            _configuration = Configuration;
            AppSettings = _configuration.GetSection("AppSettings").Get<AppSettings>();
            _context = context;
        }

        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            /*AADGraph graph = new AADGraph(AppSettings);
            string groupName = "Admins";
            string groupId = AppSettings.AADGroups.FirstOrDefault(g => String.Compare(g.Name, groupName) == 0).Id;
            bool isIngroup = await graph.IsUserInGroup(User.Claims, groupId);

            if (isIngroup == false)
            {
                return NotFound();
            }*/

            if (String.IsNullOrEmpty(searchString))
            {
                return View(await _context.JobApplications
                    .Include(j => j.JobOffer)
                    .ToListAsync()
                    );
            }
            else
            {
                return View(await _context.JobApplications
                   .Where(s => s.JobOffer.JobTitle.Contains(searchString) || s.LastName.Contains(searchString))
                   .Include(j => j.JobOffer)
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

            var jobApplication = await _context.JobApplications
                .Include(j => j.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }

            return View(jobApplication);
        }

        public async Task<IActionResult> Create(int OfferId)
        {
            var model = new JobApplication
            {
                JobOfferId = OfferId,
                JobOffer = await _context.JobOffers.FirstOrDefaultAsync(m => m.Id == OfferId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,JobOfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement,CvUrl")] JobApplication jobApplication)
        {
            if (ModelState.IsValid || jobApplication.JobOfferId != 0)
            {
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobApplication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var jobApplication = await _context.JobApplications.Include(j => j.JobOffer).FirstOrDefaultAsync(o => o.Id == id);
            if (jobApplication == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobOffer,JobOfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement,CvUrl")] JobApplication jobApplication)
        {
            if (id != jobApplication.Id)
            {
                return NotFound();
            }

            if(jobApplication.JobOfferId == 0)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobApplication);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobApplicationExists(jobApplication.Id))
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
            return View(jobApplication);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            JobApplication jobApplication = await _context.JobApplications
                .Include(j => j.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                return NotFound();
            }
            return View(jobApplication);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var jobApplication = await _context.JobApplications.Include(j => j.JobOffer).FirstOrDefaultAsync(j => j.Id == id);
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
