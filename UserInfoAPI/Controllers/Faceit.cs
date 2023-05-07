using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserInfoAPI.DTOs;
using UserInfoAPI.Interfaces;
using UserInfoAPI.Models.Generic;
using UserInfoAPI.Models.Steam;
using UserInfoAPI.Models.Faceit;
using UserInfoAPI.Services;
namespace UserInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Faceit : ControllerBase
    {
        [HttpGet("GetUserInfo/{steamID64}")]
        public async Task<IActionResult> GetUserInfo(long steamID64)
        {
            var players = await Services.Faceit.FindFaceitPlayer(steamID64);

            var faceitUserDTO = new FaceitUserDTO
            {
                Players = new List<DTOs.Player>()
            };

            foreach (var result in players.Payload.Players.Results)
            {
                var bans = await Services.Faceit.GetFaceitPlayerBans(result.Id);

                var player = new DTOs.Player()
                {
                    Id = result.Id,
                    SteamID64 = steamID64,
                    Nickname = result.Nickname,
                    Status = result.Status,
                    Games = result.Games,
                    Country = result.Country,
                    Verified = result.Verified,
                    Bans = bans.Payload.Select(b => new Ban
                    {
                        Type = b.Type,
                        Reason = b.Reason,
                        StartsAt = b.StartsAt,
                        EndsAt = b.EndsAt
                    }).ToList()
                };

                faceitUserDTO.Players.Add(player);
            }
            return Ok(faceitUserDTO);
        }
    }
}
