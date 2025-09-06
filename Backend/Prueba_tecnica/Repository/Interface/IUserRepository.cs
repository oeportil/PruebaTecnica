using Prueba_tecnica.Models;

namespace Prueba_tecnica.Repository.Interface
{
    public interface IUserRepository
    {
        Task<string> Login(string username, string password);
        Task<IEnumerable<User>> GetAllUsers();
        Task<User> GetUser(int id);
        Task<User> CreateUser(User user);
        Task<User> UpdateUser(User user, int id);
        Task<User> DeleteUser(int id);
    }
}
