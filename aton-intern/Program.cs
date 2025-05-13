using Aton_intern.Services.Auth;
using Aton_intern.Services.Initialization;
using Aton_intern.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//builder.Services.AddScoped<ITokenService, TokenService>(); 
builder.Services.AddSingleton<IUserService, UserService>();

var jwtKey = "YourStrongJwtSecretKeyHere_ChangeThis!";
builder.Services.AddSingleton(new TokenService(jwtKey));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = "UsersApi",
        ValidAudience = "UsersApiClient",
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
    };
});

builder.Services.AddAuthorization();

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    //app.MapScalarApiReference(options =>
    //{
    //    options.WithOpenApiRoutePattern("/swagger/{documentName}/swagger.json").WithTitle("My API Documentation").WithTheme(ScalarTheme.Mars);
    //});
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

await app.ConfigureAdminAsync();

app.Run();

//this project uses scalar for it UI, you need to add scalar/v1
