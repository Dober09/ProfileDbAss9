using ProfileAss.ViewModel; 

namespace ProfileAss.Views;

public partial class BasketPage : ContentPage
{

	
	public BasketPage(BasketViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
		
    }

   


}