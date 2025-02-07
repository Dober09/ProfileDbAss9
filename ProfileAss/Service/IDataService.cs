

using ProfileAss.Model;

namespace ProfileAss.Service
{
    public interface IDataService
    {
        Task<List<Profile>> ReadTextFile();

        Task<Profile> WirteTextToFile();

    }
}
