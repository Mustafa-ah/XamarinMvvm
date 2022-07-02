using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class UserPassword : BaseModel
    {
        public int CustomerId { get; set; }
        public string Old_password { get; set; }
        public string New_password { get; set; }
        public string Token { get; set; }
        public string Email { get; set; }
    }
}
