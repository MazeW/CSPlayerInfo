using Microsoft.AspNetCore.Mvc;
using UserInfoAPI.DTOs;
namespace UserInfoAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class Faceit : ControllerBase
    {
        [HttpGet("GetUserInfo/{steamID64}")]
        public async Task<IActionResult> GetUserInfo(long steamID64)
        {
            var faceitUserDTO = new FaceitUserDTO
            {
                Players = new List<DTOs.Player>()
            };
            try
            {
                var players = await Services.Faceit.FindFaceitPlayer(steamID64);

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
            catch (Exception)
            {
                return BadRequest(new { Error = "Failed to retrieve player details" });
            }
        }
    }
}
