
using ProfileAss.Service;

using ProfileAss.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;


namespace ProfileAss.ViewModel
{
    public partial class ProfileViewModel : ObservableObject
    {


        private readonly IDataService _dataService;



        [ObservableProperty]
        private Profile profile;

        [ObservableProperty]

        private ImageSource profileImage;


        public ProfileViewModel(IDataService dataService)
        {
            _dataService = dataService;
            Profile = new Profile();
        }


        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                // check if the this is null 
                Profile = await _dataService.GetByIdAsync(1);

                if (Profile == null) {
                    Profile = new Profile
                    {
                        Id = 1, // Explicit ID for singleton pattern
                        firstname = "",
                        lastname = "",
                        email = "",
                        bio = ""
                    };

                    // Persist to database
                    await _dataService.AddAsync(Profile);

                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine(ex.Message);
                Profile = new Profile();
            }

        }


        [RelayCommand]
        private async Task OnsaveAsync()
        {
            try
            {
                bool isUpdated = await _dataService.UpdateAsync(Profile);
                if (isUpdated)
                {
                    System.Diagnostics.Debug.WriteLine($"Profile saved successfully: {Profile.firstname}"
                        );

                    await App.Current.MainPage.DisplayAlert("Success", "Profile saved successfully", "OK");
                }
            }
            catch (Exception ex) {
                {
                    System.Diagnostics.Debug.WriteLine($"Profile save failed {ex.Message}");
                    await App.Current.MainPage.DisplayAlert("Error", "Failed to save profile", "OK");
                }

            }


         












        }
    } 
}
