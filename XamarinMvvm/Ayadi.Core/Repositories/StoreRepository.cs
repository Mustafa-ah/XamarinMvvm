using Ayadi.Core.Contracts.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ayadi.Core.Model;
using Newtonsoft.Json.Linq;
using Akavache;
using System.Reactive.Linq;

namespace Ayadi.Core.Repositories
{
    public class StoreRepository : BaseRepository, IStoreRepository
    {
        public async Task<List<Store>> GetAllStorsFromAPI(User user)
        {
            try
            {
                string url_ = $"/api/vendors/active?languageId={user.LangID}";
                List<Store> lst = await GetStorsList(url_, user.AccessToken);
                SaveStorsToLocal(lst);
                return lst;
            }
            catch (Exception)
            {
                return new List<Store>();
            }
        }

        private async Task<List<Store>> GetAllStorsFromLocal()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<List<Store>>(Constants.StorsKey);
            }
            catch
            {
                return new List<Store>();
            }
        }

        private async void SaveStorsToLocal(List<Store> StorsLst)
        {
            await BlobCache.LocalMachine.InsertObject<List<Store>>(Constants.StorsKey, StorsLst);
        }

        public async Task<List<Store>> GetAllStors(User user)
        {
            List<Store> storsList = new List<Store>();
            try
            {
                storsList = await GetAllStorsFromLocal();
                if (storsList == null || storsList.Count == 0)
                {
                    storsList = await GetAllStorsFromAPI(user);
                }
            }
            catch (Exception)
            {
                //throw;//x
            }
            return storsList;
        }

        public async Task<Store> GetStoreById(string id, User user)
        {
            Store sto = new Store();
            try
            {
                string url_ = $"/api/vendors/{id}?languageId={user.LangID}";
                List<Store> stList = await GetStorsList(url_, user.AccessToken);
                //string jsonString = await GetStringAsync("/api/vendors/" + id, tokenz);
                //JObject jsonObject_ = JObject.Parse(jsonString);
                //IList<JToken> _itemList = jsonObject_["vendors"].Children().ToList();
                //sto = await DeserializeTObject<Store>(_itemList[0].ToString());
                if (stList.Count > 0)
                {
                    sto = stList[0];
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return sto;
        }

        public async Task<Store> GetStoreByName(string name, User user)
        {
            Store sto = new Store();
            try
            {
                string url_ = $"/api/vendors?vendor_name={name}&languageId={user.LangID}";
                List<Store> stList = await GetStorsList(url_, user.AccessToken);
                //string jsonString = await GetStringAsync("/api/vendors?vendor_name=" + name, tokenz);
                //JObject jsonObject_ = JObject.Parse(jsonString);
                //IList<JToken> _itemList = jsonObject_["vendors"].Children().ToList();
                //sto = await DeserializeTObject<Store>(_itemList[0].ToString());
                if (stList.Count > 0)
                {
                    sto = stList[0];
                }
            }
            catch (Exception)
            {

                //throw;//x
            }
            return sto;
        }

        public async Task<List<Store>> GetStorsNamesByIds(int[] ids, User user)
        {
            List<Store> lll = new List<Store>();
            try
            {
                string IdsUrl = "?ids=";
                if (ids == null || ids.Length == 0)
                {
                    return lll;
                }
                int[] ids_ = ids.Distinct().ToArray();
                for (int i = 0; i < ids_.Length; i++)
                {
                    IdsUrl = IdsUrl + $"{ids_[i]}%2C";
                }
                string url_ = $"/api/vendors{IdsUrl}&fields=Id%2CName&languageId={user.LangID}";
                //lll = await GetStorsList("/api/vendors" + IdsUrl + "&fields=Id%2CName", tokenz);
                lll = await GetStorsList(url_, user.AccessToken);
            }
            catch (Exception)
            {
                //throw;//x
            }
            return lll;
        }

        private async Task<List<Store>> GetStorsList(string resource, string token)
        {
            List<Store> lll = new List<Store>();
            try
            {
                string jsonString = await GetStringAsync(resource, token);
                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["vendors"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["vendors"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    Store stor = await DeserializeTObject<Store>(itemList.ToString());
                    if (stor != null)
                    {
                        if (stor.Localized_Vendor.Count > 0)
                        {
                            stor.SearcName = stor.Name;
                            stor.Name = stor.Localized_Vendor[0].Localized_name;
                            stor.Description = stor.Localized_Vendor[0].Localized_Description;
                        }
                        lll.Add(stor);
                    }

                }

            }
            catch (Exception ex)
            {
                // result = new T();
            }
            return lll;
        }
    }
}
