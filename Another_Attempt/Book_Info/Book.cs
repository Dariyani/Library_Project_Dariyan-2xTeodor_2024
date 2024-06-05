using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Another_Attempt.Book_Info
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [MaxLength(255)]
        public string Title { get; set; }

        [Required]
        [MaxLength(100)]
        public string Author { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public int TotalCopies { get; set; }

        public int AvailableCopies { get; set; }
        public ICollection<BorrowTransaction> BorrowTransactions { get; set; }
    }
}
