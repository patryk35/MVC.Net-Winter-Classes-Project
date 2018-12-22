using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    public class JobApplicationViews : JobApplication
    {
        public string OfferName { get; set; }

        public JobApplicationViews(JobApplication jobApplication)
        {
            this.FirstName = jobApplication.FirstName;
            this.LastName = jobApplication.LastName;
            this.Id = jobApplication.Id;
            this.PhoneNumber = jobApplication.PhoneNumber;
            this.OfferId = jobApplication.OfferId;
            this.ContactAgreement = jobApplication.ContactAgreement;
            this.CvUrl = jobApplication.CvUrl;
            this.EmailAddress = jobApplication.EmailAddress;
        }

        public JobApplicationViews()
        {
        }
    }
}
