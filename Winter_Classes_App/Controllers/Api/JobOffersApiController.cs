using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;
using Winter_Classes_App.Models.Paging.Models;

namespace Winter_Classes_App.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobOffersApiController : ControllerBase
    {
        protected readonly DataContext _context;

        public JobOffersApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public PagingView GetJobOffers([FromQuery(Name = "searchString")]string searchString, [FromQuery(Name = "pageNo")]int pageNo = 1)
        {
            int totalPage, totalRecord, pageSize;
            pageSize = 8;


            List<JobOffer> record = null;
            if (searchString == null)
            {
                totalRecord = _context.JobOffers.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = (from j in _context.JobOffers select j).OrderByDescending(j => j.ValidUntil).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecord = _context.JobOffers.Where(s => s.JobTitle.Contains(searchString) || s.Description.Contains(searchString)).Count();

                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = _context.JobOffers
                                .Where(s => s.JobTitle.Contains(searchString) || s.Description.Contains(searchString))
                                .OrderByDescending(j => j.ValidUntil)
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }
            // workaround below - it should be done in other way (medium)
            foreach (JobOffer r in record)
            {
                r.Company = _context.Companies.FirstOrDefault(c => c.Id == r.CompanyId);
                r.Description = ""; //Some optimalization - that string isn't necessary and could be large
            }
            PagingView empData = new PagingView
            {
                PagingModel = record,
                TotalPage = totalPage
            };

            return empData;
        }


        [HttpGet("{id}")]
        public async Task<JobOffer> GetJobOfferById(int id)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel == PrivilegesLevel.NO_LOGGEDIN)
            {
                return NotFound();
            }*/

            var jobOffer = _context.JobOffers
                .FirstOrDefault(m => m.Id == id);
            if (jobOffer == null)
            {
                HttpContext.Response.StatusCode = 400; //not found
                return null;
            }

            return jobOffer;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostOffer([FromForm]JobOffer model)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            if (privilegesLevel < PrivilegesLevel.HR)
            {
                return Unauthorized();
            }*/

            if (model == null)
            {
                throw new ArgumentNullException("JobOffer can not be null");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
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
            return CreatedAtAction("Index", "JobOffersController", null, null);
        }

        [HttpPost("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(JobOffer model)
        {
           /* PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel < PrivilegesLevel.HR)
            {
                return NotFound();
            }*/
            if (!ModelState.IsValid) return BadRequest(ModelState);
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
            }
            return RedirectToAction("Details", new { id = model.Id });
        }


        [HttpDelete("{id}")]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel < PrivilegesLevel.HR)
            {
                return NotFound();
            }*/
            var jobOffer = await _context.JobOffers.FindAsync(id);
            _context.JobOffers.Remove(jobOffer);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Index", "JobOffersController", null, null);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private bool JobOfferExists(int id)
        {
            return _context.JobOffers.Any(e => e.Id == id);
        }
    }
}