using Flurl;
using Flurl.Http;
using System.Diagnostics;
using System.Text.RegularExpressions;
using UserInfoAPI.DTOs;
using UserInfoAPI.Interfaces;
using UserInfoAPI.Models.Steam;
namespace UserInfoAPI.Services
{
    public class Steam : ISteam
    {
        private readonly string _apiKey;

        private readonly string ApiBaseUrl = "http://api.steampowered.com/";

        public Steam(string apiKey)
        {
            _apiKey = apiKey;
        }

        /// <summary>
        /// Input is the custom vanity url that you can set for your profile
        /// </summary>
        /// <param name="vanityUrl"></param>
        /// <returns>int Success (1 successful / 42 failure)
        /// long steamId (user's steam id)
        /// string Message (mostly used for error)</returns>
        public async Task<VanityUrl> ConvertVanityUrlToSteamID64(string vanityUrl)
        {

            VanityUrl getVanityUrl = await ApiBaseUrl
                      .AppendPathSegment("ISteamUser/ResolveVanityURL/v0001/")
                      .SetQueryParam("key", _apiKey)
                      .SetQueryParam("vanityurl", vanityUrl)
                      .GetJsonAsync<VanityUrl>();

            return getVanityUrl;
        }

        public async Task<PlayerSummary> GetPlayerSummary(long steamID64)
        {
            PlayerSummary getPlayerSummary = await ApiBaseUrl
                .AppendPathSegment("ISteamUser/GetPlayerSummaries/v2/")
                .SetQueryParam("key", _apiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("steamids", steamID64)
                .GetJsonAsync<PlayerSummary>();

            return getPlayerSummary;

        }

        public async Task<PlayerBans> GetPlayerBans(long steamID64)
        {

            PlayerBans playerBans = await ApiBaseUrl
                .AppendPathSegment("ISteamUser/GetPlayerBans/v1")
                .SetQueryParam("key", _apiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("steamids", steamID64)
                .GetJsonAsync<PlayerBans>();

            return playerBans;

        }

        public async Task<int> GetPlayerSteamLevel(long steamID64)
        {

            dynamic getLevel = await ApiBaseUrl
                .AppendPathSegment("IPlayerService/GetSteamLevel/v1")
                .SetQueryParam("key", _apiKey)
                .SetQueryParam("steamid", steamID64)
                .GetJsonAsync<dynamic>();

            return getLevel.response?.player_level ?? -1;

        }

        public async Task<PlayerPlaytime> GetPlayerPlaytime(long steamID64, int appId)
        {

            dynamic playtime = await ApiBaseUrl
                .AppendPathSegment("IPlayerService/GetOwnedGames/v1")
                .SetQueryParam("key", _apiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("steamid", steamID64)
                .SetQueryParam("include_played_free_games", 1)
                .SetQueryParam("appids_filter[0]", appId) // 730 csgo
                .GetJsonAsync<dynamic>();

            if (((Newtonsoft.Json.Linq.JObject)playtime.response).Count == 0 || playtime.response.game_count == 0 || playtime.response.games[0].playtime_forever == 0)
            {
                return new PlayerPlaytime { PrivatePlaytime = true };
            }
            // possibility to implement platform specific playtime too
            // response: {"response":{"game_count":1,"games":[{"appid":730,"playtime_forever":0,"playtime_windows_forever":0,"playtime_mac_forever":0,"playtime_linux_forever":0,"rtime_last_played":0}]}}
            return new PlayerPlaytime { TotalPlaytime = playtime.response.games[0].playtime_forever / 60, PlaytimeLast2Weeks = playtime.response.games[0].playtime_2weeks / 60, PrivatePlaytime = false };

        }

        public async Task<PlayerStats> GetPlayerStats(long steamID64, int appId)
        {

            PlayerStats playerStats = await ApiBaseUrl
                .AppendPathSegment("ISteamUserStats/GetUserStatsForGame/v0002")
                .SetQueryParam("key", _apiKey)
                .SetQueryParam("format", "json")
                .SetQueryParam("appid", appId) // 730 csgo
                .SetQueryParam("steamid", steamID64)
                .GetJsonAsync<PlayerStats>();

            return playerStats;
        }

        public long ConvertSteamID2ToSteamID64(string id)
        {
            try
            {
                string[] parts = id.Split(':');
                long steam64 = (long)(((ulong)(int.Parse(parts[2]) * 2 + int.Parse(parts[1]))) + 0x0110000100000000UL);
                return steam64;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public long ConvertSteamID3ToSteamID64(string id)
        {
            try
            {
                if (long.TryParse(id.AsSpan(5, id.Length - 6), out long steamID32)) // Extract the numerical part and try parsing it to a long
                {
                    long steamID64 = steamID32 + 76561197960265728L; // Add the offset to get the SteamID64 value
                    return steamID64;
                }
                return 0;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public SteamIDLists ExtractSteamIDsFromString(string input)
        {
            Debug.WriteLine($"received: {input}");

            Regex regex = new(@"(?<CUSTOMPROFILE>https?:\/\/steamcommunity\.com\/id\/[A-Za-z0-9_-]+)|(?<CUSTOMURL>\/id\/[A-Za-z0-9_-]+)|(?<PROFILE>https?:\/\/steamcommunity\.com\/profiles\/[0-9]+)|(?<STEAMID2>STEAM_[10]:[10]:[0-9]+)|(?<STEAMID3>\[U:[10]:[0-9]+\])|(?<STEAMID64>[^\/][0-9]{8,})|(?<CUSTOMSTRING>[A-Za-z0-9_-]{2,32})");

            MatchCollection matches = regex.Matches(input);

            if (matches.Count == 0)
            {
                throw new Exception("No matches");
            }

            SteamIDLists steamIDLists = new();

            foreach (Match match in matches)
            {
                if (match.Groups["CUSTOMPROFILE"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.CustomProfiles.Contains(value))
                    {
                        Debug.WriteLine($"CUSTOMPROFILE match found: {value}");
                        steamIDLists.CustomProfiles.Add(value);
                    }
                }

                if (match.Groups["CUSTOMURL"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.CustomUrls.Contains(value))
                    {
                        Debug.WriteLine($"CUSTOMURL match found: {value}");
                        steamIDLists.CustomUrls.Add(value);
                    }
                }

                if (match.Groups["PROFILE"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.Profiles.Contains(value))
                    {
                        Debug.WriteLine($"PROFILE match found: {value}");
                        steamIDLists.Profiles.Add(value);
                    }
                }

                if (match.Groups["STEAMID2"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.SteamId2s.Contains(value))
                    {
                        Debug.WriteLine($"STEAMID2 match found: {value}");
                        steamIDLists.SteamId2s.Add(value);
                    }
                }

                if (match.Groups["STEAMID3"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.SteamId3s.Contains(value))
                    {
                        Debug.WriteLine($"STEAMID3 match found: {value}");
                        steamIDLists.SteamId3s.Add(value);
                    }
                }

                if (match.Groups["STEAMID64"].Success)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.SteamId64s.Contains(value))
                    {
                        Debug.WriteLine($"STEAMID64 match found: {value}");
                        steamIDLists.SteamId64s.Add(value);
                    }
                }

                if (match.Groups["CUSTOMSTRING"].Success && input.Length < 33)
                {
                    string value = match.Value.Trim();
                    if (!steamIDLists.CustomStrings.Contains(value))
                    {
                        Debug.WriteLine($"CUSTOMSTRING match found: {value}");
                        steamIDLists.CustomStrings.Add(value);
                    }
                }
            }

            return steamIDLists;
        }

        public async Task<long> ConvertCustomProfileToSteamID64(string input)
        {
            int startIndex = input.IndexOf("/id/") + 4;
            int endIndex = input.IndexOf("/", startIndex);
            if (endIndex == -1)
            {
                endIndex = input.Length;
            }
            string username = input[startIndex..endIndex];

            VanityUrl converted = await ConvertVanityUrlToSteamID64(username);
            return long.Parse(converted.Response.Steamid);
        }

        public long ConvertProfileToSteamID64(string profileUrl)
        {
            int lastSlashIndex = profileUrl.LastIndexOf('/');

            string steamIdSubstring = profileUrl[(lastSlashIndex + 1)..];

            long steamId64 = long.Parse(steamIdSubstring);

            return steamId64;
        }

        public async Task<List<long>> ConvertToSteamID64Async(SteamIDLists steamIDLists)
        {
            List<long> steamID64s = new();

            foreach (var property in steamIDLists.GetType().GetProperties())
            {
                List<string> list = (List<string>)property.GetValue(steamIDLists);
                string propertyName = property.Name;

                foreach (string item in list)
                {
                    long steamID64 = 0;

                    switch (propertyName)
                    {
                        case "CustomProfiles":
                            steamID64 = await ConvertCustomProfileToSteamID64(item);
                            break;
                        case "CustomUrls":
                            steamID64 = await ConvertCustomProfileToSteamID64(item);
                            break;
                        case "Profiles":
                            steamID64 = ConvertProfileToSteamID64(item);
                            break;
                        case "SteamId2s":
                            steamID64 = ConvertSteamID2ToSteamID64(item);
                            break;
                        case "SteamId3s":
                            steamID64 = ConvertSteamID3ToSteamID64(item);
                            break;
                        case "SteamId64s":
                            steamID64 = long.Parse(item);
                            break;
                        case "CustomStrings":
                            steamID64 = long.Parse((await ConvertVanityUrlToSteamID64(item)).Response.Steamid);
                            break;
                    }

                    if (!steamID64s.Contains(steamID64) && steamID64 != 0) // if there is some parsing issue or something we don't wanna add 0
                    {
                        steamID64s.Add(steamID64);
                    }
                    
                }
            }
            return steamID64s;
        }

    }
}