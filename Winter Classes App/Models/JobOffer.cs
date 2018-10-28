using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    public class JobOffer
    {
        public long Id { get; set; }
        public string JobTitle { get; set; }

        public string JobDescription { get; set; }

        public int Salary { get; set; }

        public string SkillsRequired { get; set; }
    }
}
