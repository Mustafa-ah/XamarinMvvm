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
    public class SponserRepository : BaseRepository, ISponserRepository
    {
        public async Task<List<SponserSlider>> GetAllSponsers(User user)
        {
            List<SponserSlider> lll = new List<SponserSlider>();
            try
            {
                List<SponserSlider> savedList = await GetAllSponsersFromLocal();
                if (savedList != null && savedList.Count > 0)
                {
                    lll = savedList;
                }
                else
                {
                    lll = await GetAllSponsersFromApi(user);
                }
            }
            catch (Exception)
            {

            }
            return lll;
        }

        // public to call it from UpdateDataService
        public async Task<List<SponserSlider>> GetAllSponsersFromApi(User user)
        {
            List<SponserSlider> lll = new List<SponserSlider>();
            try
            {
                //  string tokenz = await GetAccessToken();
                lll = await GetSponsersList("/api/sponsorsliders", user.AccessToken);
                SaveHomeSponserToLocal(lll);
            }
            catch (Exception)
            {

            }
            return lll;
        }

        private async Task<List<SponserSlider>> GetAllSponsersFromLocal()
        {
            try
            {
                return await BlobCache.LocalMachine.GetObject<List<SponserSlider>>(Constants.HomeTopSliderKey);
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

        private async void SaveHomeSponserToLocal(List<SponserSlider> ls)
        {
            try
            {
                await BlobCache.LocalMachine.InsertObject<List<SponserSlider>>(Constants.HomeTopSliderKey, ls);
            }
            catch
            {
                //throw;//x
            }
        }

        private async Task<List<SponserSlider>> GetSponsersList(string resource, string token)
        {
            List<SponserSlider> lll = new List<SponserSlider>();
            try
            {
                string jsonString = await GetStringAsync(resource, token);
                JObject jsonObject_ = JObject.Parse(jsonString);
                if (jsonObject_["sponsorSliders"] == null)
                {
                    return lll;
                }
                IList<JToken> _itemList = jsonObject_["sponsorSliders"].Children().ToList();
                foreach (JToken itemList in _itemList)
                {
                    SponserSlider slid = await DeserializeTObject<SponserSlider>(itemList.ToString());
                    if (slid != null)
                    {
                        lll.Add(slid);
                    }
                    //IList<JToken> _imagesList = itemList["Images"].Children().ToList();
                    //foreach (var imageItem in _imagesList)
                    //{
                    //    Sponser cat = DeserializeTObject<Sponser>(imageItem.ToString());
                    //    if (cat != null)
                    //    {
                    //        lll.Add(cat);
                    //    }
                    //}

                    break;
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
}
