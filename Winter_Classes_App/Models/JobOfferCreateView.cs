﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    public class JobOfferCreateView : JobOffer
    {
        public IEnumerable<Company> Companies { get; set; }
    }
}
