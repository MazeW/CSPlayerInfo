namespace UserInfoAPI.Models.Steam
{
    public class Player
    {
        public long Steamid { get; set; }
        public int Communityvisibilitystate { get; set; }
        public int Profilestate { get; set; }
        public string Personaname { get; set; }
        public int Commentpermission { get; set; }
        public string Profileurl { get; set; }
        public string Avatar { get; set; }
        public string Avatarmedium { get; set; }
        public string Avatarfull { get; set; }
        public string Avatarhash { get; set; }
        public int Personastate { get; set; }
        public string Realname { get; set; }
        public string Primaryclanid { get; set; }
        public int Timecreated { get; set; }
        public int Personastateflags { get; set; }
        public string Loccountrycode { get; set; }
        public string Locstatecode { get; set; }
        public int Loccityid { get; set; }

        public DateTime CreationDate()
        {
            return DateTimeOffset.FromUnixTimeSeconds(Timecreated).UtcDateTime;
        }
    }

    public class PlayerResponse
    {
        public List<Player> Players { get; set; }
    }

    public class PlayerSummary
    {
        public PlayerResponse Response { get; set; }
    }
}
