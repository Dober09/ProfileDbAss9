
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
        private ObservableCollection<Profile> profiles;

        [ObservableProperty]
        private Profile _profile;

        public ProfileViewModel(DataService dataService)
        {
            _dataService = dataService;
              
        

             
        }


        [RelayCommand]
        private async Task LoadDataAsync()
        {
            try
            {
                var profiles = await _dataService.ReadTextFile();
               
                Profiles = new ObservableCollection<Profile>(profiles);
            }
            catch (Exception ex) { 
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

        }













    }
}
