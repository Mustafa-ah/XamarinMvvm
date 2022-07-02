using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class User : BaseModel
    {
       
        public int Id { get; set; }
        public bool IsGuestUser { get; set; }

        public string First_name { get; set; }
        public string Last_name { get; set; }

        
        public string Phone { get; set; }

        public string Email { get; set; }

        
        public string AccessToken { get; set; }
        public string LangID { get; set; }

        public string Username { get; set; }


        public string Password { get; set; }
        public string Gender { get; set; }



        public int? DateOfBirthDay { get; set; }
        public int? DateOfBirthMonth { get; set; }
        public int? DateOfBirthYear { get; set; }

        public int? Registered_in_store_id { get; set; }
        public bool AcceptPrivacyPolicyEnabled { get; set; }

        
        public UserAdress Billing_address { get; set; }

        [JsonIgnore]
        public UserAdress Shipping_address { get; set; }

        [JsonIgnore]
        public List<UserAdress> Addresses { get; set; }

        [JsonIgnore]
        public string RespondsMessage { get; set; }
    }
}
