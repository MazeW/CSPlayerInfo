namespace UserInfoAPI.Models.Steam
{
    public class BannedPlayer
    {
        public long SteamId { get; set; }
        public bool CommunityBanned { get; set; }
        public bool VACBanned { get; set; }
        public int NumberOfVACBans { get; set; }
        public int DaysSinceLastBan { get; set; }
        public int NumberOfGameBans { get; set; }
        public string EconomyBan { get; set; }
    }

    public class PlayerBans
    {
        public List<BannedPlayer> Players { get; set; }
    }
}
