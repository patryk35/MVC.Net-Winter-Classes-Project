using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using CommunityCertForT;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class JobApplicationsController : BaseController
    {
        public JobApplicationsController(IConfiguration Configuration, DataContext context) : base(Configuration, context){}

        public async Task<IActionResult> Index([FromQuery(Name = "search")] string searchString)
        {
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int) privilegesLevel;
            if (privilegesLevel < PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            } else if (privilegesLevel == PrivilegesLevel.LOGGEDIN)
            {
                return View(await _context.JobApplications
                    .Include(j => j.JobOffer)
                    .Where(j => j.EmailAddress == ((ClaimsIdentity)User.Identity).FindFirst("Emails").Value)
                    .ToListAsync()
    );
            }

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
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel == PrivilegesLevel.NO_LOGGEDIN)
            {
                return NotFound();
            }

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
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel != PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }

            var model = new JobApplication
            {
                JobOfferId = OfferId,
                JobOffer = await _context.JobOffers.FirstOrDefaultAsync(m => m.Id == OfferId)
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(IFormFile photoFile, IFormFile cvFile, [Bind("Id,JobOfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement")] JobApplication jobApplication)
        {
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel != PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }

            if (cvFile == null || cvFile.Length == 0)
                return View(jobApplication);

           if (photoFile == null || photoFile.Length == 0)
                return View(jobApplication);

            if (ModelState.IsValid || jobApplication.JobOfferId != 0)
            {
                string connectionString = "DefaultEndpointsProtocol=https;AccountName=picturesstorage0pm0ja;AccountKey=G1cR4nj5zRNCE6HO/WOnxTyyCKUzfyYq0FqLNgY/JFs6siFKmNVpiBOwzvX38Li1mEJ+G39WBj4Ni2SaGUsPPg==;EndpointSuffix=core.windows.net";

                CloudStorageAccount storageAccount = null;
                if (CloudStorageAccount.TryParse(connectionString, out storageAccount))
                {
                    CloudBlobClient cloudBlobClient = storageAccount.CreateCloudBlobClient();

                    // Get reference to the blob container by passing the name by reading the value from the configuration (appsettings.json)
                    CloudBlobContainer cvContainer = cloudBlobClient.GetContainerReference("applications");
                    CloudBlobContainer photosContainer = cloudBlobClient.GetContainerReference("photos");
                    
                    string cvExtention = cvFile.FileName.Split(".").Last();
                    string photoExtention = photoFile.FileName.Split(".").Last();
                    // Get the reference to the block blob from the container
                    CloudBlockBlob cvBlockBlob = cvContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + "." + cvExtention);
                    CloudBlockBlob photosBlockBlob = photosContainer.GetBlockBlobReference(Guid.NewGuid().ToString() + "." + photoExtention);


                    // Upload the files
                    using (var fileStream = cvFile.OpenReadStream())
                    {
                        await cvBlockBlob.UploadFromStreamAsync(fileStream);
                    }

                    if (cvBlockBlob.Uri == null)
                    {
                        return View(jobApplication);
                    }
                    jobApplication.CvUrl = cvBlockBlob.Name;


                    using (var fileStream = photoFile.OpenReadStream())
                    {
                        await photosBlockBlob.UploadFromStreamAsync(fileStream);
                    }

                    if (photosBlockBlob.Uri == null)
                    {
                        return View(jobApplication);
                    }
                    jobApplication.UserImage = photosBlockBlob.Name;
                }
                
                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(jobApplication);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel != PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }
            
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            var jobApplication = await _context.JobApplications.Include(j => j.JobOffer).FirstOrDefaultAsync(o => o.Id == id);
            if (jobApplication == null) return new HttpStatusCodeResult(HttpStatusCode.NotFound);
            return View(jobApplication);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,JobOffer,JobOfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement")] JobApplication jobApplication)
        {
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel != PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }

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
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel < PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }

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
            PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel < PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }

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
