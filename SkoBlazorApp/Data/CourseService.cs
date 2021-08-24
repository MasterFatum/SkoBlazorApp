using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace SkoBlazorApp.Data
{
    public class CourseService
    {
        readonly SkoContext _skoContext = new SkoContext();

        public Course GetCourse(int courseId)
        {
            try
            {
                Course course = _skoContext.Courses.Where(u => u.UserId == 1)
                    .FirstOrDefault(c => c.Id == courseId);
                return course;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }
        public async Task<bool> AddCourseAsync(Course course)
        {
            try
            {
                await _skoContext.Courses.AddAsync(course);
                await _skoContext.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

            return false;
        }

        public async Task<bool> DeleteCourseAsync(int userId, int id)
        {
            try
            {
                Course course = await _skoContext.Courses.Where(u => u.UserId == userId).FirstOrDefaultAsync(c => c.Id == id);

                if (course != null)
                {
                    _skoContext.Courses.Remove(course);
                    await _skoContext.SaveChangesAsync();
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }
        public void EditCourse(Course course)
        {
            try
            {
                Course courseEdit = _skoContext.Courses.Where(u => u.UserId == course.UserId)
                    .FirstOrDefault(c => c.Id == course.Id);

                if (courseEdit != null)
                {
                    courseEdit.Title = course.Title;
                    courseEdit.Description = course.Description;
                    courseEdit.Hyperlink = course.Hyperlink;
                    courseEdit.FileName = course.FileName;

                    if (courseEdit.Evaluation != null)
                    {
                        courseEdit.DateEdit =
                            String.Format($"{DateTime.Now.ToShortDateString()} {DateTime.Now.ToShortTimeString()}");
                    }

                    _skoContext.Courses.Update(courseEdit);
                    _skoContext.SaveChanges();

                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<List<String>> GetAllCategory()
        {
            try
            {
                return await _skoContext.Courses.Select(c => c.Category).Distinct().ToListAsync();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }

        public async Task<List<Course>> GetCoursesByUserId(int userId)
        {
            try
            {
                List<Course> courses = await _skoContext.Courses.Where(u => u.UserId.Equals(userId)).ToListAsync();

                return courses.ToList();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Course> GetCoursesByCategory(int userId, string category)
        {
            try
            {
                var courses = _skoContext.Courses.Where(u => u.UserId == userId).Where(c => c.Category == category);

                return courses.ToList();

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Course> GetCoursesByFio(string lastname, string firstname, string middlename, string category)
        {
            try
            {
                User userId = _skoContext.Users.OrderBy(l => l.Lastname).Where(l => l.Lastname == lastname).Where(f => f.Firstname == firstname)
                    .FirstOrDefault(m => m.Middlename == middlename);

                IEnumerable<Course> courseses = _skoContext.Courses.Where(u => u.UserId == userId.Id)
                    .Where(c => c.Category == category);

                return courseses.ToList();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Course> GetCoursesByFio(string lastname, string firstname, string middlename)
        {
            try
            {
                User userId = _skoContext.Users.Where(l => l.Lastname == lastname).Where(f => f.Firstname == firstname)
                    .FirstOrDefault(m => m.Middlename == middlename);

                IEnumerable<Course> courseses = _skoContext.Courses.Where(u => u.UserId == userId.Id);

                return courseses.ToList();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public void SetRatingCourse(int userId, int id, int rating, string evaluating)
        {
            try
            {
                Course course = _skoContext.Courses.Where(u => u.UserId == userId).FirstOrDefault(c => c.Id == id);

                if (course != null)
                {
                    course.Evaluation = rating;
                    course.Evaluating = evaluating;

                    _skoContext.Courses.Update(course);
                    _skoContext.SaveChanges();

                }
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public string AllRating(int userId)
        {
            try
            {
                return _skoContext.Courses.Where(x => x.UserId == userId).Sum(r => r.Evaluation).ToString();
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public IEnumerable<Course> GetSummaryStatementByFio(string lastname, string firstname, string middlename)
        {
            try
            {
                User userId = _skoContext.Users.Where(l => l.Lastname == lastname).Where(f => f.Firstname == firstname)
                    .FirstOrDefault(m => m.Middlename == middlename);

                var summary = _skoContext.Courses.Where(u => u.UserId == userId.Id).GroupBy(c => c.Category).Select(c => new
                {
                    category = c.Key,
                    evaluation = c.Sum(e => e.Evaluation)
                }).AsEnumerable().Select(an => new Course { Category = an.category, Evaluation = an.evaluation });

                return summary.ToList();

            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        //public void ExportToExcel(DataGrid dataGrid)
        //{
        //    try
        //    {
        //        Excel.Application excel = new Excel.Application();
        //        excel.Visible = true;
        //        Workbook workbook = excel.Workbooks.Add(System.Reflection.Missing.Value);
        //        Worksheet sheet1 = (Worksheet)workbook.Sheets[1];

        //        for (int j = 0; j < dataGrid.Columns.Count; j++)
        //        {
        //            Range myRange = (Range)sheet1.Cells[1, j + 1];
        //            sheet1.Cells[1, j + 1].Font.Bold = true;
        //            sheet1.Columns[j + 1].ColumnWidth = 15;
        //            myRange.Value2 = dataGrid.Columns[j].Header;
        //        }
        //        for (int i = 0; i < dataGrid.Columns.Count; i++)
        //        {
        //            for (int j = 0; j < dataGrid.Items.Count; j++)
        //            {
        //                var b = dataGrid.Columns[i].GetCellContent(dataGrid.Items[j]) as TextBlock;
        //                Range myRange = (Range)sheet1.Cells[j + 2, i + 1];

        //                if (b != null) myRange.Value2 = b.Text;
        //            }
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message);
        //    }
    }
}
