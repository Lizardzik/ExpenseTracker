using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }
        [Column(TypeName = "nvarchar(50)")]
        public string Nickname { get; set; }

        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; }

        [Column(TypeName = "nvarchar(150)")]
        public string? ProfileImage { get; set; }

        
    }
}
