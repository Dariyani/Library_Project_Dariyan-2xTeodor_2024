using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Another_Attempt.Book_Info
{
    public class BorrowTransaction
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int BookId { get; set; }
        [ForeignKey("BookId")]
        public Book Book { get; set; }

        [Required]
        public int ClientId { get; set; }
        [ForeignKey("ClientId")]
        public Client Client { get; set; }
        [Required]
        public DateTime BorrowDate { get; set; }

        public DateTime? ReturnDate { get; set; }
    }
}
