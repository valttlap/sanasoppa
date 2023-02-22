﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;
using Sanasoppa.API.Data;

#nullable disable

namespace Sanasoppa.API.Migrations
{
    [DbContext(typeof(DataContext))]
    partial class DataContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.2")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("Sanasoppa.API.Entities.Explanation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<bool>("IsRight")
                        .HasColumnType("boolean");

                    b.Property<int>("PlayerId")
                        .HasColumnType("integer");

                    b.Property<int>("RoundId")
                        .HasColumnType("integer");

                    b.Property<string>("Text")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("PlayerId");

                    b.HasIndex("RoundId");

                    b.ToTable("Explanation");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("ConnectionId")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId")
                        .IsUnique();

                    b.ToTable("Games");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Player", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ConnectionId")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<bool>("IsDasher")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("ConnectionId")
                        .IsUnique();

                    b.HasIndex("GameId");

                    b.ToTable("Players");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Round", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<int>("GameId")
                        .HasColumnType("integer");

                    b.Property<string>("Word")
                        .IsRequired()
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.HasIndex("GameId")
                        .IsUnique();

                    b.ToTable("Rounds");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Explanation", b =>
                {
                    b.HasOne("Sanasoppa.API.Entities.Player", "Player")
                        .WithMany("Explanations")
                        .HasForeignKey("PlayerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Sanasoppa.API.Entities.Round", "Round")
                        .WithMany("Explanations")
                        .HasForeignKey("RoundId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Player");

                    b.Navigation("Round");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Player", b =>
                {
                    b.HasOne("Sanasoppa.API.Entities.Game", "Game")
                        .WithMany("Players")
                        .HasForeignKey("GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Round", b =>
                {
                    b.HasOne("Sanasoppa.API.Entities.Game", "Game")
                        .WithOne("CurrentRound")
                        .HasForeignKey("Sanasoppa.API.Entities.Round", "GameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Game");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Game", b =>
                {
                    b.Navigation("CurrentRound");

                    b.Navigation("Players");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Player", b =>
                {
                    b.Navigation("Explanations");
                });

            modelBuilder.Entity("Sanasoppa.API.Entities.Round", b =>
                {
                    b.Navigation("Explanations");
                });
#pragma warning restore 612, 618
        }
    }
}