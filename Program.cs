using WebAPIApplication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Azure.Identity;
using ExerciseApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// https://www.youtube.com/watch?v=I8p8j5MuMAo

var keyVaultUriString = builder.Configuration.GetConnectionString("KeyVaultURI");
if (keyVaultUriString is null)
{
    throw new ArgumentNullException(
        nameof(keyVaultUriString),
        "KeyVaultURI configuration value is null."
    );
}

var keyVaultURI = new Uri(keyVaultUriString);

// you need to be logged in to Azure for this to work, via azure cli or visual studio
var azureCredential = new DefaultAzureCredential();

// Adds our secrets from Key Vault to the configuration
builder.Configuration.AddAzureKeyVault(keyVaultURI, azureCredential);

var domain = $"https://{builder.Configuration["Auth0:Domain"]}/";
builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.Authority = domain;
        options.Audience = builder.Configuration["Auth0:Audience"];
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy(
        "read:messages",
        policy => policy.Requirements.Add(new HasScopeRequirement("read:messages", domain))
    );
});

builder.Services.AddSingleton<IAuthorizationHandler, HasScopeHandler>();
builder.Services.AddScoped<IDbService, DbService>();
builder.Services.AddScoped<IUserService, UserService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();
