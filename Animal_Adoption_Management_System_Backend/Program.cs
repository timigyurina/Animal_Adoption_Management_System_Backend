using Animal_Adoption_Management_System_Backend.Configurations;
using Animal_Adoption_Management_System_Backend.Data;
using Animal_Adoption_Management_System_Backend.Models.Entities;
using Animal_Adoption_Management_System_Backend.Repositories;
using Animal_Adoption_Management_System_Backend.Services.Implementations;
using Animal_Adoption_Management_System_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string connectionString = builder.Configuration.GetConnectionString("AnimalAdoptionConnection") ?? throw new InvalidOperationException("Connection string 'AnimalAdoptionConnection' was not found.");


builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AnimalAdoptionContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddIdentityCore<User>()
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<AnimalAdoptionContext>();

builder.Services.AddCors(options => options.AddPolicy("AllowAll", builder =>
{
    builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
}));


builder.Services.AddAutoMapper(typeof(AutoMapperConfiguration));

builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IAnimalBreedService, AnimalBreedService>();
builder.Services.AddScoped<IAnimalService, AnimalService>();
builder.Services.AddScoped<IShelterService, ShelterService>();
builder.Services.AddScoped<IAnimalShelterService, AnimalShelterService>();
builder.Services.AddScoped<IDonationService, DonationService>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IAdoptionApplicationService, AdoptionApplicationService>();
builder.Services.AddScoped<IAdoptionContractService, AdoptionContractService>();
builder.Services.AddScoped<IManagedAdoptionContractService, ManagedAdoptionContractService>();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddTransient<DataInitialiser>();



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

app.UseHttpsRedirection();

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();

app.Run();
