using ProfileAss.ViewModel;

namespace ProfileAss.Views
{

	public partial class ProfilePage : ContentPage
	{
		
		public ProfilePage(ProfileViewModel viewModel)
		{
			InitializeComponent();
			
			BindingContext = viewModel;

		}

		protected override async void OnAppearing()
		{
			base.OnAppearing();

			await((ProfileViewModel)BindingContext).LoadDataCommand.ExecuteAsync(null);
		}



      


    }
}