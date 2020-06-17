using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TestApplication.Model;

namespace TestApplication.Database
{
    public class DataDbContext : DbContext
    {
        public DataDbContext() : base() { }
        public DataDbContext(DbContextOptions<DataDbContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            try
            {
                JsonConfigurationSource jsonConfigSource = new JsonConfigurationSource();
                jsonConfigSource.FileProvider = new PhysicalFileProvider(Path.GetFullPath(@"..\..\..\"));
                jsonConfigSource.Optional = false;
                jsonConfigSource.Path = @"appsettings.json";
                IConfigurationBuilder configBuilder = new ConfigurationBuilder();
                configBuilder.Add(jsonConfigSource);
                IConfiguration config = configBuilder.Build();
                optionsBuilder.UseSqlServer(config["ConnectionString"]);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("Не удалось подключиться к базе данных, пожалуйста проверьте строку подключения!");
            }
        }

        public DbSet<City> Cities { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Region> Regions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>().HasKey(city => city.Id);
            modelBuilder.Entity<City>().HasIndex(city => city.Name).IsUnique();
            modelBuilder.Entity<Country>().HasKey(country => country.Id);
            modelBuilder.Entity<Country>().HasIndex(country => country.Code).IsUnique();
            modelBuilder.Entity<Region>().HasKey(region => region.Id);
            modelBuilder.Entity<Region>().HasIndex(region => region.Name).IsUnique();

            modelBuilder.Entity<Country>()
                .HasOne(country => country.Capital)
                .WithOne(city => city.Country).HasForeignKey("Country", "CapitalId")
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Country>()
                .HasOne(country => country.Region)
                .WithMany(region => region.Countries).HasForeignKey(country => country.RegionId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
