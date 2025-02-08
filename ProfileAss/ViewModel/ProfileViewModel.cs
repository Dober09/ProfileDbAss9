
using ProfileAss.Service;

using ProfileAss.Model;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;

namespace ProfileAss.ViewModel
{
    public partial class ProfileViewModel:  ObservableObject
    {

       
        DataService _dataService;

        

        [ObservableProperty]
        private Profile profile;

        public ProfileViewModel(DataService dataService)
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
               Profile = await _dataService.ReadTextFile() ?? new Profile();
            }
            catch (Exception ex) { 
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }


        [RelayCommand]
        private async Task OnsaveAsync()
        {
            try
            {

               //write to json file 
                await _dataService.WriteToFile(Profile);
                System.Diagnostics.Debug.WriteLine($"Profile saved successfully: {Profile.firstname}");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Profile not saved failed: {ex.Message}");

            }
            



        }













    }
}
