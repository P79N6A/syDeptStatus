using System.Collections.Generic;
using System.Linq;

namespace syDeptStatus.Models.Entity
{
    public class Model
    {
    }

    public class User
    {
        public int Id { get; set; }
        public string Empno { get; set; }
        public string Password { get; set; }
    }

    public class UserStore
    {
        private static readonly List<User> _users = new List<User>
        {
            new User {Id = 1, Empno = "1", Password = "1"}
        };

        public User FindUser(string empno, string password)
        {
            return _users.FirstOrDefault(_ => _.Empno == empno && _.Password == password);
        }
    }
}