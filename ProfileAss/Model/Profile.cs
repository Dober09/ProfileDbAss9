
using System.Text.Json.Serialization;

namespace ProfileAss.Model
{
    public class Profile
    {
        [JsonPropertyName("firstname")]
        public string firstname { get; set; }

        [JsonPropertyName("lastname")]
        public string lastname { get; set; }

        [JsonPropertyName("email")]
        public string email { get; set; }

        [JsonPropertyName("bio")]
        public string bio { get; set; }

        [JsonPropertyName("imagePath")]
        public string imagePath { get; set; }

    }
}
