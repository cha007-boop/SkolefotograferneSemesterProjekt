using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotographerService
    {
        Task Add(Photographer photographer);
        Task<List<Photographer>> GetAll();
        Task<Photographer> SearchByID(int id);
        Task Update(Photographer newPhotographer);
        //Task Delete(int id);
    }
}
