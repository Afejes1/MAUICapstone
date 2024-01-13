using System;
using System.IO;
using System.Threading.Tasks;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;

namespace AFejes_Capstone.Services
{
    public class ReportService
    {
        private readonly DatabaseService _databaseService;

        public ReportService()
        {
            _databaseService = App.DatabaseServiceInstance;
        }
        public ReportService(DatabaseService databaseService)
        {
            _databaseService = databaseService;
        }

        public async Task GenerateReportAsync(string filePath)
        {
            using (var writer = new PdfWriter(filePath))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);
                    document.Add(new Paragraph("WGU: SAM Report")
                        .SetFontSize(20)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.CENTER));

                    document.Add(new Paragraph($"Generated on: {DateTime.Now}")
                        .SetFontSize(12)
                        .SetTextAlignment(iText.Layout.Properties.TextAlignment.RIGHT));

                    var terms = await _databaseService.GetTermsAsync();
                    foreach (var term in terms)
                    {
                        document.Add(new Paragraph($"Term: {term.Title}")
                            .SetFontSize(18));

                        document.Add(new Paragraph($"Start Date: {term.StartDate}")
                            .SetFontSize(14));

                        document.Add(new Paragraph($"End Date: {term.AnticipatedEndDate}")
                            .SetFontSize(14));

                        var courses = await _databaseService.GetCoursesForTermAsync(term.Id);
                        foreach (var course in courses)
                        {
                            document.Add(new Paragraph($"   Course: {course.CourseName}")
                                .SetFontSize(16));

                            var assessments = await _databaseService.GetAssessmentsByCourseIdAsync(course.Id);
                            foreach (var assessment in assessments)
                            {
                                document.Add(new Paragraph($"      Assessment: {assessment.AssessmentName}")
                                    .SetFontSize(14));
                            }
                        }
                    }
                }
            }
        }
    }

}
