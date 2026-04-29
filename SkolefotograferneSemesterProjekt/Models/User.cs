namespace SkolefotograferneSemesterProjekt.Models
{
    public class User
    {
        public int ID { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; }
        public User()
        {
            
        }

    }
}
