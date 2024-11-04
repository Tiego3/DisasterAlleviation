using DisasterAlleviation;
using DisasterAlleviation.Data;
using DisasterAlleviation.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

public class IntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    public ApplicationDbContext Context { get; private set; }

    public IntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(builder =>
        {
            builder.ConfigureServices(services =>
            {
                // Remove the existing DbContext registration
                var serviceDescriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                if (serviceDescriptor != null)
                {
                    services.Remove(serviceDescriptor);
                }

                // Add DbContext with InMemory options for testing
                services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var serviceProvider = services.BuildServiceProvider();
                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    Context = scopedServices.GetRequiredService<ApplicationDbContext>();
                    SeedDatabase();
                }
            });
        }).CreateClient();
    }

    private void SeedDatabase()
    {
        // Seed initial data for testing
        Context.Donations.Add(new Donation
        {
            ResourceType = "Food",
            NameOfItem = "Rice",
            Date = DateTime.Now,
            NumberOfItems = 10,
            Description = "Staple food item",
            Status = "Pending"
        });

        Context.IncidentReports.Add(new IncidentReport
        {
            IncidentType = "Flood",
            Location = "Coastal Area",
            Severity = "High",
            Description = "Severe flooding due to heavy rains",
            ReportDate = DateTime.Now
        });

        Context.SaveChanges();
    }

    // Example test method
    [Fact]
    public async Task TestGetDonations()
    {
        // Act: Call the donations endpoint (replace with your actual endpoint)
        var response = await _client.GetAsync("/Donations");
        response.EnsureSuccessStatusCode();

        // Assert: Check that the response contains the seeded data
        var donations = await Context.Donations.ToListAsync();
        Assert.NotEmpty(donations);
    }
}
