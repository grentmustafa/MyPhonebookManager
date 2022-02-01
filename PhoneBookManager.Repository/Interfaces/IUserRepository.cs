using MyPhoneBookManager.Entitites.Models;

namespace MyPhoneBookManager.Repository.Interfaces
{
    public interface IUserRepository
    {
        User GetById(long id);
        User GetByFullName(string fullName);
        IEnumerable<User> GetAllUsers();
        User AddUser(User userToAdd);
        User UpdateUser(long id, string firstname, string lastname);
        User DeleteUser(long id);

    }
}
