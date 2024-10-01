using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DisasterAlleviation.Models;


namespace DisasterAlleviation.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // Add the IncidentReports DbSet
        public DbSet<IncidentReport> IncidentReports { get; set; }

        public DbSet<Donation> Donations { get; set; }

    }
}
