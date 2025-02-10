
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

        [ObservableProperty]

        private ImageSource profileImage; 
        

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

                // Load the profile image if it exists, otherwise use default
                if (!string.IsNullOrEmpty(Profile.imagePath) && File.Exists(Profile.imagePath))
                {
                    ProfileImage = ImageSource.FromFile(Profile.imagePath);
                }
                else
                {
                    ProfileImage = "user.png";
                }
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
                await App.Current.MainPage.DisplayAlert("", "Profile saved successfully","ok");
            }
            catch(Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Profile not saved failed: {ex.Message}");
            }

        }


        [RelayCommand]
        private async Task UploadImage()
        {
            await ShowImagePicker();

        }


      
        private async Task ShowImagePicker()
        {
            try
            {

                var fileResult = await FilePicker.PickAsync(new PickOptions
                {
                    PickerTitle = "Please select an image",
                    FileTypes = FilePickerFileType.Images
                });


                if (fileResult != null)
                {

                    using var sourceStream = await fileResult.OpenReadAsync();
                    using var memorySteam = new MemoryStream();

                    //copy the stream to a memory
                    await sourceStream.CopyToAsync(memorySteam);
                    memorySteam.Position = 0;



                   var localPath=  await _dataService.UploadLocalAsync(fileResult.FileName, memorySteam);

                    // Update the Profile.imagePath
                    Profile.imagePath = localPath;

                    // Update the ProfileImage
                    ProfileImage = ImageSource.FromFile(localPath);

                }
            }
            catch (Exception ex) {
                System.Diagnostics.Debug.WriteLine($"Error uploading Image: {ex.Message}");
                await App.Current.MainPage.DisplayAlert("error", "Failed to uploaud image", "Ok");
                ProfileImage = "user.png";
            
            }
        }













    }
}
