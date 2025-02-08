

using ProfileAss.Model;

using System.Text.Json;


namespace ProfileAss.Service
{
    public class DataService : IDataService
    {
        
        private readonly string filePath;
       


        public DataService()
        {
            filePath = Path.Combine(FileSystem.AppDataDirectory, "ProfileData.json");
        }
        public  async Task<Profile> ReadTextFile()


        {

            try
            {

                //check if file exist
                if (!File.Exists(filePath)) { 
                    return new Profile();
                }
                System.Diagnostics.Debug.WriteLine($"filePath ----> ${filePath}");

                var content = await File.ReadAllTextAsync(filePath);
                return JsonSerializer.Deserialize<Profile>(content);
              
               

             
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Error reading profiles : {ex.Message}");
                return new Profile();

            }
           
            
        }

        public async Task<string> UploadLocalAsync(string filename, Stream stream)
        {
            var localPath =  Path.Combine(FileSystem.AppDataDirectory, filename);
            using var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write);
            await fs.CopyToAsync(stream);

            return localPath;
        }


        public async Task WriteToFile(Profile p)
        {
            try
            {
            
                string json = JsonSerializer.Serialize(p);
                await File.WriteAllTextAsync(filePath,json);
            
                System.Diagnostics.Debug.WriteLine($"Succefully wrote the info");
            }
            catch (Exception ex) {

                System.Diagnostics.Debug.WriteLine($"error failed to save profile {ex.Message}");
            }
        }
    }
}
