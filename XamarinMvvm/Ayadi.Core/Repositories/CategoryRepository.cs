using Ayadi.Core.Contracts.Repository;
using Akavache;
using System.Reactive.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using MvvmCross.Platform;
using Newtonsoft.Json.Linq;

namespace Ayadi.Core.Repositories
{
    class CategoryRepository : BaseRepository, ICategoryRepository
    {
        // User _user;
        // string _accessToken;
        //public CategoryRepository()
        //{
        //    _user = new User();

        //}
        //public async Task<List<Category>> GetAllCategories()
        //{
        //    string token = "gMbBa1Q4aXiVjj5Oz0SQgfB-M3e-qRjyMZIE5VMtg91BcpK1ASrJ9YPILjtjqw8CqN6d_soNgEqQE8kt4I1C6hrPOmK1cygREv_S6_LCSygtAVknmTCHYxVts-vEON-4VajaGTdRlCWyTZ9eizeCKm8r_thOXmyryqz0qoc50XZLGkVvjcvm49jadqqWYTzP-j0MEIoi1ZUoI7WGuvV2q-HisYvkYDzEZBfPF11_S7qSOtbqFb_Es3rznW60UX2MdFt7EgYydEx8tC6RrYy5gQ";
        //    return await GetAsync<List<Category>>("/api/categories", token);
        //}

        public async Task<List<Category>> GetAllCategoriesFromAPI(User user)
        {
            List<Category> lll = new List<Category>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Category());

                string url_ = $"/api/categories?{ObjFields}&languageId={user.LangID}";
                lll = await GetCategoriesList(url_, user.AccessToken);
                SaveAllCategoriesToLocal(lll);

            }
            catch (Exception ex)
            {

            }
            return lll;
        }
        public async Task<List<Category>> GetAllCategories(User user)
        {
            List<Category> lll = new List<Category>();
            try
            {
                List<Category> savedList = await GetAllCategoriesFromLocal();
                if (savedList == null || savedList.Count == 0)
                {
                    lll = await GetAllCategoriesFromAPI(user);
                }
                else
                {
                    lll = savedList;
                }
                
            }
            catch (Exception ex)
            {

            }
            return lll;
           // // _user = await GetUserData();
           // string tokenz = await GetAccessToken();
           // //if (_user.AccessToken == null)
           // //{
           // //    tokenz = await GetNewAccessToken();
           // //    _user.Name = "User NAme";
           // //    _user.AccessToken = tokenz;
           // //    SaveUserData(_user);
           // //    Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "From IF ", _user.AccessToken);
           // //}
           // //else
           // //{
           // //    tokenz = _user.AccessToken;
           // //    Mvx.Trace(MvvmCross.Platform.Platform.MvxTraceLevel.Diagnostic, "from Else", _user.AccessToken);
           // //}
           
           // //string wefef = "CTga8RG2-FoqsftF5K92ZNCsh9Q1BEFtlIeSd5dX82n6j8WyIRd0JPPxDvJG4JI95YEddXeodihoBsygUJH5FeiMlmsIiNtu-I_xObnZkvY2_WlxI9jRYy04M-lJoNh4jvuHO632kmJvTUUlZJDrQ-iY4CzaSDfdDnaDu0K2Hdxl_7KQVPeCRMOmZMqAuB_S7c_uqZnhdggK6gLnRmDC9DrqA6lF5BTXj7gVKB8G_NwEPS9qbZdAtE7f_Hd1sE7BITQPyw8jIzUFpvwjIL8LRw";
           // //string token = "mMbBa1Q4aXiVjj5Oz0SQgfB-M3e-qRjyMZIE5VMtg91BcpK1ASrJ9YPILjtjqw8CqN6d_soNgEqQE8kt4I1C6hrPOmK1cygREv_S6_LCSygtAVknmTCHYxVts-vEON-4VajaGTdRlCWyTZ9eizeCKm8r_thOXmyryqz0qoc50XZLGkVvjcvm49jadqqWYTzP-j0MEIoi1ZUoI7WGuvV2q-HisYvkYDzEZBfPF11_S7qSOtbqFb_Es3rznW60UX2MdFt7EgYydEx8tC6RrYy5gQ";
           //// List<Category> dd = await GetlAsync("/api/categories", tokenz);
           // List<Category> dd = await GetArticles("/api/categories", tokenz);
           // return dd;
        }

        public async Task<List<Category>> GetHomeCategoriesFromAPI(User user)
        {
            List<Category> lll = new List<Category>();
            try
            {
                string ObjFields = await GetURLFieldsNames(new Category());
                string url_ = $"/api/categories?limit=9&{ObjFields}&languageId={user.LangID}";
                lll = await GetCategoriesList(url_, user.AccessToken);
                SaveHomeCategoriesToLocal(lll);
            }
            catch (Exception)
            {

            }
            return lll;
        }

        public async Task<List<Category>> GetHomeCategories(User user)
        {
            List<Category> lll = new List<Category>();
            try
            {
                List<Category> savedList = await GetHomeCategoriesFromLocal();
                if (savedList == null || savedList.Count == 0)
                {
                    lll = await GetHomeCategoriesFromAPI(user);
                }
                else
                {
                    lll = savedList;
                }
            }
            catch (Exception)
            {

            }
            return lll;
        }

        private async Task<List<Category>> GetCategoriesList(string resource, string token)
        {
            List<Category> lll = new List<Category>();
            try
            {
                string jsonString = await GetStringAsync(resource, token);
                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["categories"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["categories"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Category cat = await DeserializeTObject<Category>(itemList.ToString());
                    if (cat != null)
                    {
                        // localization
                        if (cat.Localized_names.Count > 0)
                        {
                            cat.Name = cat.Localized_names[0].Localized_name;
                            //if (!string.IsNullOrEmpty(cat.Localized_names[0].Localized_name))
                            //{
                            //    cat.Name = cat.Localized_names[0].Localized_name;
                            //}
                        }
                        lll.Add(cat);
                    }
                    
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

        //

        // local db......................................................

        private async Task<List<Category>> GetHomeCategoriesFromLocal()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<List<Category>>(Constants.HomeCategoriesKey);
            }
            catch (KeyNotFoundException ke)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
                //throw;//x
            }
        }

        private async void SaveHomeCategoriesToLocal(List<Category> ls)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject<List<Category>>(Constants.HomeCategoriesKey, ls);
            }
            catch
            {
                //throw;//x
            }
        }

        private async Task<List<Category>> GetAllCategoriesFromLocal()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<List<Category>>(Constants.AllCategoriesKey);
            }
            catch (KeyNotFoundException ke)
            {
                return null;
            }
            catch (Exception)
            {
                return null;
                //throw;//x
            }
        }

        private async void SaveAllCategoriesToLocal(List<Category> ls)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject<List<Category>>(Constants.AllCategoriesKey, ls);
            }
            catch
            {
                //throw;//x
            }
        }
    }
}
