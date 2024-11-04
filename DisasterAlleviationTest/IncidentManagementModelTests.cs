using DisasterAlleviation.Data;
using DisasterAlleviation.Pages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviationTest
{
    public class IncidentManagementTests
    {
        private readonly TestReport _testReport;

        public IncidentManagementTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;
            _testReport = new TestReport();
        }

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
                var stopwatch = Stopwatch.StartNew();
                await pageModel.OnGetAsync();
                stopwatch.Stop();

                // Assert
                var isSuccess = pageModel.IncidentReports.First().IncidentType == "Flood" &&
                                pageModel.IncidentReports.Last().IncidentType == "Fire";

                _testReport.AddTestResult(nameof(OnGetAsync_ReturnsIncidentReportsOrderedByReportDate), isSuccess, stopwatch.Elapsed);

                // Generate report at the end of tests
                _testReport.GenerateHtmlReport("TestReport.html");
            }
        }
    }
}

