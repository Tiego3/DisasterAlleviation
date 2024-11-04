using System;
using System.Collections.Generic;
using System.IO;

namespace DisasterAlleviationTest
{
    public class TestReport
    {
        public List<TestResult> TestResults { get; } = new List<TestResult>();

        public void AddTestResult(string testName, bool isSuccess, TimeSpan duration)
        {
            TestResults.Add(new TestResult
            {
                Name = testName,
                IsSuccess = isSuccess,
                Duration = duration
            });
        }

        public void GenerateHtmlReport(string reportPath)
        {
            var reportBuilder = new StringWriter();

            reportBuilder.WriteLine("<html>");
            reportBuilder.WriteLine("<head><title>Test Report</title></head>");
            reportBuilder.WriteLine("<body>");
            reportBuilder.WriteLine("<h1>Unit Test Report</h1>");
            reportBuilder.WriteLine("<table border='1'>");
            reportBuilder.WriteLine("<tr><th>Test Name</th><th>Status</th><th>Duration (ms)</th></tr>");

            foreach (var result in TestResults)
            {
                reportBuilder.WriteLine($"<tr><td>{result.Name}</td><td>{(result.IsSuccess ? "Passed" : "Failed")}</td><td>{result.Duration.TotalMilliseconds}</td></tr>");
            }

            reportBuilder.WriteLine("</table>");

            // Summary
            var totalTests = TestResults.Count;
            var passedTests = TestResults.Count(r => r.IsSuccess);
            var failedTests = totalTests - passedTests;

            reportBuilder.WriteLine("<h2>Summary</h2>");
            reportBuilder.WriteLine($"<p>Total Tests: {totalTests}</p>");
            reportBuilder.WriteLine($"<p>Passed: {passedTests}</p>");
            reportBuilder.WriteLine($"<p>Failed: {failedTests}</p>");

            reportBuilder.WriteLine("</body>");
            reportBuilder.WriteLine("</html>");

            File.WriteAllText(reportPath, reportBuilder.ToString());
        }

        public class TestResult // Change made here to make TestResult public
        {
            public string Name { get; set; }
            public bool IsSuccess { get; set; }
            public TimeSpan Duration { get; set; }
        }
    }
}
