using Microsoft.EntityFrameworkCore;
using ProfileAss.Data;
using ProfileAss.Model;


namespace ProfileAss.Service
{
    public class DataService : IDataService
    {
  
        private readonly DatabaseContext _context;

        public DataService(DatabaseContext context)
        {
            
            _context = context;
        }

        public async Task<List<BasketItem>> GetBasketItemsByProfileIdAsync(int profileId)
        {
            return await _context.basketItems
                .Include(bi => bi.ProductItem)
                .Where(bi => bi.Basket.ProfileId == profileId)
                .ToListAsync();
        }

        public async Task<List<ProductItem>> GetAllProductAsync()
        {
            // 
            return await _context.products.ToListAsync();
        }

        public async Task<Profile> GetByIdAsync(int id)
        {
            //return await _context.Profile.FindAsync(id);
            return await _context.person.FindAsync(id);
        }

        public async Task<bool> AddProductAsync(ProductItem item) {
            try {
                await _context.products.AddAsync(item);
                await _context.SaveChangesAsync();
                System.Diagnostics.Debug.WriteLine($"Saved item");
                return true;
            
            } catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex);
                return false;
            }
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



        public async Task<Basket> GetOrCreateBasketAsync(int profileId)
        {
            var basket = await _context.baskets
                .FirstOrDefaultAsync(b => b.ProfileId == profileId);

            if (basket == null)
            {
                basket = new Basket { ProfileId = profileId };
                await _context.baskets.AddAsync(basket);
                await _context.SaveChangesAsync();
            }

            return basket;
        }

        public async Task<bool> RemoveBasketItemAsync(BasketItem item)
        {
            try
            {
                _context.basketItems.Remove(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> AddBasketItemAsync(BasketItem item)
        {
            try
            {
                await _context.basketItems.AddAsync(item);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
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
