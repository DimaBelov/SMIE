using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SMIE.Core.Data;
using SMIE.DAL.Entities;
using SMIE.DAL.Interfaces;

namespace SMIE.DAL.Services
{
    static class UserDb
    {
        static readonly List<User> _db = new List<User>();

        public static void Add(User user) => _db.Add(user);

        public static User Get(string userNameOrEmail, string password) => _db.FirstOrDefault(u => (u.UserName.Equals(userNameOrEmail) && u.Password.Equals(password)) || (u.Email.Equals(userNameOrEmail) && u.Password.Equals(password)));

        public static bool IsUserNameExsists(string userName) => _db.Any(u => u.UserName.Equals(userName));

        public static bool IsEmailExsists(string email) => _db.Any(u => u.Email.Equals(email));
    }

    public class UserService : AppService, IUserService
    {
        public UserService(IGenericRepository repository) : base(repository)
        {
        }

        public async Task Add(User user)
        {
            await Task.Run(() => UserDb.Add(user));
        }

        public async Task<User> Get(string userNameOrEmail, string password)
        {
            return await Task.Run(() => UserDb.Get(userNameOrEmail, password));
        }

        public async Task<bool> IsUserNameExsists(string email)
        {
            return await Task.Run(() => UserDb.IsUserNameExsists(email));
        }

        public async Task<bool> IsEmailExsists(string email)
        {
            return await Task.Run(() => UserDb.IsEmailExsists(email));
        }
    }
}
