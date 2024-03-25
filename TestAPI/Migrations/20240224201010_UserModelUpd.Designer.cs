﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using WebAPI.Logic;

#nullable disable

namespace WebAPI.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20240224201010_UserModelUpd")]
    partial class UserModelUpd
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("WebAPI.Models.Developer", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("LogoURL")
                        .HasColumnType("text");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("ID");

                    b.ToTable("Developer");
                });

            modelBuilder.Entity("WebAPI.Models.Game", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("AchievementsAmount")
                        .HasColumnType("integer");

                    b.Property<int>("DeveloperID")
                        .HasColumnType("integer");

                    b.Property<string>("LogoURL")
                        .HasColumnType("text");

                    b.Property<float>("PriceUsd")
                        .HasColumnType("real");

                    b.Property<DateTime>("PublishDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.HasKey("ID");

                    b.HasIndex("DeveloperID");

                    b.ToTable("Game");
                });

            modelBuilder.Entity("WebAPI.Models.GameStats", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<int>("AchievementsGotten")
                        .HasColumnType("integer");

                    b.Property<int>("GameID")
                        .HasColumnType("integer");

                    b.Property<float>("HoursPlayed")
                        .HasColumnType("real");

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.HasIndex("UserID");

                    b.ToTable("GameStats");
                });

            modelBuilder.Entity("WebAPI.Models.Review", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<int>("GameID")
                        .HasColumnType("integer");

                    b.Property<bool>("IsPositive")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("LastEditDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Text")
                        .HasMaxLength(9999)
                        .HasColumnType("character varying(9999)");

                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.HasKey("ID");

                    b.HasIndex("GameID");

                    b.HasIndex("UserID");

                    b.ToTable("Review");
                });

            modelBuilder.Entity("WebAPI.Models.User", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("ID"));

                    b.Property<float>("BalanceUSD")
                        .HasColumnType("real");

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("timestamp with time zone");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)");

                    b.Property<string>("Login")
                        .IsRequired()
                        .HasMaxLength(99)
                        .HasColumnType("character varying(99)");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<string>("ProfilePictureURL")
                        .HasColumnType("text");

                    b.HasKey("ID");

                    b.ToTable("User");
                });

            modelBuilder.Entity("WebAPI.Models.UserGame", b =>
                {
                    b.Property<int>("UserID")
                        .HasColumnType("integer");

                    b.Property<int>("GameID")
                        .HasColumnType("integer");

                    b.HasKey("UserID", "GameID");

                    b.HasIndex("GameID");

                    b.ToTable("UserGame");
                });

            modelBuilder.Entity("WebAPI.Models.Game", b =>
                {
                    b.HasOne("WebAPI.Models.Developer", "Developer")
                        .WithMany("PublishedGames")
                        .HasForeignKey("DeveloperID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Developer");
                });

            modelBuilder.Entity("WebAPI.Models.GameStats", b =>
                {
                    b.HasOne("WebAPI.Models.Game", "Game")
                        .WithMany()
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.Models.User", "User")
                        .WithMany("GamesStats")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI.Models.Review", b =>
                {
                    b.HasOne("WebAPI.Models.Game", "Game")
                        .WithMany("Reviews")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.Models.User", "User")
                        .WithMany()
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI.Models.UserGame", b =>
                {
                    b.HasOne("WebAPI.Models.Game", "Game")
                        .WithMany("UserGames")
                        .HasForeignKey("GameID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebAPI.Models.User", "User")
                        .WithMany("UserGames")
                        .HasForeignKey("UserID")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");

                    b.Navigation("User");
                });

            modelBuilder.Entity("WebAPI.Models.Developer", b =>
                {
                    b.Navigation("PublishedGames");
                });

            modelBuilder.Entity("WebAPI.Models.Game", b =>
                {
                    b.Navigation("Reviews");

                    b.Navigation("UserGames");
                });

            modelBuilder.Entity("WebAPI.Models.User", b =>
                {
                    b.Navigation("GamesStats");

                    b.Navigation("UserGames");
                });
#pragma warning restore 612, 618
        }
    }
}
