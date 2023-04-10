namespace UserInfoAPI.Models.Steam
{
    public class Achievement
    {
        public string Name { get; set; }
        public int Achieved { get; set; }
    }

    public class PlayerStats
    {
        public string SteamID { get; set; }
        public string GameName { get; set; }
        public List<Stat> Stats { get; set; }
        public List<Achievement> Achievements { get; set; }
    }

    public class PlayerStatsResponse
    {
        public PlayerStatsResponse Playerstats { get; set; }
    }

    public class Stat
    {
        public string Name { get; set; }
        public int Value { get; set; }
    }
}
