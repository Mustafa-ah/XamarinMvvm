namespace Ayadi.Core.Model
{
    public class City : BaseModel
    {
        public int CityId { get; set; }
        public string Name { get; set; }
        public bool IsSelected { get; set; }
        public int GovId { get; set; }
        public City(string name)
        {
            Name = name;
            IsSelected = false;
        }
    }
}
