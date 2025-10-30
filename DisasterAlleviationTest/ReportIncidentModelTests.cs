using DisasterAlleviation.Data;
using DisasterAlleviation.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DisasterAlleviationTest
{
    public class ReportIncidentModelTests
    {
        private readonly ApplicationDbContext _context;
        private readonly TestReport _testReport;

        public ReportIncidentModelTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
            _testReport = new TestReport();
        }

        [Fact]
        public async Task OnPostAsync_ShouldAddIncidentReport_WhenModelStateIsValid()
        {
            // Arrange
            var pageModel = new ReportIncidentModel(_context)
            {
                Input = new ReportIncidentModel.InputModel
                {
                    IncidentType = "Earthquake",
                    Location = "Test Location",
                    Severity = "High",
                    Description = "Test description",
                    ReportDate = DateTime.Now
                }
            };

            // Act
            var stopwatch = Stopwatch.StartNew();
            var result = await pageModel.OnPostAsync();
            stopwatch.Stop();

            // Assert
            var isSuccess = _context.IncidentReports.Count() == 1 && result is RedirectToPageResult;
            _testReport.AddTestResult(nameof(OnPostAsync_ShouldAddIncidentReport_WhenModelStateIsValid), isSuccess, stopwatch.Elapsed);

            // Generate report at the end of tests
            _testReport.GenerateHtmlReport("*/TestReport.html");
        }
    }

   
}
