using Animal_Adoption_Management_System_Backend.Authorization;
using Animal_Adoption_Management_System_Backend.Configurations;
using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Middlewares;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;
using AutoMapper;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("AnimalAdoptionConnection") ?? throw new InvalidOperationException("Connection string 'AnimalAdoptionConnection' was not found.");

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnimalAdoptionContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddTokenProvider<DataProtectorTokenProvider<User>>(builder.Configuration["JwtSettings:TokenProvider"])
    .AddEntityFrameworkStores<AnimalAdoptionContext>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowOnlyLocalhostOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000");
        policy.AllowAnyHeader();
        policy.AllowAnyMethod();
        policy.AllowCredentials();
    });
});

builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

builder.Services.AddSingleton<IAuthorizationHandler, AgeHandler>();
builder.Services.AddSingleton<IEnumService, EnumService>();
builder.Services.AddScoped<IAuthManager, AuthManager>();
builder.Services.AddScoped<IPermissionChecker, PermissionChecker>();

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAnimalBreedService, AnimalBreedService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IShelterService, ShelterService>();
builder.Services.AddScoped<IAnimalShelterService, AnimalShelterService>();
builder.Services.AddScoped<IDonationService, DonationService>();
//builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageService>(x => new ImageService(x.GetRequiredService<AnimalAdoptionContext>(),x.GetRequiredService<IMapper>(), x.GetRequiredService<IHostEnvironment>()));
builder.Services.AddScoped<IAdoptionApplicationService, AdoptionApplicationService>();
builder.Services.AddScoped<IAdoptionContractService, AdoptionContractService>();
builder.Services.AddScoped<IManagedAdoptionContractService, ManagedAdoptionContractService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddTransient<DataInitialiser>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultSignOutScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ClockSkew = TimeSpan.Zero,
        ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
        ValidAudience = builder.Configuration["JwtSettings:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:Key"]))
    };
    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("X-Access-Token"))
                context.Token = context.Request.Cookies["X-Access-Token"];
            return Task.CompletedTask;
        }
    };
});

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("MinimalAge", policy =>
        policy.Requirements.Add(new AgeRequirement(20)));
});
// Serilog configuration
var logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .CreateLogger();
builder.Logging.AddSerilog(logger);


var app = builder.Build();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using var scope = app.Services.CreateScope();
IServiceProvider services = scope.ServiceProvider;

var initializer = services.GetRequiredService<DataInitialiser>();
await initializer.SeedAsync();

app.UseMiddleware<ExceptionMiddleware>();

app.UseHttpsRedirection();

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(Path.Combine(builder.Environment.ContentRootPath, "Images")),
    RequestPath = "/Images"
});

app.UseCors("AllowOnlyLocalhostOrigin");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
