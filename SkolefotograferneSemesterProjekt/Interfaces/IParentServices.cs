using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IParentServices
    {
        Task<int> AddParent(Parent parent);

        Task <List<Parent>> GetAllParents();

        Task<List<Parent>> FilterParents(string Filter);

        Task<Parent> SearchParent(int id);

        Task deleteParent(Parent parent);

        Task Update(Parent newParent);

    }
}
