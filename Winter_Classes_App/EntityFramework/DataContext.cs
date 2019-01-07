using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Winter_Classes_App.Models;

namespace Winter_Classes_App.EntityFramework
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DataContext() { }
        public virtual DbSet<JobApplication> JobApplications { get; set; }
        public virtual DbSet<JobOffer> JobOffers { get; set; }
        public virtual DbSet<Company> Companies { get; set; }
        public virtual DbSet<Session> Session { get; set; }
    }
}
