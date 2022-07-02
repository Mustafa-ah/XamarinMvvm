using Ayadi.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Contracts.Services
{
    public interface IUserDataService
    {
        Task<User> GetUserById(User user);

        Task<User> PutUser(User user);
        // i could use 'GetUserById' instead of this but 'GetUserById' has lots of data wich i dont need 
        Task<List<UserAdress>> GetUserAdresses(User user);

        Task<User> Signup(string accessToken, User user);
        Task<User> Login(string accessToken, User user);
        Task<User> LoginAsGuest(string accessToken);

        Task<bool> DeleteUserAddress(User user, int adressId);
        Task<UserAdress> AddUserAddress(User user, UserAdress adress);
        Task<bool> UpdateUserAddress(User user, UserAdress adress);

        Task<Response> ChangeUserPassword(User user, UserPassword password);
        Task<Response> RecoverUserPassword(User user);
        Task<Response> RecoverUserPassword(User user, UserPassword password);
        // local
        Task<User> GetSavedUser();
        Task<bool> SaveUserToLocal(User user);
        Task<string> GetUserAccessToken();
        void Logout( User user);

        string AccessToken { get; set; }
        string LangId { get; set; }
        int UserId { get; set; }
    }
}
