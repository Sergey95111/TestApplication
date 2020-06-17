using System.Collections.Generic;

namespace TestApplication.Model
{
    public class Region
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public IList<Country> Countries { get; set; }
    }
}
