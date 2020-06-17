using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TestApplication.Model;

namespace TestApplication.Database
{
    public class Repository : IRepository
    {
        public DataDbContext context { get; set; }

        public Repository(DataDbContext ctx)
        {
            context = ctx;
        }

        public IEnumerable<CountryViewModel> GetCountryViewModels()
        {
            return context.Countries.Include(country => country.Capital).Include(country => country.Region).AsNoTracking()
                .Select(country => new CountryViewModel
                {
                    Name = country.Name,
                    NumericCode = country.Code,
                    Capital = country.Capital.Name,
                    Area = country.Area,
                    Population = country.Population,
                    Region = country.Region.Name
                }).ToList();
        }

        public CountryViewModel AddViewModel(CountryViewModel viewModel)
        {
            City capital = context.Cities.Where(city => city.Name == viewModel.Capital).FirstOrDefault();
            if (capital == null)
            {
                capital = new City() { Name = viewModel.Capital };
                context.Cities.Add(capital);
            }
            Region region = context.Regions.Where(region => region.Name == viewModel.Region).FirstOrDefault();
            if (region == null)
            {
                region = new Region() { Name = viewModel.Region };
                context.Regions.Add(region);
            }
            Country country = context.Countries.Where(country => country.Code == viewModel.NumericCode).FirstOrDefault();
            if (country == null)
            {
                country = new Country()
                {
                    Name = viewModel.Name,
                    Code = viewModel.NumericCode,
                    Area = viewModel.Area.HasValue ? viewModel.Area.Value : 0,
                    Population = viewModel.Population.HasValue ? viewModel.Population.Value : 0,
                    Capital = capital,
                    Region = region
                };
                context.Countries.Add(country);
            }
            context.SaveChanges();
            return viewModel;
        }
    }
}
