using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Threading.Tasks;
using UserInfoAPI.Models.Steam;
using UserInfoAPI.Services;
using UserInfoAPI.DTOs;

namespace UserInfoAPITests
{
    [TestClass]
    public class SteamMethods
    {
        private IConfiguration Config { get; }

        private readonly Steam client;

        public SteamMethods()
        {
            var builder = new ConfigurationBuilder()
                .AddUserSecrets<SteamMethods>();
            Config = builder.Build();
            client = new Steam(Config["SteamApiKey"]); // make sure to set your user secret
        }

        [TestMethod, TestCategory("Conversion")]
        public async Task ConvertVanityUrlToSteamID64Url()
        {
            // Arrange
            string myVanityUrl = "mazewalker";
            long mySteam64Id = 76561198142951961;

            // Act
            VanityUrl result = await client.ConvertVanityUrlToSteamID64(myVanityUrl);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(result.Response.Steamid, mySteam64Id);
        }
        [TestMethod, TestCategory("Conversion")]
        public async Task ConvertVanityUrlToSteamID64NotFound()
        {
            // Arrange
            string myVanityUrl = "superlongsteamurlthatwontbefound";
            long mySteam64Id = 76561198142951961;

            // Act
            VanityUrl result = await client.ConvertVanityUrlToSteamID64(myVanityUrl);

            Console.WriteLine($"Message: {result.Response.Message}");

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result.Response.Steamid, mySteam64Id);
            Assert.AreEqual(result.Response.Success, 42); // 42 = not found
        }

        [TestMethod, TestCategory("Conversion")]
        public void ConvertSteamID2ToSteamID64()
        {
            // Arrange
            string steamID2 = "STEAM_0:1:91343116";

            long steamID64 = 76561198142951961;

            // Act
            long result = client.ConvertSteamID2ToSteamID64(steamID2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, 0);
            Assert.AreEqual(result, steamID64);
        }

        [TestMethod, TestCategory("Conversion")]
        public void ConvertSteamID3ToSteamID64()
        {
            // Arrange
            string steamID2 = "[U:1:182686233]";

            long steamID64 = 76561198142951961;

            // Act
            long result = client.ConvertSteamID3ToSteamID64(steamID2);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreNotEqual(result, 0);
            Assert.AreEqual(result, steamID64);
        }

        [TestMethod, TestCategory("Parsing")]
        public void ExtractSteamIDsFromString()
        {
            // Arrange
            string customProfiles = "http://steamcommunity.com/id/mazewalker";
            int customProfilesCount = 1;
            string customUrls = "/id/mazewalker";
            int customUrlsCount = 1;
            int customStringsCount = 0;
            string profiles = "http://steamcommunity.com/profiles/76561198142951961";
            int profilesCount = 1;
            string steamID2s = "STEAM_0:1:91343116";
            int steamID2sCount = 1;
            string steamID3s = "[U:1:182686233]";
            int steamID3sCount = 1;
            string steamID64s = "76561198142951961";
            int steamID64sCount = 1;
            int customStringCount = 0;

            string inputString = "STEAM_0:1:91343116 [U:1:182686233] 76561198142951961 MazeWalker http://steamcommunity.com/profiles/76561198142951961 http://steamcommunity.com/id/mazewalker /id/mazewalker random text /profiles/76561198142951961 unrelated text here ";

            // Act
            SteamIDLists result = client.ExtractSteamIDsFromString(inputString);

            // Assert
            Assert.AreEqual(customProfiles, result.CustomProfiles[0]);
            Assert.AreEqual(result.CustomProfiles.Count, customProfilesCount);

            Assert.AreEqual(result.CustomStrings.Count, customStringsCount);

            Assert.AreEqual(customUrls, result.CustomUrls[0], customUrls);
            Assert.AreEqual(result.CustomUrls.Count, customUrlsCount);

            Assert.AreEqual(steamID2s, result.SteamId2s[0]);
            Assert.AreEqual(steamID2sCount, result.SteamId2s.Count);

            Assert.AreEqual(profiles, result.Profiles[0]);
            Assert.AreEqual(profilesCount, result.Profiles.Count);

            Assert.AreEqual(steamID3s, result.SteamId3s[0]);
            Assert.AreEqual(steamID3sCount, result.SteamId3s.Count);

            Assert.AreEqual(steamID64s, result.SteamId64s[0]);
            Assert.AreEqual(steamID64sCount, result.SteamId64s.Count);

            Assert.AreEqual(result.CustomStrings.Count, customStringCount);
        }
        [TestMethod, TestCategory("Parsing")]
        public void ExtractSteamIDsFromStringOnlyCustomId()
        {
            // Arrange
            string customString = "MazeWalker";
            int customStringCount = 1;

            // Act
            SteamIDLists result = client.ExtractSteamIDsFromString(customString);

            // Assert
            Assert.AreEqual(result.CustomStrings[0], customString);
            Assert.AreEqual(result.CustomStrings.Count, customStringCount);

        }
    }
}