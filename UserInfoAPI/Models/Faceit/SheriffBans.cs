using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace UserInfoAPI.Models.Faceit
{
    public class BansPayload
    {
        [JsonProperty("nickname", NullValueHandling = NullValueHandling.Ignore)]
        public string Nickname { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }

        [JsonProperty("reason", NullValueHandling = NullValueHandling.Ignore)]
        public string Reason { get; set; }

        [JsonProperty("game", NullValueHandling = NullValueHandling.Ignore)]
        public object Game { get; set; }

        [JsonProperty("starts_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime StartsAt { get; set; }

        [JsonProperty("ends_at", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime EndsAt { get; set; } = DateTime.MaxValue;

        [JsonProperty("user_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UserId { get; set; }
    }
    public class SheriffBans
    {

        [JsonPropertyName("payload")]
        public List<BansPayload> Payload { get; set; }
    }
}
