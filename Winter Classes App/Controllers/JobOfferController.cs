using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.Controllers
{
    public class JobOfferController : Controller
    {
        private static List<JobOffer> _jobOffers = new List<JobOffer>
        {
            new JobOffer{Id=1, JobTitle="Backend Developer", JobDescription="Experienced .Net Developer", Salary=1000, SkillsRequired=".NET, JS"},
            new JobOffer{Id=2, JobTitle="Frontend Developer"},
            new JobOffer{Id=3, JobTitle="Manager"},
            new JobOffer{Id=4, JobTitle="Teacher"},
            new JobOffer{Id=5, JobTitle="Cook"},
            new JobOffer{Id=6, JobTitle="Android Developer"},


        };
        public IActionResult Index()
        {
            return View(_jobOffers);
        }
        public IActionResult Details(int id)
        {
            var offer = _jobOffers.FirstOrDefault(o => o.Id == id);
            return View(offer);
        }
    }
}