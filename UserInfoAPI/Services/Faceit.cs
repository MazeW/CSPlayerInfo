using Flurl;
using Flurl.Http;
using UserInfoAPI.Models.Faceit;

namespace UserInfoAPI.Services
{
    public static class Faceit
    {
        private static readonly string baseUrl = "https://api.faceit.com/";

        public static async Task<FindPlayer> FindFaceitPlayer(long steamID64)
        {
            return await baseUrl
                .AppendPathSegment("search/v1/")
                .SetQueryParam("limit", "10")
                .SetQueryParam("query", $"{steamID64}")
                .GetJsonAsync<FindPlayer>();
        }

        public static async Task<SheriffBans> GetFaceitPlayerBans(string faceitId)
        {
            return await baseUrl
                .AppendPathSegment("sheriff/v1/bans/")
                .AppendPathSegment(faceitId)
                .GetJsonAsync<SheriffBans>();
        }
    }
}
