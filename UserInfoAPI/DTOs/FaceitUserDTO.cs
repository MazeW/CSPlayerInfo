using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using UserInfoAPI.DTOs;
using UserInfoAPI.Interfaces;
using UserInfoAPI.Models.Generic;
using UserInfoAPI.Models.Steam;
using UserInfoAPI.Models.Faceit;
using UserInfoAPI.Services;
namespace UserInfoAPI.DTOs
{
    public class Ban
    {
        public string Type { get; set; }
        public string Reason { get; set; }
        public DateTime StartsAt { get; set; }
        public DateTime EndsAt { get; set; }
    }


    public class Player
    {
        public string Id { get; set; }
        public long SteamID64 { get; set; }
        public string Nickname { get; set; }
        public string Status { get; set; }
        public List<Game> Games { get; set; }
        public string Country { get; set; }
        public bool Verified { get; set; }
        public List<Ban> Bans { get; set; }
    }

    public class FaceitUserDTO
    {
        public List<Player> Players { get; set; }
    }
}