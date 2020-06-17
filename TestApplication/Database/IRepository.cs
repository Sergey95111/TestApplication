using System;
using System.Collections.Generic;
using System.Text;
using TestApplication.Model;

namespace TestApplication.Database
{
    interface IRepository
    {
        public IEnumerable<CountryViewModel> GetCountryViewModels();
        public CountryViewModel AddViewModel(CountryViewModel viewModel);
        //public Country AddCountry(Country country);
        //public City AddCity(City city);
        //public Region AddRegion(Region region);
    }
}
