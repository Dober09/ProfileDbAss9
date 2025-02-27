using CommunityToolkit.Mvvm.ComponentModel;
using ProfileAss.Model;

using System.Collections.ObjectModel;
using ProfileAss.Service;
using CommunityToolkit.Mvvm.Input;

namespace ProfileAss.ViewModel
{
    public partial class ProductViewModel : ObservableObject
    {
        //private BasketViewModel _basketViewModel;

        private readonly BasketViewModel _basketViewModel;


        private readonly IDataService _dataService;

        [ObservableProperty]
        public ObservableCollection<ProductItem> productItems;

        public ProductViewModel(IDataService dataService, BasketViewModel basketViewModel)
        {

            _dataService = dataService;
            _basketViewModel = basketViewModel;
            ProductItems = new ObservableCollection<ProductItem>();

           LoadProductItems();

        }

        [RelayCommand]
        private async void AddToBasket(ProductItem product)
        {
            if (product != null)
            {
                System.Diagnostics.Debug.WriteLine($"Adding product to basket: {product.ProductName}");
                try
                {
                    await _basketViewModel.AddToBasket(product);
                    System.Diagnostics.Debug.WriteLine($"Successfully added to basket");
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error adding to basket: {ex.Message}");
                }
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("Product is null");
            }

        }

        private async void LoadProductItems() {

            var products = await _dataService.GetAllProductAsync();

            if (products == null || !products.Any())
            {
                // Initialize default products if none exist
                var defaultProducts = new List<ProductItem>
                {
                    new ProductItem
                    {
                        ImageName = "https://thefoschini.vtexassets.com/arquivos/ids/178190269-300-400/a33297cc-9106-4294-9519-2931e0d3e30e.png?v=638751354389770000",
                        ProductName= "adidas",
                        ProductDescription= "Lightblaze Black/Grey Sneaker",
                        ProductPrice = 29.99m
                    },
                    new ProductItem
                    {
                        ImageName = "https://thefoschini.vtexassets.com/arquivos/ids/178189750-300-400/800fed35-963e-427d-b3c4-f0a073c2846c.png?v=638751353485730000",
                        ProductName = "Nike",
                        ProductDescription="Dunk Low Green/White Sneaker",
                        ProductPrice = 59.99m
                    },
                    new ProductItem
                    {
                        ImageName = "https://thefoschini.vtexassets.com/arquivos/ids/177987004-300-400/5706a5af-f122-4522-825c-51ff3d0bbeab.png?v=638749625577070000",
                        ProductName = "New Balance",
                        ProductDescription=" 2002R Yellow Sneaker",
                        ProductPrice = 89.99m
                    },
                    new ProductItem
                    {
                        ImageName = "https://images.puma.com/image/upload/f_auto,q_auto,w_600,b_rgb:FAFAFA/global/images/399853/01/sv01/fnd/ZAF/fmt/png",
                        ProductName = "Puma",
                        ProductDescription="Club Grey/White Sneaker",
                        ProductPrice = 45.99m
                    },
                    new ProductItem
                    {
                        ImageName = "https://thefoschini.vtexassets.com/arquivos/ids/177987004-300-400/5706a5af-f122-4522-825c-51ff3d0bbeab.png?v=638749625577070000",
                        ProductName = "New Balance",
                        ProductDescription=" 2002R Yellow Sneaker",
                        ProductPrice = 89.99m
                    },
                    new ProductItem
                    {
                        ImageName = "https://images.puma.com/image/upload/f_auto,q_auto,w_600,b_rgb:FAFAFA/global/images/399853/01/sv01/fnd/ZAF/fmt/png",
                        ProductName = "Puma",
                        ProductDescription="Club Grey/White Sneaker",
                        ProductPrice = 45.99m
                    }
                };

                // Save default products to database
                foreach (var product in defaultProducts)
                {
                    await _dataService.AddProductAsync(product);
                }

                // Reload products from database
                products = await _dataService.GetAllProductAsync();
            }

            // Clear existing items and add loaded products
            ProductItems.Clear();
            foreach (var product in products)
            {
                ProductItems.Add(product);
            }

        }
    }

}
    

