using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Another_Attempt.Book_Info;

namespace Another_Attempt.DbConnection
{
    public class LibraryContext : DbContext
    {
        public DbSet<Book> Books { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<BorrowTransaction> BorrowTransactions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySQL(@"server=localhost;database=books;user=huawei;password=;");//books
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BorrowTransaction>()
                .HasOne(bt => bt.Book)
                .WithMany(b => b.BorrowTransactions)
                .HasForeignKey(bt => bt.BookId);

            modelBuilder.Entity<BorrowTransaction>()
                .HasOne(bt => bt.Client)
                .WithMany(c => c.BorrowTransactions)
                .HasForeignKey(bt => bt.ClientId);
        }
    }
}