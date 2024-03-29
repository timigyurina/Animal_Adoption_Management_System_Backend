﻿// <auto-generated />
using System;
using Animal_Adoption_Management_System_Backend.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Animal_Adoption_Management_System_Backend.Migrations
{
    [DbContext(typeof(AnimalAdoptionContext))]
    [Migration("20230403060510_UpdateShelterAndEmployeeUserConnection")]
    partial class UpdateShelterAndEmployeeUserConnection
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Address", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("AddressLineOne")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("AddressLineTwo")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("City")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Region")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Addresses");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AdoptionApplication", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("ApplicationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("ApplierId")
                        .HasColumnType("text");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("ApplierId");

                    b.ToTable("AdoptionApplications");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AdoptionContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalId")
                        .HasColumnType("integer");

                    b.Property<int>("ApplierAddressId")
                        .HasColumnType("integer");

                    b.Property<string>("ApplierId")
                        .HasColumnType("text");

                    b.Property<DateTime>("ContractDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("ApplierAddressId");

                    b.HasIndex("ApplierId");

                    b.ToTable("AdoptionContracts");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("BirthDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("BreedId")
                        .HasColumnType("integer");

                    b.Property<int>("Color")
                        .HasColumnType("integer");

                    b.Property<int>("Gender")
                        .HasColumnType("integer");

                    b.Property<bool>("IsSterilised")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Notes")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("SterilisationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("BreedId");

                    b.ToTable("Animals");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AnimalBreed", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("Type")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("AnimalBreeds");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AnimalShelter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("EnrollmentDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<DateTime?>("ExitDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("ShelterId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("ShelterId");

                    b.ToTable("AnimalShelters");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Donation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<decimal>("Amount")
                        .HasColumnType("numeric");

                    b.Property<DateTime>("Date")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("DonatorId")
                        .HasColumnType("text");

                    b.Property<int>("ShelterId")
                        .HasColumnType("integer");

                    b.Property<int>("Status")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("DonatorId");

                    b.HasIndex("ShelterId");

                    b.ToTable("Donations");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AnimalId")
                        .HasColumnType("integer");

                    b.Property<DateTime>("DateTaken")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ImagePath")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("UploaderId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AnimalId");

                    b.HasIndex("UploaderId");

                    b.ToTable("Images");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.ManagedAdoptionContract", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ContractId")
                        .HasColumnType("integer");

                    b.Property<string>("ManagerId")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ContractId");

                    b.HasIndex("ManagerId");

                    b.ToTable("ManagedAdoptionContracts");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("AddressId")
                        .HasColumnType("integer");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Phone")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("AddressId");

                    b.ToTable("Shelters");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.User", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsContactOfShelter")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text");

                    b.Property<int?>("ShelterId")
                        .HasColumnType("integer");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.HasIndex("ShelterId");

                    b.ToTable("AspNetUsers", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                            AccessFailedCount = 0,
                            ConcurrencyStamp = "210ebb69-53d2-46cd-9bb4-7e6f3c84d311",
                            DateOfBirth = new DateTime(1990, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc),
                            Email = "admin@localhost.com",
                            EmailConfirmed = true,
                            FirstName = "System",
                            IsActive = true,
                            IsContactOfShelter = false,
                            LastName = "Admin",
                            LockoutEnabled = false,
                            NormalizedEmail = "ADMIN@LOCALHOST.COM",
                            NormalizedUserName = "ADMIN@LOCALHOST.COM",
                            PasswordHash = "AQAAAAEAACcQAAAAECRoF9KHAcIq2ZGtix32ZX5mOLrhaIcHDohBvRQv+pdnLvAoFoDvqJqE+hkF6yB1Eg==",
                            PhoneNumberConfirmed = false,
                            SecurityStamp = "ddea045a-d74f-474e-ac61-b9e84375972e",
                            TwoFactorEnabled = false,
                            UserName = "admin@localhost.com"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);

                    b.HasData(
                        new
                        {
                            Id = "abcd1234-82b5-4ef3-95b9-f33f6130d5ac",
                            ConcurrencyStamp = "6652b107-5655-4d9f-812c-f51200dbf437",
                            Name = "Administrator",
                            NormalizedName = "ADMINISTRATOR"
                        },
                        new
                        {
                            Id = "aaaa1111-82b5-4ef3-95b9-f33f6130d5ac",
                            ConcurrencyStamp = "175b3504-1234-4e95-8b8f-5a02e15f2589",
                            Name = "ShelterEmployee",
                            NormalizedName = "SHELTEREMPLOYEE"
                        },
                        new
                        {
                            Id = "bbbb2222-82b5-4ef3-95b9-f33f6130d5ac",
                            ConcurrencyStamp = "6f75cbcb-0573-448c-a7fb-ce6f62e9389a",
                            Name = "Adopter",
                            NormalizedName = "ADOPTER"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("RoleId")
                        .HasColumnType("text");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);

                    b.HasData(
                        new
                        {
                            UserId = "1bdd9ba1-82b5-4ef3-95b9-f13f6150d5ac",
                            RoleId = "abcd1234-82b5-4ef3-95b9-f33f6130d5ac"
                        });
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<string>("Value")
                        .HasColumnType("text");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AdoptionApplication", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", "Animal")
                        .WithMany("AdoptionApplications")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", "Applier")
                        .WithMany("AdoptionApplications")
                        .HasForeignKey("ApplierId");

                    b.Navigation("Animal");

                    b.Navigation("Applier");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AdoptionContract", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", "Animal")
                        .WithMany("AdoptionContracts")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Address", "ApplierAddress")
                        .WithMany()
                        .HasForeignKey("ApplierAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", "Applier")
                        .WithMany("AdoptionsContracts")
                        .HasForeignKey("ApplierId");

                    b.Navigation("Animal");

                    b.Navigation("Applier");

                    b.Navigation("ApplierAddress");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.AnimalBreed", "Breed")
                        .WithMany()
                        .HasForeignKey("BreedId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Breed");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.AnimalShelter", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", "Animal")
                        .WithMany("AnimalShelters")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", "Shelter")
                        .WithMany("Animals")
                        .HasForeignKey("ShelterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("Shelter");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Donation", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", "Donator")
                        .WithMany("Donations")
                        .HasForeignKey("DonatorId");

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", "Shelter")
                        .WithMany("Donations")
                        .HasForeignKey("ShelterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Donator");

                    b.Navigation("Shelter");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Image", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", "Animal")
                        .WithMany("Images")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", "Uploader")
                        .WithMany("Images")
                        .HasForeignKey("UploaderId");

                    b.Navigation("Animal");

                    b.Navigation("Uploader");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.ManagedAdoptionContract", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.AdoptionContract", "Contract")
                        .WithMany()
                        .HasForeignKey("ContractId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", "Manager")
                        .WithMany("ManagedAdoptionsContracts")
                        .HasForeignKey("ManagerId");

                    b.Navigation("Contract");

                    b.Navigation("Manager");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Address", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Address");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.User", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", "Shelter")
                        .WithMany("Employees")
                        .HasForeignKey("ShelterId");

                    b.Navigation("Shelter");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("Animal_Adoption_Management_System_Backend.Models.Entities.User", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Animal", b =>
                {
                    b.Navigation("AdoptionApplications");

                    b.Navigation("AdoptionContracts");

                    b.Navigation("AnimalShelters");

                    b.Navigation("Images");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.Shelter", b =>
                {
                    b.Navigation("Animals");

                    b.Navigation("Donations");

                    b.Navigation("Employees");
                });

            modelBuilder.Entity("Animal_Adoption_Management_System_Backend.Models.Entities.User", b =>
                {
                    b.Navigation("AdoptionApplications");

                    b.Navigation("AdoptionsContracts");

                    b.Navigation("Donations");

                    b.Navigation("Images");

                    b.Navigation("ManagedAdoptionsContracts");
                });
#pragma warning restore 612, 618
        }
    }
}
