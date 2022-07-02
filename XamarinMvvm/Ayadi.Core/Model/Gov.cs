using System.Collections.Generic;

namespace Ayadi.Core.Model
{
    public class Gov : BaseModel
    {
        public int GovId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        List<City> Cities;
        public Gov(string name)
        {
            Name = name;
            IsSelected = false;
        }
    }
}
