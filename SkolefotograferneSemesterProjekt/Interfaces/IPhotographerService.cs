using Microsoft.Data.SqlClient;
using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotographerService
    {
        Task Add(Photographer photographer);
        Task<List<Photographer>> GetAll();
        Task Update(Photographer newPhotographer);
        
    }
}
