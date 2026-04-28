using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface IPhotographerService
    {
        void Add(Photographer photographer);
        Photographer GetById(int id);
        List<Photographer> GetAll();
        void Update(Photographer newPhotographer);
        
    }
}
