using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ProfileAss.Model;
using ProfileAss.Service;
using CommunityToolkit.Mvvm.Input;

namespace ProfileAss.ViewModel
{
    public partial class BasketViewModel: ObservableObject
    {

        private readonly IDataService _dataService;
        private int _currentProfileId = 1;

        [ObservableProperty]
        public ObservableCollection<BasketItem> _basketItems = new ObservableCollection<BasketItem>();


        public BasketViewModel(IDataService dataService)
        {
            _dataService = dataService;
            LoadBasketItems();
        }


        private async void LoadBasketItems()
        {
            try
            {
                var items = await _dataService.GetBasketItemsByProfileIdAsync(_currentProfileId);
                BasketItems.Clear();
                foreach (var item in items)
                {
                    BasketItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading basket: {ex.Message}");
            }
        }


        [RelayCommand]
        public async Task AddToBasket(ProductItem product)
        {
            try
            {
                // Get/Create basket via service
                var basket = await _dataService.GetOrCreateBasketAsync(_currentProfileId);

                var basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductItemId = product.Id,
                    Quantity = 1,
                    AddedDate = DateTime.UtcNow
                };

                if (await _dataService.AddBasketItemAsync(basketItem))
                {
                    // Refresh basket items
                    LoadBasketItems();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task RemoveItem(BasketItem item)
        {
            if (await _dataService.RemoveBasketItemAsync(item))
            {
                BasketItems.Remove(item);
            }
        }


    }
}
