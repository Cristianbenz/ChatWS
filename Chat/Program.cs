using ChatWS.Helpers;
using ChatWS.Hubs;
using ChatWS.Models;
using ChatWS.Services;
using DB;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration.UserSecrets;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

//Cors
string clientCors = "AngularChatClient";
builder.Services.AddCors(config =>
{
    config.AddPolicy(
        name: clientCors,
        builder =>
        {
            builder.AllowAnyHeader();
            builder.AllowAnyMethod();
            builder.WithOrigins("http://localhost:4200");
            builder.AllowCredentials();
        }
        );
});

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddControllers()
    .AddJsonOptions(x =>
                x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Add SignalR
builder.Services.AddSignalR();

// Add DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
{
    options.UseNpgsql(ConnectionStringHelper.GetConnectionString(builder.Configuration));
});

// Add JwtBearer Authentication
IConfigurationSection jwtConfigSection = builder.Configuration.GetSection("JwtConfig");
builder.Services.Configure<JwtConfig>(jwtConfigSection);
var key = jwtConfigSection.Get<JwtConfig>();
builder.Services.AddSingleton<IJwtConfig>(x => x.GetRequiredService<IOptions<JwtConfig>>().Value);

builder.Services.AddAuthentication(config =>
{
    config.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    config.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateAudience = false,
            ValidateIssuer = false,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key.Key))
        };
    });

// Configure database
var dbConfigSection = builder.Configuration.GetSection(nameof(DbConfig));
builder.Services.Configure<DbConfig>(dbConfigSection);

// Inject DB
builder.Services.AddSingleton<IDbConfig>( a => a.GetRequiredService<IOptions<DbConfig>>().Value);

// Add Services
builder.Services.AddSingleton<ChatHub>();
builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<MessagesService>();
builder.Services.AddScoped<ChatService>();
builder.Services.AddScoped<UserService>();

var app = builder.Build();

var scope = app.Services.CreateScope();
await MigrationHelper.MigrateDataAsync(scope.ServiceProvider);

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Set route for ChatHub
app.MapHub<ChatHub>("/chat");

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.UseCors(clientCors);

app.MapControllers();

app.Run();
