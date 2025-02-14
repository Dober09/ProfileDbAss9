

using ProfileAss.Model;

namespace ProfileAss.Service
{
    public interface IDataService
    {
        Task<List<Profile>> GetAllAsync();
        Task<Profile> GetByIdAsync(int id);
        Task<bool> AddAsync(Profile entity);
        Task<bool> UpdateAsync(Profile entity);
        Task<bool> DeleteAsync(int id);
    }
}
