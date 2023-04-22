using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserInfoAPI.Interfaces;
using UserInfoAPI.Models.Generic;
using UserInfoAPI.Models.Steam;
using UserInfoAPI.DTOs;

namespace UserInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Steam : ControllerBase
    {
        private readonly ISteam client;

        public Steam(ISteam steam) => client = steam;


        [HttpGet("ResolveVanityURL/{customUrl}")]
        public async Task<IActionResult> ResolveVanityUrl(string customUrl)
        {
            try
            {
                VanityUrl result = await client.ConvertVanityUrlToSteamID64(customUrl);
                if (result.Response.Success == 42)
                {
                    return NotFound(new { Error = "Url you provided wasn't found." });
                }

                return Ok(result.Response);
            }
            catch (Exception)
            {
                return BadRequest($"Error resolving vanity URL, make sure it is properly formatted.");
            }
        }

        [HttpGet("PlayerSummary/{steamID64}")]
        public async Task<IActionResult> GetPlayerSummary(long steamID64)
        {
            try
            {
                var result = await client.GetPlayerSummary(steamID64);
                return Ok(result.Response.Players.FirstOrDefault());
            }
            catch (Exception)
            {
                return BadRequest("Failed to retrieve info");
            }
        }

        [HttpPost("ExtractSteamIDsFromString")]
        public async Task<IActionResult> ExttractSteamIDsAsync([FromBody] ReceivePost body)
        {
            string input = body.Input;
            if (string.IsNullOrEmpty(input))
            {
                return NotFound(JsonConvert.SerializeObject(new { Error = "Empty or invalid body provided" }, Formatting.Indented));
            }

            try
            {
                SteamIDLists extractedList = client.ExtractSteamIDsFromString(input);
                ExtractIDsDTO returnList = new()
                {
                    ParsedList = extractedList,
                    ConvertedList = await client.ConvertToSteamID64Async(extractedList)
                };

                return Ok(JsonConvert.SerializeObject(returnList, Formatting.Indented));
            }
            catch (Exception)
            {
                return NotFound(JsonConvert.SerializeObject(new { Error = "Invalid input or no steamid's found" }, Formatting.Indented));
            }


        }
    }
}
