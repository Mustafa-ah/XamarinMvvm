using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Gender : BaseModel
    {
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public string Id { get; set; }
        public Gender(string type, string _id)
        {
            Name = type;
            Id = _id;
            IsSelected = false;
        }
    }
}
