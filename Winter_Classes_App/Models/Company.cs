using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Winter_Classes_App.Models
{
    public class Company : IPagingModel
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
    }
}
