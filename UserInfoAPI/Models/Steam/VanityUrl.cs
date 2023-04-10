namespace UserInfoAPI.Models.Steam
{
    public class VanityResponse
    {
        public int Success { get; set; }
        public long Steamid { get; set; }
        public string Message { get; set; }

    }
    public class VanityUrl
    {
        public VanityResponse Response { get; set; }
    }
}
