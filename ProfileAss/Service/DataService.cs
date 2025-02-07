
using ProfileAss.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace ProfileAss.Service
{
    public class DataService : IDataService
    {
        List<Profile> profiles = new List<Profile>();

       


        public  async Task<List<Profile>> ReadTextFile()


        {

            try
            {
                if (profiles.Count > 0) { return profiles; }
                //open the file from the app package
                using var stream = await FileSystem.OpenAppPackageFileAsync("ProfileData.json");

                //Read the entire content of the file
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                //Deserialize the content
                // If you have a list of profiles,


                 profiles = JsonSerializer.Deserialize<List<Profile>>(content);

                return profiles;

            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error reading profiles : {ex.Message}");
                return null;

            }
           
            
        }

        public Task<Profile> WirteTextToFile()
        {
            throw new NotImplementedException();
        }
    }
}
