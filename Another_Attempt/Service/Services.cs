using Another_Attempt.Book_Info;
using Another_Attempt.DbConnection;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZstdSharp.Unsafe;

namespace Another_Attempt.Service
{
    public class Services
    {
        private readonly LibraryContext _context;

        public Services(LibraryContext context)
        {
            _context = context;
        }
        public void ShowBookInfo()
        {
            var books = _context.Books.ToList();
            if (books.Any())
            {
                Console.WriteLine("\nList of all books:");
                foreach (var book in books)
                {
                    Console.WriteLine($"ID: {book.BookId}, Title: {book.Title}, Author: {book.Author}, Year: {book.Year}, Total Copies: {book.TotalCopies}, Available Copies: {book.AvailableCopies}");
                }
            }
            else
            {
                Console.WriteLine("No books available.");
            }
        }
        public void ListAllBooks()
        {
            var books = _context.Books.Where(b => b.AvailableCopies > 0).ToList();
            if (books.Count == 0)
            {
                Console.WriteLine("No books available.");
            }
            else
            {
                foreach (var book in books)
                {
                    Console.WriteLine($"Title: {book.Title}");
                }
            }
        }

        

        public void TakeBook()
        {
            {
                Console.Write("Enter the client's ID: ");
                if (int.TryParse(Console.ReadLine(), out int clientId))
                {
                    var client = _context.Clients
                        .Include(c => c.BorrowTransactions)
                        .FirstOrDefault(c => c.ClientId == clientId);

                    if (client == null)
                    {
                        Console.WriteLine("Client not found.");
                        return;
                    }

                    int currentBorrowedBooks = client.BorrowTransactions
                        .Count(bt => bt.ReturnDate == null);

                    if (currentBorrowedBooks >= 3)
                    {
                        Console.WriteLine("Client has already borrowed the maximum number of books (3).");
                        return;
                    }

                    Console.Write("Enter the book's ID: ");
                    if (int.TryParse(Console.ReadLine(), out int bookId))
                    {
                        var book = _context.Books.Find(bookId);
                        if (book == null)
                        {
                            Console.WriteLine("Book not found.");
                            return;
                        }

                        if (book.AvailableCopies > 0)
                        {
                            var borrowTransaction = new BorrowTransaction
                            {
                                BookId = bookId,
                                ClientId = clientId,
                                BorrowDate = DateTime.Now
                            };

                            book.AvailableCopies -= 1;

                            _context.BorrowTransactions.Add(borrowTransaction);
                            _context.SaveChanges();
                            Console.WriteLine("Book borrowed successfully.");
                        }
                        else
                        {
                            Console.WriteLine("No available copies.");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Invalid book ID format.");
                    }
                }
                else
                {
                    Console.WriteLine("Invalid client ID format.");
                }
            }
        }
        private bool AskIfContinue()
        {
            Console.Write("Do you want to borrow another book? (y/n): ");
            var response = Console.ReadLine().Trim().ToLower();
            return response == "y";
        }
        public void ReturnBook()
        {
            Console.Write("Enter the borrow transaction ID: ");
            if (int.TryParse(Console.ReadLine(), out int borrowTransactionId))
            {
                var transaction = _context.BorrowTransactions.Find(borrowTransactionId);
                if (transaction == null)
                {
                    Console.WriteLine("Borrow transaction not found.");
                    return;
                }

                if (transaction.ReturnDate != null)
                {
                    Console.WriteLine("This book has already been returned.");
                    return;
                }

                transaction.ReturnDate = DateTime.Now;

                var book = _context.Books.Find(transaction.BookId);
                if (book != null)
                {
                    book.AvailableCopies += 1;
                }

                _context.SaveChanges();
                Console.WriteLine("Book returned successfully.");
            }
            else
            {
                Console.WriteLine("Invalid transaction ID format.");
            }
        }
        public void CheckTakenBooks()
        {
            {
                Console.Write("Enter book ID: ");
                var bookId = int.Parse(Console.ReadLine());

                var book = _context.Books
                    .Include(b => b.BorrowTransactions)
                    .ThenInclude(bt => bt.Client)
                    .FirstOrDefault(b => b.BookId == bookId);

                if (book == null)
                {
                    Console.WriteLine("Book not found.");
                    return;
                }

                Console.WriteLine($"Book ID: {book.BookId}");
                Console.WriteLine($"Title: {book.Title}");
                Console.WriteLine($"Physical Copy Information: {book.AvailableCopies}");

                var activeTransaction = book.BorrowTransactions
                    .FirstOrDefault(bt => bt.ReturnDate == null);

                if (activeTransaction == null)
                {
                    Console.WriteLine("Noone has taken this book yet.");
                }
                else
                {
                    Console.WriteLine($"Available copies: {activeTransaction.Book.AvailableCopies}");
                    Console.WriteLine($"Client ID: {activeTransaction.Client.ClientId}");
                    Console.WriteLine($"Client Name: {activeTransaction.Client.FirstName} {activeTransaction.Client.LastName}");
                    Console.WriteLine($"Borrow Date: {activeTransaction.BorrowDate}");
                }
            }
        }
        public void CheckTakenBooksByAClient()
        {
            Console.Write("Enter client ID: ");
            var clientId = int.Parse(Console.ReadLine());

            var client = _context.Clients
                .Include(c => c.BorrowTransactions)
                .ThenInclude(bt => bt.Book)
                .FirstOrDefault(c => c.ClientId == clientId);

            if (client == null)
            {
                Console.WriteLine("Client not found.");
                return;
            }

            Console.WriteLine($"Client ID: {client.ClientId}");
            Console.WriteLine($"Name: {client.FirstName} {client.LastName}");
            Console.WriteLine("Currently Borrowed Books:");

            var borrowedBooks = client.BorrowTransactions
                .Where(bt => bt.ReturnDate == null)
                .ToList();

            if (borrowedBooks.Count == 0)
            {
                Console.WriteLine("This client has not borrowed any books currently.");
            }
            else
            {
                foreach (var transaction in borrowedBooks)
                {
                    Console.WriteLine();
                    Console.WriteLine($"- Book Title: {transaction.Book.Title}");
                    Console.WriteLine();
                    Console.WriteLine($"  Borrow Date: {transaction.BorrowDate}");
                    Console.WriteLine();
                }
            }
        }
        public void AddNewBook()
        {
            Console.Write("Enter book title: ");
            var title = Console.ReadLine();

            Console.Write("Enter book author: ");
            var author = Console.ReadLine();

            Console.Write("Enter year of publication: ");
            var year = int.Parse(Console.ReadLine());

            Console.Write("Enter total number of copies: ");
            var totalCopies = int.Parse(Console.ReadLine());

            var book = new Book
            {
                Title = title,
                Author = author,
                Year = year,
                TotalCopies = totalCopies,
                AvailableCopies = totalCopies
            };

            _context.Books.Add(book);
            _context.SaveChanges();

            Console.WriteLine("Book added successfully.");
        }
        public void CheckBookByKeyword()
        {
            Console.Write("Enter a keyword to search in book titles: ");
            var keyword = Console.ReadLine();

            var books = _context.Books
                .Where(b => b.Title.Contains(keyword))
                .ToList();

            if (books.Count == 0)
            {
                Console.WriteLine($"No books found containing the keyword '{keyword}' in the title.");
                return;
            }

            Console.WriteLine($"Books containing '{keyword}' in the title:");
            foreach (var book in books)
            {
                Console.WriteLine($"- Title: {book.Title}");
                Console.WriteLine($"  Author: {book.Author}");
                Console.WriteLine($"  Available Copies: {book.AvailableCopies}");
                Console.WriteLine();
            }
        }
        public void CheckTitlesByAuthor()
        {
            Console.Write("Enter author name: ");
            var authorName = Console.ReadLine();

            var booksByAuthor = _context.Books
                .Where(b => b.Author.Contains(authorName))
                .ToList();

            if (booksByAuthor.Count == 0)
            {
                Console.WriteLine("No books found by this author.");
                return;
            }

            Console.WriteLine($"Books by {authorName}:");
            foreach (var book in booksByAuthor)
            {
                Console.WriteLine($"- Title: {book.Title}");
                Console.WriteLine($"  Available Copies: {book.AvailableCopies}");
                Console.WriteLine();
            }
        }
        public void DeleteBook()
        {
            Console.Write("Enter the ID of the book to delete: ");
            if (int.TryParse(Console.ReadLine(), out int bookId))
            {
                var book = _context.Books.Find(bookId);
                if (book == null)
                {
                    Console.WriteLine("Book not found.");
                    return;
                }

                _context.Books.Remove(book);
                _context.SaveChanges();
                Console.WriteLine($"Book with ID {bookId} has been deleted.");
            }
            else
            {
                Console.WriteLine("Invalid ID format.");
            }
        }
        public void CheckBookData()
        {
            var books = _context.Books.ToList();
            Random rand = new Random();

            foreach (var book in books)
            {
                var borrowHistory = _context.BorrowTransactions
                    .Where(bt => bt.BookId == book.BookId)
                    .Select(bt => new { bt.Client.FirstName, bt.Client.LastName, bt.BorrowDate, bt.ReturnDate })
                    .ToList();

                if (borrowHistory.Count == 0)
                {
                    Console.WriteLine($"No borrow history found for book '{book.Title}'.");
                }
                else
                {
                    Console.WriteLine($"Borrow history for book '{book.Title}':");
                    foreach (var entry in borrowHistory)
                    {
                        Console.WriteLine($"- {entry.FirstName} {entry.LastName} (Borrowed: {entry.BorrowDate}, Returned: {entry.ReturnDate})");
                    }
                }
                Console.WriteLine();
            }

            _context.SaveChanges();
        }
    }
}

