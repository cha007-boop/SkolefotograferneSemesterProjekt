using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IParentServices
    {
        Task AddParent(Parent parent);

        Task <List<Parent>> GetAllParents();

        Task<List<Parent>> FilterParents(String Filter);

        Task<Parent> SearchParent(int id);

    }
}
