using ProfileAss.ViewModel;

namespace ProfileAss.Views;

public partial class ProductPage : ContentPage
{
	public ProductPage(ProductViewModel viewModel)
	{
		InitializeComponent();

		BindingContext = viewModel;
	}
}