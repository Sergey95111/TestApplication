namespace TestApplication.Model
{
    public class Country
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public double Area { get; set; }
        public int Population { get; set; }

        public long CapitalId { get; set; }
        public City Capital { get; set; }

        public long RegionId { get; set; }
        public Region Region { get; set; }
    }
}
