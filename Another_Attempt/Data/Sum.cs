using Another_Attempt.Book_Info;
using Another_Attempt.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Attempt.Data
{
    public static class Data
    {
        public static void Initialize(LibraryContext context)
        {
            context.Database.EnsureCreated();
            if (context.Books.Any())
            {
                return;
            }

            context.Books.AddRange(
                new Book { Title = "Under the Yoke", Author = "Ivan Vazov", Year = 1893, TotalCopies = 30, AvailableCopies = 30 },
                new Book { Title = "Bay Ganyo", Author = "Aleko Konstantinov", Year = 1895, TotalCopies = 25, AvailableCopies = 25 },
                new Book { Title = "Tobacco", Author = "Dimitar Dimov", Year = 1951, TotalCopies = 22, AvailableCopies = 22 },
                 new Book { Title = "Epic of the Forgotten", Author = "Ivan Vazov", Year = 1893, TotalCopies = 19, AvailableCopies = 19 },
                new Book { Title = "Unkind-unloved", Author = "Ivan Vazov", Year = 1883, TotalCopies = 48, AvailableCopies = 48 },
                new Book { Title = "Uncles", Author = "Ivan Vazov", Year = 1885, TotalCopies = 10, AvailableCopies = 10 },
                 new Book { Title = "The Volunteers at Shipka", Author = "Ivan Vazov", Year = 1883, TotalCopies = 38, AvailableCopies = 38 },
                new Book { Title = "Hitar Petar: Tales", Author = "Sava Popov", Year = 1958, TotalCopies = 40, AvailableCopies = 40 },
                new Book { Title = "Noah's Ark", Author = "Yordan Radichkov", Year = 1988, TotalCopies = 21, AvailableCopies = 21 },
                 new Book { Title = "Diary of a Wimpy Kid", Author = "Jeff Kinny", Year = 2007, TotalCopies = 50, AvailableCopies = 50 },
                new Book { Title = "Top bulgarian mysteries", Author = "Slavi Panayotov", Year = 2018, TotalCopies = 40, AvailableCopies = 40 },
                new Book { Title = "Jujutsu Kaisen", Author = "Gege Akutami", Year = 2018, TotalCopies = 33, AvailableCopies = 33 }

            );

            context.Clients.AddRange(
                new Client { FirstName = "Mike", LastName = "Oxlong" },
                new Client { FirstName = "John", LastName = "Biden" },
                new Client { FirstName = "Gosho", LastName = "Gerganov" },
                new Client { FirstName = "Mitko", LastName = "Ivanov" },
                new Client { FirstName = "Kaloyan", LastName = "Ganev" },
                new Client { FirstName = "Viktor", LastName = "Grigorov" },
                new Client { FirstName = "Dimitar", LastName = "Dimitrov" },
                new Client { FirstName = "Petur", LastName = "Cheren" },
                new Client { FirstName = "Don", LastName = "Bueno" },
                new Client { FirstName = "Peter", LastName = "Griffin" },
                new Client { FirstName = "Rick", LastName = "Sanches" },
                new Client { FirstName = "Harry", LastName = "Poter" },
                new Client { FirstName = "Satoru", LastName = "Gojo" },
                new Client { FirstName = "Toji", LastName = "Foshiguro" }
            );

            context.SaveChanges();
        }
    }
}
