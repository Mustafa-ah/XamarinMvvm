using Akavache;
using System.Reactive.Linq;
using Ayadi.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Newtonsoft.Json.Linq;

namespace Ayadi.Core.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {

        public async Task<List<UserAdress>> GetUserAdresses(User user)
        {
            List<UserAdress> ls = new List<UserAdress>();
            try
            {
                //string token = await GetAccessToken();/api/customers/1/address
                ls = await GetUserAdresses($"/api/customers/{user.Id}/address", user.AccessToken);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return ls;
        }

        public async Task<User> GetUserById(User user)
        {
            User us = new User();
            try
            {
                string url_ = $"/api/customer/account/{user.Id}";
                string jsonString = await GetStringAsync(url_, user.AccessToken);
                us = await GetUser(jsonString);
                //us = await GetUserData();
                //if (us == null)
                //{
                //    string token = await GetAccessToken();
                //    string jsonString = await GetStringAsync(url_, token);
                //    us = await GetUser(jsonString);
                //    await SaveUserToLocal(us);
                //}
            }
            catch (Exception)
            {

                //throw;//x
            }
            return us;
        }

        private async Task<User> GetUser(string jsonString)
        {
            User us_ = new User();
            try
            {
                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["errors"] == null)
                {
                    IList<JToken> _itemList = jsonObject_["customers"].Children().ToList();
                    us_ = await DeserializeTObject<User>(_itemList[0].ToString());
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return us_;
        }

        private async Task<List<UserAdress>> GetUserAdresses(string resource, string token)
        {
            List<UserAdress> us_Adrress = new List<UserAdress>();
            // FavouriteRepository favRepo = new FavouriteRepository();
            try
            {
                string jsonString = await GetStringAsync(resource, token);

                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["addresses"] == null)
                {
                    return us_Adrress;
                }
               // IList<JToken> _itemList = jsonObject_["customers"].Children().ToList();
                IList<JToken> _AdressesList = jsonObject_["addresses"].Children().ToList();

                foreach (JToken ads in _AdressesList)
                {
                    UserAdress adres_ = await DeserializeTObject<UserAdress>(ads.ToString());
                    if (adres_ != null)
                    {
                        us_Adrress.Add(adres_);
                    }
                }
            }
            catch (Exception ex)
            {
                // result = new T();
                //throw;//x
            }
            return us_Adrress;

        }

        //public async Task<User> GetSavedUserFromLocal()
        //{
        //    try
        //    {
        //        return await BlobCache.LocalMachine.GetObject<User>("customer");
        //    }
        //    catch (KeyNotFoundException ke)
        //    {
        //        return null;
        //    }
        //    catch (Exception)
        //    {
        //        return null;
        //        //throw;//x
        //    }
        //}

        public async Task<bool> SaveUserToLocal(User user_)
        {
            return await SaveUserData(user_);
        }

        public async Task<User> PutUser(User user)
        {
            User us = new User();
            try
            {
                // string tokenz = await GetAccessToken();
                string url_ = $"/api/customer/account/{user.Id}";
                string listJson = await SerializeObject(user);
                //string newStr = "{\"customer\":" + listJson + "}";
                string respons = await PutStringAsync(user.AccessToken, url_, listJson);

                if (respons.ToLower().Contains("update"))
                {
                    us = user;
                }
                
            }
            catch (Exception)
            {

                //throw;//x
            }
            return us;
        }

        public async void Logout(User user)
        {
            try
            {
               // User _currentUser = await GetUserData();
                User NewGuest = await LoginAsGuest(user.AccessToken);
                User _SaveUser = new User();
                _SaveUser.IsGuestUser = true;
                _SaveUser.AccessToken = user.AccessToken;
                _SaveUser.Id = NewGuest.Id;
                _SaveUser.LangID = user.LangID;
                await SaveUserToLocal(_SaveUser);
            }
            catch (Exception)
            {

                //throw;//x
            }
            //ClearUserAccssesToken();
        }


        private User GetUserIdFromJson(string json)
        {
            User user_ = new User();
            try
            {
                JObject jsonObject_ = JObject.Parse(json);
                
                if (jsonObject_["CustomerId"] != null)
                {
                    user_.Id = (int)jsonObject_["CustomerId"];
                }

                if (jsonObject_["errors"] != null)
                {
                    IList<JToken> _Errors = jsonObject_["errors"]["page"]?.Children().ToList();
                    string msg_ = "";
                    foreach (var E_item in _Errors)
                    {
                        msg_ = msg_ + "\n" + E_item.ToString();
                    }
                    user_.RespondsMessage = msg_;
                }
            }
            catch (Exception ex)
            {
                return user_;
            }
            return user_;
        }

        public async Task<User> Signup(string accessToken, User user)
        {
            try
            {
               // string tokenz = await GetAccessToken();
                string listJson = await SerializeObject(user);
               // string newStr = "{\"customer\":" + listJson + "}";
                string respons = await PostStringAsync(accessToken, "/api/customer/account/register", listJson);
                if (respons.Equals("\"Valid Email\""))
                {
                    user.Id = -1;// not active
                }
                else
                {
                    User Ruser_ = GetUserIdFromJson(respons);
                    user.Id = Ruser_.Id;
                    user.RespondsMessage = Ruser_.RespondsMessage;
                }
               
                
                //else
                //{
                //    //User loginUser_ = new User();
                //    //loginUser_.Email = user.Email;
                //    //loginUser_.Password = user.Password;

                //    User LoginResponds = await Login(user.AccessToken , user);
                //    if (LoginResponds.Id > 0)
                //    {
                //        us = LoginResponds;
                //    }
                //}
            }
            catch 
            {
                return new User();
            }
            return user;
        }

        public async Task<User> Login(string accessToken, User user)
        {
            User us = new User();
            try
            {

              //  string tokenz = await GetAccessToken();
                 string listJson = "{ \"email\": \"" + user.Email + "\",\"password\": \"" +
                    user.Password  + "\",\"GuestId\": \"" + user.Id + "\"}";

                string respons = await PostStringAsync(accessToken, "/api/customer/account/login", listJson);

                if (!string.IsNullOrEmpty(respons))
                {
                    User Ruser_ = GetUserIdFromJson(respons);
                    user.Id = Ruser_.Id;
                    user.RespondsMessage = Ruser_.RespondsMessage;
                    us = user;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return us;
        }

        public async Task<User> LoginAsGuest(string accessToken)
        {
            User us = new User();
            try
            {
                //string tokenz = await GetAccessToken();
                
                string respons = await PostStringAsync(accessToken, "/api/customer/account/loginGuest", "");

                if (!string.IsNullOrEmpty(respons))
                {
                    User Ruser_ = GetUserIdFromJson(respons);
                    us.Id = Ruser_.Id;
                    us.RespondsMessage = Ruser_.RespondsMessage;
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return us;
        }

        public async Task<User> GetSavedUser()
        {
            return await GetUserData();
        }


        public async Task<string> GetUserAccessToken()
        {
            return await GetNewAccessToken();
        }

        public async Task<bool> DeleteUserAddress(User user, int adressId)
        {
            try
            {
                string url_ = $"/api/customers/{user.Id}/address/{adressId}";

                string respons = await DeleteAsync(user.AccessToken, url_);

                if (respons.ToLower().Contains("deleted"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch 
            {
                return false;
            }
        }

        public async Task<UserAdress> AddUserAddress(User user, UserAdress adress)
        {
            try
            {
                string url_ = $"/api/customers/{user.Id}/address";
                string data_ = await SerializeObject(adress);
                string respons = await PostStringAsync(user.AccessToken, url_, data_);

                int addressId = GetUserAddressIdFromJson(respons);
                if (addressId == 0 )
                {
                    return new UserAdress();
                }
                else
                {
                    adress.Id = addressId;
                    return adress;
                }
            }
            catch
            {
                return new UserAdress();
            }
        }

        private int GetUserAddressIdFromJson(string json)
        {
            int id_ = 0;
            try
            {
                if (json.ToLower().Contains("errors"))
                {
                    return id_;
                }
                JObject jsonObject_ = JObject.Parse(json);
                id_ = (int)jsonObject_["CustomerAddressId"];
            }
            catch
            {
                return id_;
            }
            return id_;
        }

        public async Task<bool> UpdateUserAddress(User user, UserAdress adress)
        {
            try
            {
                string url_ = $"/api/customers/{user.Id}/address/{adress.Id}";
                string data_ = await SerializeObject(adress);
                string respons = await PutStringAsync(user.AccessToken, url_, data_);

                if (respons.Contains("Address Updated"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }

        public async Task<Response> ChangeUserPassword(User user, UserPassword password)
        {
            Response resObj = new Response();
            try
            {
                string url_ = "/api/customer/account/changePassword";
                string data_ = await SerializeObject(password);
               
                string respons = await PostStringAsync(user.AccessToken, url_, data_);
                
                if (respons.Contains("Password was changed"))
                {
                    resObj.Ok = true;
                }
                else
                {
                    string msg_ = GetErrorMessage(respons);
                    resObj.Message = msg_;
                }
            }
            catch
            {
                //return false;
            }
            return resObj;
        }

        public async Task<Response> RecoverUserPassword(User user)
        {
            Response resObj = new Response();
            try
            {
                string url_ = $"/api/customer/account/passwordRecovery?email={user.Email}&languageId={user.LangID}";
               
                string respons = await PostStringAsync(user.AccessToken, url_, "{}");

                if (respons.Contains("Email with instructions has been sent to you."))
                {
                    resObj.Ok = true;
                }
                else
                {
                    string msg_ = GetErrorMessage(respons);
                    resObj.Message = msg_;
                }
            }
            catch
            {
                //return false;
            }
            return resObj;
        }

        public async Task<Response> RecoverUserPassword(User user, UserPassword password)
        {
            Response resObj = new Response();
            try
            {
                string url_ = "/api/customer/account/passwordRecoveryConfirm";
                string data_ = await SerializeObject(password);
                string respons = await PostStringAsync(user.AccessToken, url_, data_);

                if (respons.Contains("Your password has been changed"))
                {
                    resObj.Ok = true;
                }
                else
                {
                    string msg_ = GetErrorMessage(respons);
                    resObj.Message = msg_;
                }
            }
            catch
            {
                //return false;
            }
            return resObj;
        }

        private string GetErrorMessage(string json)
        {
            string msg = "";
            try
            {
                if (string.IsNullOrEmpty(json))
                {
                    return msg;
                }
                JObject jsonObject_ = JObject.Parse(json);
                if (jsonObject_["errors"] != null)
                {
                    IList<JToken> _Errors = new List<JToken>();

                    if (jsonObject_["errors"]["page"] != null)
                    {
                        _Errors = jsonObject_["errors"]["page"].Children().ToList();
                    }
                    else if (jsonObject_["errors"]["customer"] != null)
                    {
                        _Errors = jsonObject_["errors"]["customer"].Children().ToList();
                    }
                    foreach (var E_item in _Errors)
                    {
                        msg = msg + "\n" + E_item.ToString();
                    }
                }
            }
            catch 
            {

            }
            return msg;
        }
    }
}
