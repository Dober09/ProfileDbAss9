
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
        Profile profiles;

       

        public  async Task<Profile> ReadTextFile()


        {

            try
            {
                //open the file from the app package
                using var stream = await FileSystem.OpenAppPackageFileAsync("DataFile.txt");

                //Read the entire content of the file
                using var reader = new StreamReader(stream);
                var content = await reader.ReadToEndAsync();

                //Deserialize the content
                // If you have a list of profiles,


                var profiles = JsonSerializer.Deserialize<Profile>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

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
