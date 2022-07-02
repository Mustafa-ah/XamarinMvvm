using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;

using System.Collections.Generic;
using RestSharp.Portable.HttpClient;
using RestSharp.Portable;
using Newtonsoft.Json.Linq;
using System.Linq;
using Ayadi.Core.Model;
using System;
using Akavache;
using System.Reactive.Linq;
using System.Text;
using Newtonsoft.Json.Serialization;
using MvvmCross.Platform;

namespace Ayadi.Core.Repositories
{
    public class BaseRepository
    {

          private string url = "http://xyz.com";
        

        //// new  online
        //private string client_id = "a71c62ad-a53f-4975-8058-cd3a37011d8e";
        //private string client_secret = "ebb43438-a075-4897-a7b5-d2acb08334d0";

        // new  token
        private string client_id = "57a361be-c16d-49e4-ba33-682ef46d9ea5";
        private string client_secret = "388f7fd0-6e37-465e-807d-958aef022c83";

        // old local
        //private string client_id = "ef5026b5-4a9d-4a1d-aa48-f47e869ae744";
        //private string client_secret = "505cdc23-c775-4772-8e96-36786f73abc2";

        User _user;

        private HttpClient CreateHttpClient()
        {
            var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return httpClient;
        }

        protected async Task<T> GetAsync<T>(string url)
            where T : new()
        {
            HttpClient httpClient = CreateHttpClient();
            T result;

            try
            {
                var response = await httpClient.GetStringAsync(url);
                result = await Task.Run(() => result = JsonConvert.DeserializeObject<T>(response));
            }
            catch
            {
                result = new T();
            }

            return result;
        }

        protected async Task<string> GetAccessToken()
        {
            _user = await GetUserData();
            string tokenz = "";
            if (_user.AccessToken == null)
            {
                tokenz = await GetNewAccessToken();
                _user.AccessToken = tokenz;
                await SaveUserData(_user);
              // Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "From IF ", _user.AccessToken);
            }
            else
            {
                tokenz = _user.AccessToken;
               // Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "from Else", _user.AccessToken);
            }
            return tokenz;
        }

