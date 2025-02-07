
using ProfileAss.Service;

using ProfileAss.Model;
using CommunityToolkit.Mvvm.ComponentModel;

namespace ProfileAss.ViewModel
{
    public class ProfileViewModel:  ObservableObject
    {

        Profile Profile { get; set; }
        DataService _dataService;

        public ProfileViewModel(DataService dataService)
        {
            _dataService = dataService;

            Initialize();  
        }


        public async void Initialize()
        {
            var profile = await _dataService.ReadTextFile();

           Profile = new Profile { 
               firstname = profile.firstname,
               lastname=profile.lastname,
               email = profile.email,
                boi = profile.boi,
           };


        }







    }
}
