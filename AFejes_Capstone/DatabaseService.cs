using AFejes_Capstone.Models;
using SQLite;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AFejes_Capstone
{
    public class DatabaseService
    {
        readonly SQLiteAsyncConnection _database;

        public DatabaseService(string databasePath)
        {
            _database = new SQLiteAsyncConnection(databasePath, Constants.Flags);
        }

        public async Task InitializeDatabaseAsync()
        {
            await _database.CreateTableAsync<Term>().ConfigureAwait(false);
            await _database.CreateTableAsync<Course>().ConfigureAwait(false);
            await _database.CreateTableAsync<Assessment>().ConfigureAwait(false);
        }

        public Task<List<Term>> GetTermsAsync()
        {
            return _database.Table<Term>().ToListAsync();
        }

        public Task<int> SaveTermAsync(Term term)
        {
            return term.Id == 0 ? _database.InsertAsync(term) : _database.UpdateAsync(term);
        }

        public async Task<int> DeleteTermAsync(Term term)
        {
            var courses = await GetCoursesForTermAsync(term.Id);
            foreach (var course in courses)
            {
                var assessments = await GetAssessmentsByCourseIdAsync(course.Id);
                foreach (var assessment in assessments)
                {
                    await DeleteAssessmentAsync(assessment);
                }
                await DeleteCourseAsync(course);
            }
            return await _database.DeleteAsync(term);
        }


        public Task<List<Course>> GetCoursesAsync()
        {
            return _database.Table<Course>().ToListAsync();
        }

        public Task<List<Course>> GetCoursesForTermAsync(int termId)
        {
            return _database.Table<Course>().Where(c => c.TermId == termId).ToListAsync();
        }

        public Task<int> SaveCourseAsync(Course course)
        {
            return course.Id == 0 ? _database.InsertAsync(course) : _database.UpdateAsync(course);
        }

        public async Task<int> DeleteCourseAsync(Course course)
        {
            var assessments = await GetAssessmentsByCourseIdAsync(course.Id);
            foreach (var assessment in assessments)
            {
                await DeleteAssessmentAsync(assessment);
            }
            return await _database.DeleteAsync(course);
        }


        public Task<List<Assessment>> GetAssessmentsAsync()
        {
            return _database.Table<Assessment>().ToListAsync();
        }

        public Task<List<Assessment>> GetAssessmentsByCourseIdAsync(int courseId)
        {
            return _database.Table<Assessment>().Where(a => a.CourseId == courseId).ToListAsync();
        }

        public Task<int> SaveAssessmentAsync(Assessment assessment)
        {
            return assessment.Id == 0 ? _database.InsertAsync(assessment) : _database.UpdateAsync(assessment);
        }

        public Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            return _database.DeleteAsync(assessment);
        }

        public async Task<List<SearchResultItem>> SearchTermsAsync(string query)
        {
            var terms = await _database.Table<Term>()
                                       .Where(t => t.Title.Contains(query))
                                       .ToListAsync();

            return terms.Cast<SearchResultItem>().ToList();
        }
        public async Task<List<SearchResultItem>> SearchCoursesAsync(string query)
        {
            var courses = await _database.Table<Course>()
                                         .Where(c => c.CourseName.Contains(query))
                                         .ToListAsync();

            return courses.Cast<SearchResultItem>().ToList();
        }

        public async Task<List<SearchResultItem>> SearchAssessmentsAsync(string query)
        {
            var assessments = await _database.Table<Assessment>()
                                             .Where(a => a.AssessmentName.Contains(query))
                                             .ToListAsync();

            return assessments.Cast<SearchResultItem>().ToList();
        }

    }
}
