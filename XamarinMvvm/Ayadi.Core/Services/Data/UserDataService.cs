using Ayadi.Core.Contracts.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Ayadi.Core.Contracts.Repository;

namespace Ayadi.Core.Services.Data
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserRepository _userRepository;

        private string _accessToken;
        public string AccessToken
        {
            get
            {
                return _accessToken;
            }

            set
            {
                _accessToken = value;
            }
        }

        public string LangId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public int UserId
        {
            get
            {
                throw new NotImplementedException();
            }

            set
            {
                throw new NotImplementedException();
            }
        }

        public UserDataService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<string> GetUserAccessToken()
        {
            return await _userRepository.GetUserAccessToken();
        }

        public async Task<User> GetSavedUser()
        {
            return await _userRepository.GetSavedUser();
        }

        // i could use 'GetUserById' instead of this but 'GetUserById' has lots of data wich i dont need 
        public async Task<List<UserAdress>> GetUserAdresses(User user)
        {
            return await _userRepository.GetUserAdresses(user);
        }

        public async Task<User> GetUserById(User user)
        {
            return await _userRepository.GetUserById(user);
        }

        public async Task<User> Login(string accessToken, User user)
        {
            return await _userRepository.Login(accessToken,user);
        }

        public async Task<User> LoginAsGuest(string accessToken)
        {
            return await _userRepository.LoginAsGuest(accessToken);
        }

        public void Logout(User user)
        {
            _userRepository.Logout(user);
        }

        public async Task<User> PutUser(User user)
        {
            return await _userRepository.PutUser(user);
        }

        public async Task<bool> SaveUserToLocal(User user)
        {
            return await  _userRepository.SaveUserToLocal(user);
        }

        public async Task<User> Signup(string accessToken, User user)
        {
            return await _userRepository.Signup(accessToken, user);
        }

        public async Task<bool> DeleteUserAddress(User user, int adressId)
        {
            return await _userRepository.DeleteUserAddress(user, adressId);
        }

        public async Task<UserAdress> AddUserAddress(User user, UserAdress adress)
        {
            return await _userRepository.AddUserAddress(user, adress);
        }

        public async Task<bool> UpdateUserAddress(User user, UserAdress adress)
        {
            return await _userRepository.UpdateUserAddress(user, adress);
        }

        public async Task<Response> ChangeUserPassword(User user, UserPassword password)
        {
            return await _userRepository.ChangeUserPassword(user, password);
        }

        public async Task<Response> RecoverUserPassword(User user)
        {
            return await _userRepository.RecoverUserPassword(user);
        }

        public async Task<Response> RecoverUserPassword(User user, UserPassword password)
        {
            return await _userRepository.RecoverUserPassword(user, password);
        }
    }
}
