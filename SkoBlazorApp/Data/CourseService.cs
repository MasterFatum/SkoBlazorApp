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
        }

        public async Task<bool> DeleteCourseAsync(int userId, int id)
        {
            try
            {
                Course course = await _skoContext.Courses.Where(u => u.UserId == userId)
                    .FirstOrDefaultAsync(c => c.Id == id);

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
                            String.Format($"{DateTime.Now.ToShortDateString()}");
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
                List<Course> courses = await _skoContext.Courses.Where(u => u.UserId.Equals(userId))
                    .OrderBy(d => d.Date).ToListAsync();

                return courses;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }
        }

        public async Task<List<Course>> GetCoursesByUserId(int userId, int year)
        {
            try
            {
                List<Course> courses = await _skoContext.Courses.Where(u => u.UserId == userId)
                    .Where(y => y.Year == year).ToListAsync();

                return courses;
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


        public List<int> GetYears()
        {
            try
            {
                List<int> years = _skoContext.Courses.Select(y => y.Year).Distinct().OrderByDescending(y => y).ToList();

                return years;
            }
            catch (Exception)
            {
                throw new NotImplementedException();
            }

        }
    }
}
