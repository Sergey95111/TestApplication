using System;
using System.Collections.Generic;
using System.Text;

namespace TestApplication.Model
{
    public class CountryViewModel
    {
        public string Name { get; set; }
        public string NumericCode { get; set; }
        public double? Area { get; set; }
        public int? Population { get; set; }
        public string Capital { get; set; }
        public string Region { get; set; }

        public string GetString()
        {
            return $"Страна: {Name}, Код страны: {NumericCode}, Столица: {Capital}, Площадь: {Area}, Население: {Population}, Регион: {Region}";
        }
    }
}
