using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using DisasterAlleviation.Models;
using DisasterAlleviation.Pages;


namespace DisasterAlleviation.Data
{
    public class ApplicationDbContext:IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
        {
        }

        // Add the IncidentReports DbSet
        public DbSet<Disaster> IncidentReports { get; set; }

        public DbSet<Donation> Donations { get; set; }

        public DbSet<Volunteer> Volunteers { get; set; }

        public DbSet<MonetaryDonation> MonetaryDonations { get; set; }
        public DbSet<GoodsDonation> GoodsDonations { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Donor> Donors { get; set; }

        public DbSet<Disaster> Disasters { get; set; }

    }
}
