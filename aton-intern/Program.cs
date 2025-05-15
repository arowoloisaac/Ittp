using aton_intern.Configuration;
using Aton_intern.Services.Auth;
using Aton_intern.Services.Initialization;
using Aton_intern.Services.UserService;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
//builder.Services.AddOpenApi();

//reference here - https://stackoverflow.com/questions/79265776/how-to-add-jwt-token-support-globally-in-scalar-for-a-net-9-application
builder.Services.AddOpenApi("v1", options => { options.AddDocumentTransformer<BearerSecuritySchemeTransformer>(); });

//adding swaggerUI to the application
builder.Services.AddSwaggerGen();
builder.Services.AddSwaggerGen(opt => {
    opt.SwaggerDoc("v1", new OpenApiInfo { Title= "Swagger UI",  Version = "v1" });
    //opt.EnableAnnotations();
    opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        In = ParameterLocation.Header,
        Description = "Input valid token",
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        BearerFormat = "JWT",
        Scheme = "Bearer"
    });
    opt.AddSecurityRequirement(new OpenApiSecurityRequirement
                 {
                     {
                         new OpenApiSecurityScheme
                         {
                             Reference = new OpenApiReference
                             {
                                 Type = ReferenceType.SecurityScheme,
                                 Id ="Bearer"
                             }
                         },
                         new string[]{}
                     }
                 });
});

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

builder.Services.AddAuthorization( options  =>
{
    options.AddPolicy("IsAdmin", policy =>
    {
        policy.RequireClaim("Admin", "True");
    });
});

var app = builder.Build();

app.MapDefaultEndpoints();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    app.UseSwagger();
    app.UseSwaggerUI();

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

/***
 *this project uses both scalar and swagger for it UI
 *to add both UI to the application, you need to copy the localhost route and add the respective name (scalar and swagger)***/
