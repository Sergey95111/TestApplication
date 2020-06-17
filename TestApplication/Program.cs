using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.Json;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using TestApplication.Database;
using TestApplication.Model;

namespace TestApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            while (true)
            {
                using (DataDbContext context = new DataDbContext())
                {
                    IRepository repository = new Repository(context);
                    Console.WriteLine("Чтобы получить содержимое базы данных введите в командной строке 'database'.");
                    Console.WriteLine("Чтобы получить информацию о стране введите ее название.\n");
                    string command = Console.ReadLine();
                    if (command.ToLower() == "database")
                    {
                        IEnumerable<CountryViewModel> result = repository.GetCountryViewModels();
                        foreach (var item in result)
                            Console.WriteLine(item.GetString());
                        Console.WriteLine();
                    }
                    else
                    {
                        HttpClient client = new HttpClient();
                        try
                        {
                            string result = client.GetStringAsync(String.Format("https://www.restcountries.eu/rest/v2/name/{0}", command)).Result;
                            JsonSerializerOptions options = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
                            CountryViewModel ViewModel = JsonSerializer.Deserialize<List<CountryViewModel>>(result, options).First();
                            Console.WriteLine(ViewModel.GetString());
                            Console.WriteLine("Сохранить информацию о стране в базе данных?");
                            Console.WriteLine(" y / n\n");
                            for (bool answered = false; answered == false;)
                            {
                                ConsoleKeyInfo key = Console.ReadKey();
                                switch (key.KeyChar)
                                {
                                    case 'y':
                                        repository.AddViewModel(ViewModel);
                                        Console.WriteLine();
                                        Console.WriteLine("Данные были сохранены!");
                                        answered = true;
                                        break;
                                    case 'n':
                                        Console.WriteLine();
                                        Console.WriteLine("Данные не были сохранены!");
                                        answered = true;
                                        break;
                                }
                            }
                        }
                        catch (AggregateException ex)
                        {
                            Console.WriteLine("Что-то пошло не так, количество ошибок: {0}, сообщение: {1}", ex.InnerExceptions.Count, ex.Message);
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine(ex.Message);
                        }
                        finally
                        {
                            Console.WriteLine();
                            client.Dispose();
                        }
                    }
                }
            }
        }
    }
}
