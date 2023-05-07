using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Threading.Tasks;
using UserInfoAPI.Models.Faceit;
using UserInfoAPI.Services;

namespace UserInfoAPITests
{
    [TestClass]
    public class FaceitMethods
    {

        [TestMethod, TestCategory("Search")]
        public async Task FindSingleAccount()
        {
            // Arrange
            long steamId = 76561198142951961;

            int resultCount = 1;

            string nickname = "Maz-e";

            // Act
            FindPlayer faceitUser = await Faceit.FindFaceitPlayer(steamId);

            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(faceitUser));

            string faceitName = faceitUser.Payload.Players.MainAccount().Nickname;
            int searchResultCount = faceitUser.Payload.Players.Total_count;
            // Assert
            Assert.AreEqual(faceitName, nickname);
            Assert.AreEqual(resultCount, searchResultCount);
        }

        [TestMethod, TestCategory("Bans")]
        public async Task CheckUserBans()
        {
            // Arrange
            string faceitId = "d7df9e38-eae4-45b9-ac9c-c20ea556ec86";
            int playerBans = 0;
            // Act
            SheriffBans bans = await Faceit.GetFaceitPlayerBans(faceitId);

            int banCount = bans.Payload.Count;
            // Assert
            Assert.AreEqual(banCount, playerBans);
        }

        [TestMethod, TestCategory("Bans")]
        public async Task CheckKnownBannedUserBans()
        {
            // Arrange
            string faceitId = "d957ca7a-35d4-4fd2-92f2-f6ce047792d1";
            int playerBans = 1;
            // Act
            SheriffBans bans = await Faceit.GetFaceitPlayerBans(faceitId);
            string? bannedId = bans.Payload?.FirstOrDefault()?.UserId;
            int? banCount = bans.Payload?.Count;
            Console.WriteLine(Newtonsoft.Json.JsonConvert.SerializeObject(bans));
            // Assert
            Assert.AreEqual(banCount, playerBans);
            Assert.AreEqual(faceitId,bannedId);
        }

    }
}
