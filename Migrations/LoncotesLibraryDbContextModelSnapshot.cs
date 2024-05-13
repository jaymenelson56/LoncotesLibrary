﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace LoncotesLibrary.Migrations
{
    [DbContext(typeof(LoncotesLibraryDbContext))]
    partial class LoncotesLibraryDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Library.Checkout", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("CheckoutDate")
                        .HasColumnType("timestamp without time zone");

                    b.Property<int>("MaterialId")
                        .HasColumnType("integer");

                    b.Property<int>("PatronId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("ReturnDate")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("MaterialId");

                    b.HasIndex("PatronId");

                    b.ToTable("Checkouts");
                });

            modelBuilder.Entity("Library.Genre", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Genres");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Name = "SciFi"
                        },
                        new
                        {
                            Id = 2,
                            Name = "History"
                        },
                        new
                        {
                            Id = 3,
                            Name = "Fantasy"
                        },
                        new
                        {
                            Id = 4,
                            Name = "Mystery"
                        },
                        new
                        {
                            Id = 5,
                            Name = "Romance"
                        });
                });

            modelBuilder.Entity("Library.Material", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GenreId")
                        .HasColumnType("integer");

                    b.Property<string>("MaterialName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("MaterialTypeId")
                        .HasColumnType("integer");

                    b.Property<DateTime?>("OutOfCirculationSince")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.HasIndex("GenreId");

                    b.HasIndex("MaterialTypeId");

                    b.ToTable("Materials");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            GenreId = 1,
                            MaterialName = "Clifford: The Puppy Years",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 2,
                            GenreId = 1,
                            MaterialName = "Hands-On Programming with C#",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 3,
                            GenreId = 2,
                            MaterialName = "National Geographic",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 4,
                            GenreId = 5,
                            MaterialName = "Spice Girls: Greatest Hits",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 5,
                            GenreId = 3,
                            MaterialName = "Arthur's New Puppy",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 6,
                            GenreId = 4,
                            MaterialName = "DC Comics",
                            MaterialTypeId = 2
                        },
                        new
                        {
                            Id = 7,
                            GenreId = 5,
                            MaterialName = "Infest by Papa Roach",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 8,
                            GenreId = 4,
                            MaterialName = "House of the Dead: Overkill",
                            MaterialTypeId = 1
                        },
                        new
                        {
                            Id = 9,
                            GenreId = 4,
                            MaterialName = "Edgar Allan Poe: The Complete Tales and Poems",
                            MaterialTypeId = 3
                        },
                        new
                        {
                            Id = 10,
                            GenreId = 5,
                            MaterialName = "The Beef Chronicles: Kendrick vs. Drake",
                            MaterialTypeId = 1
                        });
                });

            modelBuilder.Entity("Library.MaterialType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("CheckoutDays")
                        .HasColumnType("integer");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("MaterialTypes");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            CheckoutDays = 14,
                            Name = "Book"
                        },
                        new
                        {
                            Id = 2,
                            CheckoutDays = 7,
                            Name = "Periodical"
                        },
                        new
                        {
                            Id = 3,
                            CheckoutDays = 10,
                            Name = "CD"
                        });
                });

            modelBuilder.Entity("Library.Patron", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("Address")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean");

                    b.Property<string>("LastName")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Patrons");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Address = "123 Main St",
                            Email = "john@example.com",
                            FirstName = "Gator",
                            IsActive = true,
                            LastName = "Golf"
                        },
                        new
                        {
                            Id = 2,
                            Address = "456 Elm St",
                            Email = "jane@example.com",
                            FirstName = "Chicken",
                            IsActive = true,
                            LastName = "Limbo"
                        });
                });

            modelBuilder.Entity("Library.Checkout", b =>
                {
                    b.HasOne("Library.Material", "Material")
                        .WithMany()
                        .HasForeignKey("MaterialId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.Patron", "Patron")
                        .WithMany()
                        .HasForeignKey("PatronId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Material");

                    b.Navigation("Patron");
                });

            modelBuilder.Entity("Library.Material", b =>
                {
                    b.HasOne("Library.Genre", "Genre")
                        .WithMany()
                        .HasForeignKey("GenreId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Library.MaterialType", "MaterialType")
                        .WithMany()
                        .HasForeignKey("MaterialTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Genre");

                    b.Navigation("MaterialType");
                });
#pragma warning restore 612, 618
        }
    }
}
