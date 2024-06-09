using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ExpenseTracker.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "First Name is required")]
        [StringLength(50, ErrorMessage = "First Name cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last Name is required")]
        [StringLength(50, ErrorMessage = "Last Name cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "NickName is required")]
        [StringLength(50, ErrorMessage = "Nick Name cannot be longer than 50 characters")]
        [Column(TypeName = "nvarchar(50)")]
        public string Nickname { get; set; }

        [Required(ErrorMessage = "Password is required")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Password is too short")]
        [Column(TypeName = "nvarchar(50)")]
        public string Password { get; set; }

        public byte[]? ProfileImage { get; set; }

        //OTHER
        [NotMapped]
       
        public string NicknameLogin { get; set; }
        [NotMapped]
       
        public string PasswordLogin { get; set; }

        [NotMapped]
        public string? DisplayImage
        {
            get
            {
                if (ProfileImage != null && ProfileImage.Length > 0)
                {
                    string base64String = Convert.ToBase64String(ProfileImage);
                    return $"data:image/jpg;base64,{base64String}";
                }
                return string.Empty; 
            }
        }
    }
}
