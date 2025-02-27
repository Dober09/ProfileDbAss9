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

                // Create a temporary basket item for immediate display
                var tempBasketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductItemId = product.Id,
                    ProductItem = product, // Set the product directly for UI display
                    Quantity = 1
                };

                // Add to UI immediately
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    BasketItems.Add(tempBasketItem);
                    BadgeCounterService.SetCount(BadgeCounterService.Count + 1);

                    string message = "Added item to cart";
                    var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
                    toast.Show();
                });

                // Save to database in the background
                var basketItem = new BasketItem
                {
                    BasketId = basket.Id,
                    ProductItemId = product.Id,
                    Quantity = 1
                };

                bool success = await _dataService.AddBasketItemAsync(basketItem);
                if (success)
                {
                    // Get the saved item with proper ID
                    var savedItem = await _dataService.GetBasketItemAsync(basketItem.Id);

                    // Replace the temporary item with the saved one
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        int index = BasketItems.IndexOf(tempBasketItem);
                        if (index >= 0)
                        {
                            BasketItems[index] = savedItem;
                        }
                    });

                    System.Diagnostics.Debug.WriteLine($"Successfully added {product.ProductName} to basket");
                }
                else
                {
                    // If save failed, remove the temporary item
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        BasketItems.Remove(tempBasketItem);
                        BadgeCounterService.SetCount(BadgeCounterService.Count - 1);

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
            // Remove from UI immediately
            MainThread.BeginInvokeOnMainThread(() =>
            {
                BasketItems.Remove(item);
                BadgeCounterService.SetCount(BadgeCounterService.Count - 1);
            });

            // Remove from database in the background
            await _dataService.RemoveBasketItemAsync(item);
        }

        [RelayCommand]
        public async Task IncreaseQuantity(BasketItem item)
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

        [RelayCommand]
        public async Task DecreaseQuantity(BasketItem item)
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
    }
}

//using CommunityToolkit.Mvvm.ComponentModel;
//using System.Collections.ObjectModel;
//using ProfileAss.Model;
//using ProfileAss.Service;
//using CommunityToolkit.Mvvm.Input;
//using CommunityToolkit.Maui.Alerts;
//using ProfileAss.Service;
//using System.Collections.Specialized;

//namespace ProfileAss.ViewModel
//{
//    public partial class BasketViewModel: ObservableObject
//    {

//        private readonly IDataService _dataService;
//        private int _currentProfileId = 1;

//        [ObservableProperty]
//        public ObservableCollection<BasketItem> _basketItems = new ObservableCollection<BasketItem>();

//        [ObservableProperty]
//        private  BadgeCounterService _badgeCounterService;

//        [ObservableProperty]
//        private decimal _totalPrice;

//        public BasketViewModel(IDataService dataService, BadgeCounterService badgeCounterService)
//        {
//            _badgeCounterService = badgeCounterService;

//            _basketItems = new ObservableCollection<BasketItem>();
//            // Subscribe to collection changes to update total price
//            _basketItems.CollectionChanged += BasketItems_CollectionChanged;


//            _dataService = dataService;
//            LoadBasketItems();
//        }

//        private void BasketItems_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
//        {
//            CalculateTotalPrice();
//        }

//        private void CalculateTotalPrice()
//        {
//            TotalPrice = BasketItems.Sum(item => item.ProductItem.ProductPrice * item.Quantity);
//        }

//        private async void LoadBasketItems()
//        {
//            try
//            {
//                var items = await _dataService.GetBasketItemsByProfileIdAsync(_currentProfileId);

//                MainThread.BeginInvokeOnMainThread(() =>
//                {
//                    BasketItems.Clear();
//                    foreach (var item in items)
//                    {
//                        BasketItems.Add(item);
//                    }
//                    System.Diagnostics.Debug.WriteLine($"Loaded {BasketItems.Count} items into basket");
//                    BadgeCounterService.SetCount(BasketItems.Count);

//                    CalculateTotalPrice();

//                });

//                }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error loading basket: {ex.Message}");
//            }
//        }


//        [RelayCommand]
//        public async Task AddToBasket(ProductItem product)
//        {
//            try
//            {
//                // First get/create the basket
//                var basket = await _dataService.GetOrCreateBasketAsync(_currentProfileId);

//                // Check if item already exists
//                var existingItem = BasketItems.FirstOrDefault(i => i.ProductItemId == product.Id);
//                if (existingItem != null)
//                {
//                    // Optionally increment quantity instead of adding new item
//                    existingItem.Quantity++;
//                    // Update in price
//                    CalculateTotalPrice();
//                    return;
//                }

//                // Create new basket item with only the necessary references
//                var basketItem = new BasketItem
//                {
//                    BasketId = basket.Id,
//                    ProductItemId = product.Id,
//                    Quantity = 1
//                };

//                if (await _dataService.AddBasketItemAsync(basketItem))
//                {

//                    string message = "added item to cart";
//                    var toast = Toast.Make(message, CommunityToolkit.Maui.Core.ToastDuration.Short, 10);
//                    toast.Show();
//                    // Reload the complete item with its navigation properties
//                    basketItem = await _dataService.GetBasketItemAsync(basketItem.Id);


//                    MainThread.BeginInvokeOnMainThread(() => {
//                        BasketItems.Add(basketItem);
//                        BadgeCounterService.SetCount(BadgeCounterService.Count + 1);
//                    });

//                    System.Diagnostics.Debug.WriteLine($"Successfully added {product.ProductName} to basket");
//                }
//                else
//                {
//                    System.Diagnostics.Debug.WriteLine($"Failed to add {product.ProductName} to basket");
//                }
//            }
//            catch (Exception ex)
//            {
//                System.Diagnostics.Debug.WriteLine($"Error adding to basket: {ex.Message}");
//                throw;
//            }
//        }

//        [RelayCommand]
//        public async Task RemoveItem(BasketItem item)
//        {
//            if (await _dataService.RemoveBasketItemAsync(item))
//            {
//                MainThread.BeginInvokeOnMainThread(() => {
//                    BasketItems.Remove(item);
//                    BadgeCounterService.SetCount(BadgeCounterService.Count - 1);
//                });
//            }
//        }


//        [RelayCommand]
//        public async Task IncreaseQuantity(BasketItem item)
//        {
//            item.Quantity++;
//            // You would need to add UpdateBasketItemAsync to your DataService
//            // await _dataService.UpdateBasketItemAsync(item);
//            CalculateTotalPrice();
//        }

//        [RelayCommand]
//        public async Task DecreaseQuantity(BasketItem item)
//        {
//            if (item.Quantity > 1)
//            {
//                item.Quantity--;
//                // You would need to add UpdateBasketItemAsync to your DataService
//                // await _dataService.UpdateBasketItemAsync(item);
//                CalculateTotalPrice();
//            }
//            else
//            {
//                // If quantity would become 0, remove the item
//                await RemoveItem(item);
//            }
//        }


//    }
//}
