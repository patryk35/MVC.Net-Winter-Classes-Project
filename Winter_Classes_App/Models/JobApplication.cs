using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    public class JobApplication : IPagingModel
    {
        public int Id { get; set; }
        public virtual JobOffer JobOffer { get; set; }
        public string UserImage { get; set; }
        public virtual int JobOfferId { get; set; }
        [Required]
        public string FirstName { get; set; }
        [Required]
        public string LastName { get; set; }
        [Required]
        [Phone]
        public string PhoneNumber { get; set; }
        [Required]
        [EmailAddress]
        public string EmailAddress { get; set; }
        public bool ContactAgreement { get; set; }
        public string CvUrl { get; set; }
    }
}
