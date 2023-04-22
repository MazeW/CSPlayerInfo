using UserInfoAPI.Models.Steam;
namespace UserInfoAPI.Interfaces
{
    public interface ISteam
    {
        Task<VanityUrl> ConvertVanityUrlToSteamID64(string customUrl);
        Task<PlayerSummary> GetPlayerSummary(long steam64id);
        Task<PlayerBans> GetPlayerBans(long steam64id);
        Task<int> GetPlayerSteamLevel(long steam64id);
        Task<PlayerPlaytime> GetPlayerPlaytime(long steam64id, int appId);
        Task<PlayerStats> GetPlayerStats(long steam64id, int appId);

        SteamIDLists ExtractSteamIDsFromString(string input);

        Task<List<long>> ConvertToSteamID64Async(SteamIDLists steamIDLists);

    }
}
