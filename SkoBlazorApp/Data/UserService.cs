using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SkoBlazorApp.Data
{
    public class UserService
    {

        SkoContext _skoContext = new SkoContext();
        public void AddUser(User user)
        {
            try
            {
                User userExists = _skoContext.Users.FirstOrDefault(em => em.Email == user.Email);

                if (userExists == null)
                {
                    _skoContext.Users.Add(user);

                    _skoContext.SaveChanges();

                }
                else
                {

                }
            }
            catch (Exception e)
            {

            }

        }

        public void DeleteUser(int id)
        {
            try
            {
                User user = _skoContext.Users.Find(id);

                if (user != null)
                {
                    _skoContext.Users.Remove(user);
                    _skoContext.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                
            }
        }

        public void EditUser(int id, string lastname, string firstname, string middlename, string position, string email)
        {
            try
            {
                var user = _skoContext.Users.Find(id);

                if (user != null)
                {
                    user.Lastname = lastname;
                    user.Firstname = firstname;
                    user.Middlename = middlename;
                    user.Position = position;
                    user.Email = email;

                    _skoContext.Users.Update(user);

                    _skoContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public void EditUser(int id, string lastname, string firstname, string middlename, string position, string email, string password, string privilege)
        {
            try
            {
                var user = _skoContext.Users.Find(id);

                if (user != null)
                {
                    user.Id = id;
                    user.Lastname = lastname;
                    user.Firstname = firstname;
                    user.Middlename = middlename;
                    user.Position = position;
                    user.Email = email;
                    user.Password = password;
                    user.Privilege = privilege;

                    _skoContext.Users.Update(user);

                    _skoContext.SaveChanges();

                    
                }
            }
            catch (Exception ex)
            {
                
            }
        }

        public IEnumerable<User> GetAllUser()
        {
            try
            {
                IEnumerable<User> users = _skoContext.Users.ToList();

                return users;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public IEnumerable<UserEvaluationSummary> GetAllUsersName()
        {
            try
            {
                IEnumerable<UserEvaluationSummary> users = _skoContext.Users
                    .Select(x => new UserEvaluationSummary
                    {
                        LastName = x.Lastname,
                        FirstName = x.Firstname,
                        MidName = x.Middlename,
                        EvaluationSum = x.Courses.Sum(y => y.Evaluation)
                    })
                    .ToList();

                return users;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public List<String> GetFioUsers()
        {
            try
            {
                List<User> users = _skoContext.Users.OrderBy(l => l.Lastname).ToList();

                List<String> userFio = new List<string>();

                foreach (var user in users)
                {
                    userFio.Add(String.Format($"{user.Lastname} {user.Firstname} {user.Middlename}"));
                }

                return userFio;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public User ValidationUser(string username, string password)
        {
            try
            {
                User user = null;

                var findUser = _skoContext.Users.FirstOrDefault(u => u.Email == username);

                if (findUser != null)
                {
                    if (findUser.Password == password)
                    {
                        user = findUser;

                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }

        public User ValidationAdmin(string username, string password)
        {
            try
            {
                User user = null;

                var findUser = _skoContext.Users.Where(p => p.Privilege == "Admin").FirstOrDefault(u => u.Email == username);

                if (findUser != null)
                {
                    if (findUser.Password == password)
                    {
                        user = findUser;
                    }
                }

                return user;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }

        public void SokoDispose()
        {
            _skoContext.Dispose();
        }

        public string GetAllUsersCount()
        {
            try
            {
                return _skoContext.Users.Count().ToString();
            }
            catch (Exception ex)
            {
                
            }
            return null;
        }

        public int GetUserIdByFio(string lastname, string firstname, string middlename)
        {
            User userId = null;

            try
            {
                userId = _skoContext.Users.Where(l => l.Lastname == lastname).Where(f => f.Firstname == firstname)
                    .FirstOrDefault(m => m.Middlename == middlename);
            }
            catch (Exception ex)
            {
                
            }

            if (userId != null && userId.Id != 0)
            {
                return userId.Id;
            }
            return 0;
        }

        public bool UserIsOnline(int id)
        {
            try
            {
                var user = _skoContext.Users.Find(id);

                if (user != null)
                {
                    user.IsOnline = true;

                    _skoContext.Users.Update(user);

                    _skoContext.SaveChanges();

                    return true;
                }
            }
            catch (Exception ex)
            {
                
            }

            return false;
        }

        public bool UserIsOffline(int id)
        {
            try
            {
                var user = _skoContext.Users.Find(id);

                if (user != null)
                {
                    user.IsOnline = false;

                    _skoContext.Users.Update(user);

                    _skoContext.SaveChanges();

                    return false;
                }
            }
            catch (Exception ex)
            {
                
            }

            return true;
        }

        public IEnumerable<UsersInOnline> UsersInOnline()
        {
            try
            {
                IEnumerable<UsersInOnline> users = _skoContext.Users.Where(o => o.IsOnline)
                    .Select(x => new UsersInOnline
                    {
                        LastName = x.Lastname,
                        FirstName = x.Firstname,
                        MiddleName = x.Middlename,
                        Position = x.Position
                    })
                    .ToList();

                return users;
            }
            catch (Exception ex)
            {
                
            }

            return null;
        }
    }

    public class UserEvaluationSummary
    {

        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MidName { get; set; }
        public int? EvaluationSum { get; set; }
    }

    public class UsersInOnline
    {
        public string LastName { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string Position { get; set; }
    }
}