        internal async Task<string> GetNewAccessToken()
        {
            try
            {
                Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "start GetNewAccessToken0.....................................");
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    //https://social.msdn.microsoft.com/Forums/en-US/e62b89f9-f15d-457b-af07-98a41b7d729f/exception-in-httpresponsemessage-for-async-post-method?forum=winappswithcsharp
                   // client.DefaultRequestHeaders.ExpectContinue = false;
                    // We want the response to be JSON.
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                   // client.DefaultRequestHeaders.Host = _domain;
                    //client.DefaultRequestHeaders.Accept.Add(new ContentDispositionHeaderValue("application/json"));
                    //client.DefaultRequestHeaders.Add("Content-Type", "application/x-www-form-urlencoded");
                    var query = "/oauth/authorize" + "?client_id=" + client_id + "& client_secret=" + client_secret + "&response_type=code";
                    HttpResponseMessage codeResponse = await client.GetAsync(query);

                    var code = codeResponse.RequestMessage.RequestUri.Query.Replace("?code=", "");

                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // Build up the data to POST.
                    List<KeyValuePair<string, string>> postData = new List<KeyValuePair<string, string>>();
                    // postData.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
                    postData.Add(new KeyValuePair<string, string>("grant_type", "authorization_code"));
                    postData.Add(new KeyValuePair<string, string>("client_id", client_id));
                    postData.Add(new KeyValuePair<string, string>("client_secret", client_secret));

                    postData.Add(new KeyValuePair<string, string>("Content-Type", "application/x-www-form-urlencoded"));

                    postData.Add(new KeyValuePair<string, string>("code", code));

                    FormUrlEncodedContent content = new FormUrlEncodedContent(postData);

                    // Post to the Server and parse the response.
                    // HttpResponseMessage response = await client.PostAsync("Token", content);
                    // HttpResponseMessage response = await client.PostAsync("/oauth/authorize", content);
                    HttpResponseMessage response = await client.PostAsync("/api/token", content);
                    string jsonString = await response.Content.ReadAsStringAsync();
                    object responseData = JsonConvert.DeserializeObject(jsonString);

                    //var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsonString)["access_token"].ToString();
                    Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "End GetNewAccessToken0.....................................");
                    // return the Access Token.
                    string ddddd = ((dynamic)responseData).access_token;
                    System.Diagnostics.Debug.WriteLine(ddddd);
                    return ((dynamic)responseData).access_token;
                }
            }
            catch (Exception ex)
            {
                string fef = ex.Message;
                return null;
            }
           
        }

        public async Task<string> GetStringAsync(string resource, string token)
        {
         //   Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Error, "...................resource start..............." + resource);
            string Respons = "";
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetNewAccessToken();
                }
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 //   client.DefaultRequestHeaders.Host = _domain;
                    // Add the Authorization header with the AccessToken.
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // create the URL string.
                    string urlfull = string.Concat(url, resource);

                    // make the request
                    HttpResponseMessage response = await client.GetAsync(urlfull);

                    // parse the response and return the data.
                    Respons = await response.Content.ReadAsStringAsync();

                    if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        ClearUserAccssesToken();
                       // Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Error, ".................................HttpStatusCode.Unauthorized.................");
                    }
                }
            }
            catch (Exception ex)
            {
                Respons = null;
            }
            
            // Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, Respons);
            return Respons;
        }

        public async Task<string> PostStringAsync(string token, string resource, string data)
        {
            string Respons = "";
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetNewAccessToken();
                }

                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                   // client.DefaultRequestHeaders.ExpectContinue = false;
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 //   client.DefaultRequestHeaders.Host = _domain;
                    // Add the Authorization header with the AccessToken.
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // create the URL string.
                    string urlfull = string.Concat(url, resource);

                    // make the request
                    HttpResponseMessage response = await client.PostAsync(urlfull, content);

                    // parse the response and return the data.
                    Respons = await response.Content.ReadAsStringAsync();
                    Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, Respons);
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return Respons;
        }

        public async Task<string> PutStringAsync(string token, string resource, string data)
        {
            string Respons = "";
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetNewAccessToken();
                }
                HttpContent content = new StringContent(data, Encoding.UTF8, "application/json");
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                 //   client.DefaultRequestHeaders.Host = _domain;
                    // Add the Authorization header with the AccessToken.
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // create the URL string.
                    string urlfull = string.Concat(url, resource);

                    // make the request
                    HttpResponseMessage response = await client.PutAsync(urlfull, content);

                    // parse the response and return the data.
                    Respons = await response.Content.ReadAsStringAsync();
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return Respons;
        }

        public async Task<string> DeleteAsync(string token, string resource)
        {
            string Respons = "";
            try
            {
                if (string.IsNullOrEmpty(token))
                {
                    token = await GetNewAccessToken();
                }
                using (HttpClient client = new HttpClient())
                {
                    client.BaseAddress = new Uri(url);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                  //  client.DefaultRequestHeaders.Host = _domain;
                    // Add the Authorization header with the AccessToken.
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                    // create the URL string.
                    string urlfull = string.Concat(url, resource);

                    // make the request
                    HttpResponseMessage response = await client.DeleteAsync(urlfull);

                    // parse the response and return the data.
                    Respons = await response.Content.ReadAsStringAsync();
                    Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, " ----------------> " + Respons);
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return Respons;
        }

        #region Old Client Api
        /*
         * 
         *    
         public string AuthorizeClient()
         {
            var restclient = new RestClient(url);
            RestRequest request = new RestRequest("/oauth/authorize") { Method = Method.GET };
            request.AddHeader("Accept", "application/json");
            request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
            request.AddParameter("client_id", client_id);
            request.AddParameter("client_secret", client_secret);
            request.AddParameter("response_type", "code");
            var tResponse = restclient.Execute(request);
            // var code = tResponse.ResponseUri.Query.Replace("?code=", "");
            //var code = tResponse.Result.ResponseUri.Query.Replace("?code=", "");***

            request = new RestRequest("/api/token") { Method = Method.POST };
             request.AddHeader("Accept", "application/json");
             request.AddHeader("Content-Type", "application/x-www-form-urlencoded");
             request.AddParameter("client_id", client_id);
             request.AddParameter("client_secret", client_secret);
            // request.AddParameter("code", code); ***
             request.AddParameter("grant_type", "authorization_code");
             tResponse = restclient.Execute(request);
             var responseJson = tResponse.Result.Content;
             var token = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson)["access_token"].ToString();

             return token;
         }

          public async Task<List<Category>> GetArticles(string resource, string token)
        {
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
              //  client.DefaultRequestHeaders.Host = _domain;
                // Add the Authorization header with the AccessToken.
                client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);

                // create the URL string.
                string urlf = string.Concat(url, resource);

                // make the request
                HttpResponseMessage response = await client.GetAsync(urlf);

                // parse the response and return the data.
                string jsonString = await response.Content.ReadAsStringAsync();

                List<Category> lll = new List<Category>();
                try
                {
                    JObject jsonObject_ = JObject.Parse(jsonString);
                    IList<JToken> _itemList = jsonObject_["categories"].Children().ToList();
                    foreach (JToken itemList in _itemList)
                    {
                        Category dd = JsonConvert.DeserializeObject<Category>(itemList.ToString());
                        lll.Add(dd);
                    }

                }
                catch (Exception ex)
                {
                    // result = new T();
                }

                // object responseData = JsonConvert.DeserializeObject(jsonString);
                //return (dynamic)responseData;
                return lll;
            }
        }

        public Dictionary<string, object> Post(string resource, string token, Dictionary<string, string> headers = default(Dictionary<string, string>), Dictionary<string, string> parameters = default(Dictionary<string, string>), object body = null)
         {
             var restclient = new RestClient(url);
             RestRequest request = new RestRequest(resource) { Method = Method.POST };
             // request.RequestFormat = DataFormat.Json;***
             request.Serializer.ContentType = "application/json";

             request.AddHeader("Authorization", string.Format("Bearer {0}", token));
             request.AddHeader("Accept", "application/json");
             //request.AddHeader("Content-Type", "application/x-www-form-urlencoded");

             if (body != null)
                 request.AddParameter("application/json", body, ParameterType.RequestBody); //request.AddBody(body);
             if (headers != default(Dictionary<string, string>))
             {
                 foreach (var item in headers)
                 {
                     request.AddHeader(item.Key, item.Value);
                 }
             }
             if (parameters != default(Dictionary<string, string>))
             {
                 foreach (var item in parameters)
                 {
                     request.AddHeader(item.Key, item.Value);
                 }
             }
             var tResponse = restclient.Execute(request);
           //  var responseJson = tResponse.Content;***
             var responseJson = tResponse.Result.Content;
             var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
             return result;
         }

         public Dictionary<string, object> Get(string resource, string token, Dictionary<string, string> headers = default(Dictionary<string, string>), Dictionary<string, string> parameters = default(Dictionary<string, string>))
         {
             var restclient = new RestClient(url);
             RestRequest request = new RestRequest(resource) { Method = Method.GET };
             request.AddHeader("Authorization", string.Format("Bearer {0}", token));
             if (headers != default(Dictionary<string, string>))
             {
                 foreach (var item in headers)
                 {
                     request.AddHeader(item.Key, item.Value);
                 }
             }
             if (parameters != default(Dictionary<string, string>))
             {
                 foreach (var item in parameters)
                 {
                     request.AddHeader(item.Key, item.Value);
                 }
             }
             var tResponse = restclient.Execute(request);
             //var responseJson = tResponse.Content;***
             var responseJson = tResponse.Result.Content;
             var result = JsonConvert.DeserializeObject<Dictionary<string, object>>(responseJson);
             return result;
         }

             */
        #endregion



        protected async Task<bool> SaveUserData(User user)
        {
            try
            {
                //byte[] toBytes = Encoding.UTF8.GetBytes(tokenz);
                //if (!string.IsNullOrEmpty(user.AccessToken))
                //{
                //    var result = await
                //BlobCache.LocalMachine.InsertObject(Constants.UserKey, user);
                //}
                var result = await
               BlobCache.LocalMachine.InsertObject(Constants.UserKey, user);
                return true;
            }
            catch 
            {
                return false;
            }
            
        }

        protected async Task<User> GetUserData()
        {
            User us;
            try
            {
                us = await BlobCache.LocalMachine.GetObject<User>(Constants.UserKey);
                //if (string.IsNullOrEmpty(us.AccessToken))
                //{
                //    us = new User();
                //}
            }
            catch (KeyNotFoundException ke)
            {
                us = new User();
            }
            catch (Exception dd)
            {
                us = new User();
            }
            return us;
        }

        public async Task<T> DeserializeTObject<T>(string ObjString)
             where T : new()
        {
            T Obj = new T();
            try
            {
                await Task.Run(() => Obj = JsonConvert.DeserializeObject<T>(ObjString));
               // Obj = JsonConvert.DeserializeObject<T>(ObjString);
            }
            catch
            {
                //Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Error, ex.Message);
                Obj = default(T);
            }
            return Obj;
        }

        public async Task<string> SerializeObject(object obj)
        {
            string ObjJson = "";
            try
            {
                await Task.Run(() => {
                    ObjJson = JsonConvert.SerializeObject(obj, new JsonSerializerSettings
                    {
                        ContractResolver = new CamelCasePropertyNamesContractResolver()
                    });
                });
                
            }
            catch
            {
                ObjJson = "";
            }
            return ObjJson;
        }

        public async void ClearUserAccssesToken()
        {
            try
            {
                User savedUser_ = await GetUserData();
                savedUser_.AccessToken = null;
                bool sav_ = await SaveUserData(savedUser_);
              //  BlobCache.LocalMachine.Invalidate(Constants.UserKey);
            }
            catch (Exception)
            {

                //throw;//x
            }
        }

        public async Task<string> GetURLFieldsNames(object obj)
        {
            string reslt = "fields=";
            try
            {
                string serilizedObj = await SerializeObject(obj);

                JObject jsonObject_ = JObject.Parse(serilizedObj);

                var props = jsonObject_.Properties();

                foreach (var item in props)
                {
                    reslt = reslt + item.Name + ",";
                }

                // remove last comma
                reslt = reslt.Remove(reslt.Length -1 ,1);
            }
            catch (Exception)
            {

                //throw;//x
            }
            return reslt;
        }
    }
}
