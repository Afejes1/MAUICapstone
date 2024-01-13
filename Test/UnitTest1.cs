using NUnit.Framework;
using AFejes_Capstone.Models;
using AFejes_Capstone.Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AFejes_Capstone.Tests
{
    [TestFixture]
    public class DatabaseServiceTests
    {
        private DatabaseService _databaseService;

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setting up the database for testing...");
            _databaseService = new DatabaseService("TestDatabase");
            _databaseService.InitializeDatabaseAsync().Wait();
            TestContext.WriteLine("Database setup completed.");
        }

        [Test]
        public void TestAddTerm()
        {
            TestContext.WriteLine("Testing Term addition...");

            var term = new Term { Title = "Test Term", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023" };
            var result = _databaseService.SaveTermAsync(term).Result;

            Assert.Greater(result, 0, "Expected term addition to be successful.");

            TestContext.WriteLine($"Term added successfully with ID: {result}");
        }

        [Test]
        public void TestGetTerms()
        {
            TestContext.WriteLine("Testing retrieval of terms...");

            var terms = _databaseService.GetTermsAsync().Result;

            Assert.IsNotNull(terms, "Expected terms list to be not null.");
            Assert.IsInstanceOf<List<Term>>(terms, "Expected a list of Term objects.");

            TestContext.WriteLine($"Retrieved {terms.Count} terms from the database.");
        }

        [Test]
        public void TestDeleteTerm()
        {
            TestContext.WriteLine("Testing Term deletion...");

            var term = new Term { Title = "Test Term for Deletion", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023" };
            var result = _databaseService.SaveTermAsync(term).Result;

            var deleteResult = _databaseService.DeleteTermAsync(term).Result;

            Assert.AreEqual(1, deleteResult, "Expected term deletion to be successful.");

            TestContext.WriteLine("Term deletion was successful.");
        }

        [Test]
        public void TestAddCourse()
        {
            TestContext.WriteLine("Testing Course addition...");

            var course = new Course { CourseName = "Test Course", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023", TermId = 1 };
            var result = _databaseService.SaveCourseAsync(course).Result;

            Assert.Greater(result, 0, "Expected course addition to be successful.");

            TestContext.WriteLine($"Course added successfully with ID: {result}");
        }

        [Test]
        public void TestUpdateTerm()
        {
            TestContext.WriteLine("Testing Term update...");

            var term = new Term { Title = "Original Title", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023" };
            _databaseService.SaveTermAsync(term).Wait();

            term.Title = "Updated Title";
            _databaseService.SaveTermAsync(term).Wait();

            var updatedTerm = _databaseService.GetTermsAsync().Result.FirstOrDefault(t => t.Id == term.Id);
            Assert.AreEqual("Updated Title", updatedTerm.Title, "Expected term title to be updated.");

            TestContext.WriteLine("Term update was successful.");
        }

        [Test]
        public void TestGetCoursesByTermId()
        {
            TestContext.WriteLine("Testing retrieval of courses by TermId...");

            var term = new Term { Title = "Term for Courses", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023" };
            _databaseService.SaveTermAsync(term).Wait();

            var course = new Course { CourseName = "Course for Term", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023", TermId = term.Id };
            _databaseService.SaveCourseAsync(course).Wait();

            var courses = _databaseService.GetCoursesForTermAsync(term.Id).Result;
            Assert.IsTrue(courses.Any(c => c.TermId == term.Id), "Expected to retrieve courses for the given term.");

            TestContext.WriteLine($"Retrieved {courses.Count} courses for TermId {term.Id}.");
        }

        [Test]
        public void TestUpdateCourse()
        {
            TestContext.WriteLine("Testing Course update...");

            var course = new Course { CourseName = "Original Course Name", StartDate = "01/01/2023", AnticipatedEndDate = "06/30/2023", TermId = 1 };
            _databaseService.SaveCourseAsync(course).Wait();

            course.CourseName = "Updated Course Name";
            _databaseService.SaveCourseAsync(course).Wait();

            var updatedCourse = _databaseService.GetCoursesAsync().Result.FirstOrDefault(c => c.Id == course.Id);
            Assert.AreEqual("Updated Course Name", updatedCourse.CourseName, "Expected course name to be updated.");

            TestContext.WriteLine("Course update was successful.");
        }

        [Test]
        public void TestAddAssessment()
        {
            TestContext.WriteLine("Testing Assessment addition...");

            var assessment = new Assessment { AssessmentName = "Test Assessment", StartDate = "01/01/2023", EndDate = "01/15/2023", CourseId = 1, Type = "Objective" };
            var result = _databaseService.SaveAssessmentAsync(assessment).Result;

            Assert.Greater(result, 0, "Expected assessment addition to be successful.");

            TestContext.WriteLine($"Assessment added successfully with ID: {result}");
        }

        [Test]
        public void TestSearchFunctionality()
        {
            TestContext.WriteLine("Testing search functionality...");

            var searchTermsResult = _databaseService.SearchTermsAsync("Test Term").Result;
            Assert.IsTrue(searchTermsResult.Any(), "Expected to find terms with the given query.");

            var searchCoursesResult = _databaseService.SearchCoursesAsync("Test Course").Result;
            Assert.IsTrue(searchCoursesResult.Any(), "Expected to find courses with the given query.");

            var searchAssessmentsResult = _databaseService.SearchAssessmentsAsync("Test Assessment").Result;
            Assert.IsTrue(searchAssessmentsResult.Any(), "Expected to find assessments with the given query.");

            TestContext.WriteLine("All search tests were successful.");
        }

    }

    [TestFixture]
    public class ReportServiceTests
    {
        private ReportService _reportService;
        private DatabaseService _databaseService;

        private string _testFilePath = "TestReport.pdf";

        [SetUp]
        public void Setup()
        {
            TestContext.WriteLine("Setting up the database for testing...");
            _databaseService = new DatabaseService("TestDatabase");
            _databaseService.InitializeDatabaseAsync().Wait();
            TestContext.WriteLine("Database setup completed.");
            _reportService = new ReportService(_databaseService);
        }

        [Test]
        public async Task TestReportGeneration()
        {
            TestContext.WriteLine("Starting TestReportGeneration test...");

            // Try to generate a report
            TestContext.WriteLine($"Attempting to generate report at path: {_testFilePath}...");
            await _reportService.GenerateReportAsync(_testFilePath);

            // Check if the report file was created
            bool isFileCreated = File.Exists(_testFilePath);
            TestContext.WriteLine($"File creation status: {isFileCreated}");

            // Assertion
            Assert.IsTrue(isFileCreated, "Expected the report file to be created.");

            TestContext.WriteLine("TestReportGeneration test completed successfully.");
        }

        [Test]
        public async Task TestReportStructure()
        {
            TestContext.WriteLine("Starting TestReportStructure test...");

            // Generate a report
            TestContext.WriteLine($"Generating report at path: {_testFilePath}...");
            await _reportService.GenerateReportAsync(_testFilePath);

            // Get report file info
            var fileInfo = new FileInfo(_testFilePath);
            TestContext.WriteLine($"Report file size: {fileInfo.Length} bytes");

            // Check if the report has content (e.g., file size > 100 bytes)
            bool hasContent = fileInfo.Length > 100;
            TestContext.WriteLine($"Report content status: {hasContent}");

            // Assertion
            Assert.IsTrue(hasContent, "Expected the report to have content.");

            TestContext.WriteLine("TestReportStructure test completed successfully.");
        }



        [TearDown]
        public void CleanUp()
        {
            // Deleting the test report file after each test
            if (File.Exists(_testFilePath))
            {
                File.Delete(_testFilePath);
            }
        }
    }


}
