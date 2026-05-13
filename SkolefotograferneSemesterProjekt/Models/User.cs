using System.ComponentModel.DataAnnotations;

namespace SkolefotograferneSemesterProjekt.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        [Required]
        [StringLength(maximumLength: 255, MinimumLength = 6, ErrorMessage = "Password must be at least 6 characters")]
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public User()
        {
            
        }

    }
}
