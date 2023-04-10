using Newtonsoft.Json;

namespace UserInfoAPI.Models.Faceit
{
    public class BansPayload
    {
        public string Nickname { get; set; }
        public string Reason { get; set; }
        [JsonProperty("Starts_At")]
        public DateTime StartsAt { get; set; }
        [JsonProperty("Ends_At")]
        public DateTime EndsAt { get; set; }
    }
    public class SheriffBans
    {
        List<BansPayload> Payload { get; set; }
    }
}
