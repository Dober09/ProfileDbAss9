

using ProfileAss.Model;

namespace ProfileAss.Service
{
    public interface IDataService
    {
        Task<Profile> ReadTextFile();

        Task WriteToFile(Profile profile);

    }
}
