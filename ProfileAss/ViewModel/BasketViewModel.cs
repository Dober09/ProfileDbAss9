using CommunityToolkit.Mvvm.ComponentModel;
using System.Collections.ObjectModel;
using ProfileAss.Model;
using ProfileAss.Service;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Maui.Alerts;
using System.Collections.Specialized;

namespace ProfileAss.ViewModel
{
    public partial class BasketViewModel : ObservableObject
    {
        private readonly IDataService _dataService;
        private int _currentProfileId = 1;

        [ObservableProperty]
        private ObservableCollection<BasketItem> _basketItems;

        [ObservableProperty]
        private BadgeCounterService _badgeCounterService;

        [ObservableProperty]
        private decimal _totalPrice;

        public BasketViewModel(IDataService dataService, BadgeCounterService badgeCounterService)
        {
            _badgeCounterService = badgeCounterService;
            _dataService = dataService;
            _basketItems = new ObservableCollection<BasketItem>();

            // Subscribe to collection changes to update total price
            _basketItems.CollectionChanged += BasketItems_CollectionChanged;

            LoadBasketItems();
        }

        private void BasketItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            CalculateTotalPrice();
        }

        private void CalculateTotalPrice()
        {
            TotalPrice = BasketItems.Sum(item => item.ProductItem != null ? item.ProductItem.ProductPrice * item.Quantity : 0);
        }

        private async void LoadBasketItems()
        {
            try
            {
                var items = await _dataService.GetBasketItemsByProfileIdAsync(_currentProfileId);

                MainThread.BeginInvokeOnMainThread(() =>
                {
                    BasketItems.Clear();
                    foreach (var item in items)
                    {
                        BasketItems.Add(item);
                    }

                    // Update badge counter
                    BadgeCounterService.SetCount(BasketItems.Count);
                    CalculateTotalPrice();
                });

                System.Diagnostics.Debug.WriteLine($"Loaded {BasketItems.Count} items into basket");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading basket: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task AddToBasket(ProductItem product)
        {
            if (product == null)
            {
                System.Diagnostics.Debug.WriteLine("Cannot add null product to basket");
                return;
            }

            try
            {
                // Check if item already exists
                var existingItem = BasketItems.FirstOrDefault(i => i.ProductItemId == product.Id);
                if (existingItem != null)
                {
                    // Increment quantity immediately in the UI
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        existingItem.Quantity++;
                        CalculateTotalPrice();

                        string message = "Increased quantity in cart";
                        var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
                        toast.Show();
                    });

                    // Update in database in the background
                    await _dataService.UpdateBasketItemAsync(existingItem);
                    return;
                }

                // For new items, we need to handle differently
                // Get/create the basket first (this needs to be done before adding to UI)
                var basket = await _dataService.GetOrCreateBasketAsync(_currentProfileId);

                // Create a new basket item
                var basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductItemId = product.Id,
                    Quantity = 1,
                    ProductItem = product // Set the product directly for UI display
                };

                // Add to database first to get a valid ID
                bool success = await _dataService.AddBasketItemAsync(basketItem);

                if (success)
                {
                    // Get the saved item with proper ID to ensure we have the complete item
                    var savedItem = await _dataService.GetBasketItemAsync(basketItem.Id);

                    if (savedItem == null)
                    {
                        // If for some reason we couldn't get the saved item, use our original
                        savedItem = basketItem;
                    }

                    // Add to UI
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        BasketItems.Add(savedItem);
                        BadgeCounterService.SetCount(BasketItems.Count);

                        string message = "Added item to cart";
                        var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
                        toast.Show();
                    });

                    System.Diagnostics.Debug.WriteLine($"Successfully added {product.ProductName} to basket");
                }
                else
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        string message = "Failed to add item to cart";
                        var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
                        toast.Show();
                    });

                    System.Diagnostics.Debug.WriteLine($"Failed to add {product.ProductName} to basket");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error adding to basket: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task RemoveItem(BasketItem item)
        {
            if (item == null)
            {
                System.Diagnostics.Debug.WriteLine("Cannot remove null item from basket");
                return;
            }

            try
            {
                // Remove from UI immediately
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    BasketItems.Remove(item);
                    BadgeCounterService.SetCount(BasketItems.Count);

                    string message = "Removed from cart";
                    var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
                    toast.Show();
                });

                // Remove from database in the background
                bool success = await _dataService.RemoveBasketItemAsync(item);

                if (!success)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to remove item from database: {item.Id}");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error removing from basket: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task IncreaseQuantity(BasketItem item)
        {
            if (item == null) return;

            try
            {
                // Update UI immediately
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    item.Quantity++;
                    CalculateTotalPrice();
                });

                // Update database in background
                await _dataService.UpdateBasketItemAsync(item);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error increasing quantity: {ex.Message}");
            }
        }

        [RelayCommand]
        public async Task DecreaseQuantity(BasketItem item)
        {
            if (item == null) return;

            try
            {
                if (item.Quantity > 1)
                {
                    // Update UI immediately
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        item.Quantity--;
                        CalculateTotalPrice();
                    });

                    // Update database in background
                    await _dataService.UpdateBasketItemAsync(item);
                }
                else
                {
                    // If quantity would become 0, remove the item
                    await RemoveItem(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error decreasing quantity: {ex.Message}");
            }
        }

        // Add method to refresh the basket items from database
        [RelayCommand]
        public async Task RefreshBasket()
        {
            LoadBasketItems();
        }
    }
}





