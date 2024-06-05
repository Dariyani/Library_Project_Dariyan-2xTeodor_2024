using Another_Attempt.Data;
using Another_Attempt.DbConnection;
using Another_Attempt.Service;
using Microsoft.EntityFrameworkCore;

using (var context = new LibraryContext())
{
    Data.Initialize(context);
    var libraryService = new Services(context);
    Console.WriteLine("  /-------------------------------\\");
            Console.WriteLine($" |                                 |");
            Console.WriteLine(" | Welcome to the library society. |");
            Console.WriteLine(" |                                 |");
            Console.WriteLine("  \\-------------------------------/");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nSelect an option:");
                Console.WriteLine();
                Console.WriteLine("1. List all books");
                Console.WriteLine("2. Show book info");
                Console.WriteLine("3. Take a book");
                Console.WriteLine("4. Return a book");
                Console.WriteLine("5. Check taken books");
                Console.WriteLine("6. Check taken books by a certain client");
                Console.WriteLine("7. Find books by a certain author");
                Console.WriteLine("8. Find books by a certain keyword");
                Console.WriteLine("9. Check all books's history.");
                Console.WriteLine("10. Add Book.");
                Console.WriteLine("11. Exit");
                Console.WriteLine();
                Console.Write("Enter your choice: ");
                Console.WriteLine();
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        libraryService.ListAllBooks();
                        break;
                    case "2":
                        libraryService.ShowBookInfo();
                        break;
                    case "3":
                        libraryService.TakeBook();
                        break;
                    case "4":
                        libraryService.ReturnBook();
                        break;
                    case "5":
                        libraryService.CheckTakenBooks();
                        break;
                    case "6":
                        libraryService.CheckTakenBooksByAClient();
                        break;
                    case "7":
                        libraryService.CheckTitlesByAuthor();
                        break;
                    case "8":
                        libraryService.CheckBookByKeyword();
                        break;
                    case "9":
                        libraryService.CheckBookData();
                        break;
                    case "10":
                        libraryService.AddNewBook();
                        break;
                    case "11":
                    exit = true;
                    Console.WriteLine("Exiting...");
                    break;
                    default:
                        Console.WriteLine("Write a number between 1-8!");
                        break;
            }
        }
    }
