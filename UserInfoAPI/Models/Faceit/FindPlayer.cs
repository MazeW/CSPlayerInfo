using Newtonsoft.Json;

namespace UserInfoAPI.Models.Faceit
{
    public class Game
    {
        public string Name { get; set; }
        [JsonProperty("skill_level")]
        public int SkillLevel { get; set; }
    }


    public class Payload
    {
        public Players Players { get; set; }
    }

    public class Players
    {
        public List<Result> Results { get; set; }
        public int Total_count { get; set; }

        public Result MainAccount() // this is not a super good implementation, i need to investigate all the possible responses to accurately choose the main account
        {
            if (Results.Count == 1)
                return Results.FirstOrDefault();

            return Results.FirstOrDefault(x => x.Games.Count > 0);
        }
    }

    public class Result
    {
        public string Id { get; set; }
        public string Nickname { get; set; }
        public string Status { get; set; }
        public List<Game> Games { get; set; }
        public string Country { get; set; }
        public bool Verified { get; set; }

        public string ProfileUrl()
        {
            return $"https://www.faceit.com/en/players/{Nickname}";
        }
    }

    public class FindPlayer
    {
        public Payload Payload { get; set; }
    }
}
