

using ProfileAss.Model;

namespace ProfileAss.Service
{
    public interface IDataService
    {
        Task<List<ProductItem>> GetAllProductAsync();
        Task<Profile> GetByIdAsync(int id);
        Task<bool> AddAsync(Profile entity);
        Task<bool> UpdateAsync(Profile entity);
        Task<bool> DeleteAsync(int id);
        Task<bool> AddProductAsync(ProductItem item);

        Task<bool> UpdateBasketItemAsync(BasketItem item);

        Task<bool> AddBasketItemAsync(BasketItem item);
        Task<Basket> GetOrCreateBasketAsync(int profileId);
        Task<bool> RemoveBasketItemAsync(BasketItem item);

         Task<BasketItem> GetBasketItemAsync(int profileId);
        Task<List<BasketItem>> GetBasketItemsByProfileIdAsync(int profileId);
    }
}
