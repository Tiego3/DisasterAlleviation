using DisasterAlleviation.Data;
using DisasterAlleviation.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterAlleviationTest
{
    public class IncidentManagementTests
    {
        [Fact]
        public async Task OnGetAsync_ReturnsIncidentReportsOrderedByReportDate()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var context = new ApplicationDbContext(options))
            {
                context.IncidentReports.AddRange(
                    new IncidentReport
                    {
                        IncidentType = "Fire",
                        ReportDate = DateTime.Now.AddDays(-1),
                        Description = "Minor fire incident",
                        Location = "Location A",
                        Severity = "Low"
                    },
                    new IncidentReport
                    {
                        IncidentType = "Flood",
                        ReportDate = DateTime.Now,
                        Description = "Severe flooding",
                        Location = "Location B",
                        Severity = "High"
                    }
                );
                await context.SaveChangesAsync();
            }

            using (var context = new ApplicationDbContext(options))
            {
                var pageModel = new IncidentManagementModel(context);

                // Act
                await pageModel.OnGetAsync();

                // Assert
                Assert.Equal("Flood", pageModel.IncidentReports.First().IncidentType);
                Assert.Equal("Fire", pageModel.IncidentReports.Last().IncidentType);
            }
        }

    }

}
