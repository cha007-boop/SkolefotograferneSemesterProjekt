using SkolefotograferneSemesterProjekt.Models;

namespace SkolefotograferneSemesterProjekt.Interfaces
{
    public interface ISysAdminService
    {
        Task<int> Add(SysAdmin sysAdmin);
        Task<List<SysAdmin>> GetAll();
        Task<SysAdmin> SearchByID(int id);
        Task Update(SysAdmin newSysAdmin);
    }
}
