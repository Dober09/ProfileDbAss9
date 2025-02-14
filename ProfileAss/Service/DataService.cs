

using Microsoft.EntityFrameworkCore;
using ProfileAss.Data;
using ProfileAss.Model;
using System.Text.Json;

namespace ProfileAss.Service
{
    public class DataService : IDataService
    {
        
        private readonly string filePath;
        private readonly DatabaseContext _context;




        public DataService(DatabaseContext context)
        {
            filePath = Path.Combine(FileSystem.AppDataDirectory,"ProfileData.txt");
            _context = context;
        }

        public async Task<List<Profile>> GetAllAsync()
        {
            //return await _context.Profile.ToListAsync();
            return await _context.person.ToListAsync();
        }

        public async Task<Profile> GetByIdAsync(int id)
        {
            //return await _context.Profile.FindAsync(id);
            return await _context.person.FindAsync(id);
        }

        public async Task<bool> AddAsync(Profile entity)
        {
            try
            {
                //await _context.People.AddAsync(entity);
                await _context.person.AddAsync(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Profile entity)
        {
            try
            {
                _context.person.Update(entity);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var person = await GetByIdAsync(id);
            if (person == null) return false;

            try
            {
                _context.person.Remove(person);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
