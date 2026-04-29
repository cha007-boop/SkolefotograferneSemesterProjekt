namespace SkolefotograferneSemesterProjekt.Models
{
    public class SysAdmin : User
    {
        public SysAdmin()
            : base()
        {
            Role = UserRole.SysAdmin;
        }
    }
}
