using System;
using System.Collections.Generic;
using System.Text;

namespace TestApplication.Model
{
    public class City
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }
    }
}
