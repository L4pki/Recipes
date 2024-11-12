using Application.Seсurity;
using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using WebAPI.Services;
using Application.Commands.UserCommands;
using Infrastructure.Repositories.RecipeRepositories;
using Domain.Interfaces.RecipeInterfaces;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder( args );

builder.Services.AddMediatR( cfg => cfg.RegisterServicesFromAssemblyContaining<RegisterUserCommand>() );
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRecipeRepository, RecipeRepository>();
builder.Services.AddScoped<IIngridientRepository, IngridientRepository>();
builder.Services.AddScoped<IStepRepository, StepRepository>();
builder.Services.AddScoped<ITagRepository, TagRepository>();
builder.Services.AddScoped<IAuthService, AuthService>();

string secretKey = builder.Configuration[ "JwtSettings:SecretKey" ];
string issuer = builder.Configuration[ "JwtSettings:Issuer" ];
string audience = builder.Configuration[ "JwtSettings:Audience" ];

builder.Services.AddAuthentication( options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
} )
    .AddJwtBearer( options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = audience,
            IssuerSigningKey = new SymmetricSecurityKey( Encoding.UTF8.GetBytes( secretKey ) )
        };
    } );

builder.Services.AddControllers().AddJsonOptions( options =>
{
    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
    options.JsonSerializerOptions.WriteIndented = true;
} );

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen( c =>
{
    c.SwaggerDoc( "v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "My API", Version = "v1" } );

    c.AddSecurityDefinition( "Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Description = "Введите 'Bearer' [пробел] и ваш токен в поле ниже. Пример: 'Bearer abc123'",
        Name = "Authorization",
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey
    } );
    c.AddSecurityRequirement( new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[]{}
        }
    } );
} );

string connectionString = builder.Configuration.GetConnectionString( "Recipe" );

builder.Services.AddDbContext<RecipeDbContext>( o =>
{
    o.UseSqlServer( connectionString, ob => ob.MigrationsAssembly( typeof( RecipeDbContext ).Assembly.FullName ) );
}
);

var app = builder.Build();

if ( app.Environment.IsDevelopment() )
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
