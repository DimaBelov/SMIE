using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;

namespace SMIE.DAL.Services
{
    static class UserDb
    {
        static readonly List<User> _db = new List<User>();

        public static void Add(User user) => _db.Add(user);

        public static User Get(string email, string password) => _db.FirstOrDefault(u => u.Email.Equals(email) && u.Password.Equals(password));

        public static bool IsExsists(string email) => _db.Any(u => u.Email.Equals(email));
    }

    public class UserService : IUserService
    {
        public async Task Add(User user)
        {
            await Task.Run(() => UserDb.Add(user));
        }

        public async Task<User> Get(string email, string password)
        {
            return await Task.Run(() => UserDb.Get(email, password));
        }

        public async Task<bool> IsExsists(string email)
        {
            return await Task.Run(() => UserDb.IsExsists(email));
        }
    }
}
