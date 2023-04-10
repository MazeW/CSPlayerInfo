using UserInfoAPI.Services;
using UserInfoAPI.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var steamApiKey = builder.Configuration["SteamApiKey"]; // set your api key with dotnet user-secrets set "SteamApiKey" "my-steam-api-key-here" or Manage User Secrets in VS

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddSingleton<ISteam>(new Steam(steamApiKey));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
