using DisasterAlleviation.Data;
using DisasterAlleviation.Pages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DisasterAlleviationTest
{
    public class ReportIncidentModelTests
    {
        private readonly ApplicationDbContext _context;

        public ReportIncidentModelTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;
            _context = new ApplicationDbContext(options);
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
            var result = await pageModel.OnPostAsync();

            // Assert
            Assert.Equal(1, _context.IncidentReports.Count());
            Assert.IsType<RedirectToPageResult>(result);
        }
    }

}
