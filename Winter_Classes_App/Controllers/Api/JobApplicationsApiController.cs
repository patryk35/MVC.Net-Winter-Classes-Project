using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Winter_Classes_App.EntityFramework;
using Winter_Classes_App.Models;
using Winter_Classes_App.Models.Paging.Models;

namespace Winter_Classes_App.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobApplicationsApiController : ControllerBase
    {
        protected readonly DataContext _context;

        public JobApplicationsApiController(DataContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public PagingView GetJobApplications([FromQuery(Name = "searchString")]string searchString, [FromQuery(Name = "pageNo")]int pageNo = 1)
        {
            int totalPage, totalRecord, pageSize;
            pageSize = 8;


            List<JobApplication> record = null;
            if (searchString == null)
            {
                totalRecord = _context.JobApplications.Count();
                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = (from j in _context.JobApplications select j).OrderByDescending(j => j.Id).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
            }
            else
            {
                totalRecord = _context.JobApplications.Where(s => s.LastName.Contains(searchString) || s.EmailAddress.Contains(searchString)).Count();

                totalPage = (totalRecord / pageSize) + ((totalRecord % pageSize) > 0 ? 1 : 0);
                record = _context.JobApplications
                                .Where(s => s.LastName.Contains(searchString) || s.EmailAddress.Contains(searchString))
                                .OrderByDescending(j => j.Id)
                                .Skip((pageNo - 1) * pageSize)
                                .Take(pageSize)
                                .ToList();
            }
            // workaround below - it should be done in other way (medium)
            foreach (JobApplication r in record)
            {
                r.JobOffer = _context.JobOffers.FirstOrDefault(c => c.Id == r.JobOfferId);
            }
            PagingView empData = new PagingView
            {
                PagingModel = record,
                TotalPage = totalPage
            };

            return empData;
        }


        [HttpGet("{id}")]
        public async Task<JobApplication> GetJobApplicationsById(int id)
        {
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel == PrivilegesLevel.NO_LOGGEDIN)
            {
                return NotFound();
            }*/

            var jobApplication = await _context.JobApplications
                .Include(j => j.JobOffer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (jobApplication == null)
            {
                HttpContext.Response.StatusCode = 400; //not found
                return null;
            }

            return jobApplication;
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> PostAppliaction(IFormFile photoFile, IFormFile cvFile, [Bind("Id,JobOfferId,FirstName,LastName,PhoneNumber,EmailAddress,ContactAgreement")] JobApplication jobApplication)
        {

            jobApplication.JobOffer = await _context.JobOffers.FirstOrDefaultAsync(m => m.Id == jobApplication.JobOfferId);
            /*PrivilegesLevel privilegesLevel = await CheckGroup();
            ViewBag.PrivilegesLevel = (int)privilegesLevel;
            if (privilegesLevel != PrivilegesLevel.LOGGEDIN)
            {
                return NotFound();
            }*/

            if (cvFile == null || cvFile.Length == 0 || cvFile.FileName.Split(".").Last() != "pdf")
            {
                return NotFound();
            }



            if (photoFile == null || photoFile.Length == 0)
            {
                return NotFound();
            }


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
                        HttpContext.Response.StatusCode = 500;
                        return null;
                    }
                    jobApplication.CvUrl = cvBlockBlob.Name;


                    using (var fileStream = photoFile.OpenReadStream())
                    {
                        await photosBlockBlob.UploadFromStreamAsync(fileStream);
                    }

                    if (photosBlockBlob.Uri == null)
                    {
                        HttpContext.Response.StatusCode = 500;
                        return null;
                    }
                    jobApplication.UserImage = photosBlockBlob.Name;
                }

                _context.Add(jobApplication);
                await _context.SaveChangesAsync();
                return CreatedAtAction("Index", "JobApplicationsController", null, null);
            }
            return CreatedAtAction("Index", "JobApplicationsController", null, null);
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
            var JobApplication = await _context.JobApplications.FindAsync(id);
            _context.JobApplications.Remove(JobApplication);
            await _context.SaveChangesAsync();
            return CreatedAtAction("Index", "JobApplicationsController", null, null);
        }
        [ApiExplorerSettings(IgnoreApi = true)]
        private bool JobApplicationExists(int id)
        {
            return _context.JobApplications.Any(e => e.Id == id);
        }
    }
}