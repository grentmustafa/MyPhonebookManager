using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using MyPhoneBookManager.Entitites.Models;
using MyPhoneBookManager.Repository.Interfaces;

namespace MyPhoneBookManager.Repository.RepositoriesWithJson
{
    public class UserRepositoryJsonFile : IUserRepository
    {
        private readonly string userPath;
        public UserRepositoryJsonFile()
        {
            userPath = Directory.GetParent(Directory.GetCurrentDirectory()).FullName + "\\filesToRead\\users.json";
        }
        public User GetById(long id)
        {
            var users = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
            return users != null ? users.FirstOrDefault(x => x.ID==id) : null;
        }

        public User GetByFullName(string fullName)
        {
            var users = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
            return users != null ? users.FirstOrDefault(x => fullName.Equals(x.FirstName + " " + x.LastName)) : null;
        }

        public IEnumerable<User> GetAllUsers()
        {
            var users = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
            return users;
        }

        public User AddUser(User userToAdd)
        {
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
                var users = responseFromFile == null ? new List<User>() : responseFromFile.ToList();
                userToAdd.ID = users.Count() + 1;
                users.Add(userToAdd);
                JsonHelper.WriteJsonToText<User>(userPath, users);
                return userToAdd;
            }
            catch
            {
                userToAdd = null;
            }

            return userToAdd;
        }

        public User UpdateUser(long id, string firstname, string lastname)
        {
            User userToUpdate = new User();
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
                if (responseFromFile == null)
                {
                    return userToUpdate;
                }
                var users= responseFromFile.ToList();
                userToUpdate = users.FirstOrDefault(u => u.ID == id);
                if (userToUpdate == null)
                {
                    userToUpdate = new User();
                    return userToUpdate;
                }
                userToUpdate.FirstName=firstname;
                userToUpdate.LastName = lastname;
                JsonHelper.WriteJsonToText<User>(userPath, users);
            }
            catch
            {
                userToUpdate = null;
            }
            return userToUpdate;
        }

       

        public User DeleteUser(long id)
        {
            User userDeleted = new User();
            try
            {
                var responseFromFile = JsonHelper.ReadAndDeserializeFromFile<User>(userPath);
                if (responseFromFile == null)
                {
                    return userDeleted;
                }
                var users = responseFromFile.ToList();
                userDeleted = users.FirstOrDefault(u => u.ID == id);
                if (userDeleted == null)
                {
                    userDeleted = new User();
                    return userDeleted;
                }

                users.Remove(userDeleted);
                JsonHelper.WriteJsonToText<User>(userPath, users);

            }
            catch
            {
                userDeleted = null;
            }
            return userDeleted;
        }
    }
}
