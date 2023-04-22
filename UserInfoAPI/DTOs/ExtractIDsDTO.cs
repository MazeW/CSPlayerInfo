using UserInfoAPI.Models.Steam;

namespace UserInfoAPI.DTOs
{
    public class ExtractIDsDTO
    {
        public SteamIDLists ParsedList { get; set; }
        public List<long> ConvertedList { get; set; } = new();
    }
}
