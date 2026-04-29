using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUserService
    {
        void Delete(int id);
        void Update(User user);

        User VerifyUser(string mail, string password);
    }
}
