using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IUserService
    {
        int Add(User user);
        void Delete(int id);
        void Update(User user);

        User VerifyUser(string mail, string password);
    }
}
