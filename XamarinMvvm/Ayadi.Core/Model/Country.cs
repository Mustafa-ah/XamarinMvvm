using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ayadi.Core.Model
{
    public class Country : BaseModel
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public bool IsSelected { get; set; }
        public Country(string name)
        {
            Name = name;
            IsSelected = false;
            Id = 69;
        }
    }
}
