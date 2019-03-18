using System.Threading.Tasks;
using SMIE.DAL.Entities;

namespace SMIE.DAL.Interfaces
{
    public interface IUserService
    {
        Task Add(User user);

        Task<User> Get(string email, string password);

        Task<bool> IsUserNameExsists(string name);

        Task<bool> IsEmailExsists(string email);
    }
}
