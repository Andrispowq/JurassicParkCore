﻿// <auto-generated />
using System;
using JurassicPark.Core.DataSchemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JurassicPark.Core.Migrations
{
    [DbContext(typeof(JurassicParkDbContext))]
    partial class JurassicParkDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.3");

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Animal", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("Age")
                        .HasColumnType("INTEGER");

                    b.Property<long>("AnimalTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<bool>("HasChip")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Health")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HungerLevel")
                        .HasColumnType("TEXT");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(30)
                        .HasColumnType("TEXT");

                    b.Property<long?>("PointOfInterestId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Sex")
                        .HasColumnType("INTEGER");

                    b.Property<int>("State")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ThirstLevel")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PointOfInterestId");

                    b.HasIndex("PositionId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("AnimalTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.AnimalGroup", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("GroupTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long?>("NextPointOfInterestId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GroupTypeId");

                    b.HasIndex("NextPointOfInterestId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("AnimalGroupTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.AnimalType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EatingHabit")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("VisitorSatisfaction")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("AnimalTypeTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Discovered", b =>
                {
                    b.Property<long>("AnimalId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("MapObjectId")
                        .HasColumnType("INTEGER");

                    b.HasKey("AnimalId", "MapObjectId");

                    b.HasIndex("MapObjectId");

                    b.ToTable("DiscoveredTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Jeep", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("RouteId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("RouteProgression")
                        .HasColumnType("TEXT");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeatedVisitors")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("RouteId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("JeepTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.JeepRoute", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavedGameId");

                    b.ToTable("JeepRouteTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.MapObject", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long>("MapObjectTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<long>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ResourceAmount")
                        .HasColumnType("TEXT");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("MapObjectTypeId");

                    b.HasIndex("PositionId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("MapObjectTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.MapObjectType", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("TEXT");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("ResourceAmount")
                        .HasColumnType("TEXT");

                    b.Property<int>("ResourceType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("MapObjectTypeTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Position", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<long?>("JeepRouteId")
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.HasIndex("JeepRouteId");

                    b.ToTable("PositionTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.SavedGame", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<int>("DaysPassed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Difficulty")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameSpeed")
                        .HasColumnType("INTEGER");

                    b.Property<int>("GameState")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("HoursSinceGoalMet")
                        .HasColumnType("TEXT");

                    b.Property<long>("MapHeight")
                        .HasColumnType("INTEGER");

                    b.Property<string>("MapSeed")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("TEXT");

                    b.Property<long>("MapWidth")
                        .HasColumnType("INTEGER");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("TEXT");

                    b.Property<TimeOnly>("TimeOfDay")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("VisitorSatisfaction")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("Name")
                        .IsUnique();

                    b.ToTable("SavedGameTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Transaction", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<DateTime>("CreatedAt")
                        .HasColumnType("TEXT");

                    b.Property<long>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavedGameId");

                    b.ToTable("TransactionTable");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Animal", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.AnimalType", "AnimalType")
                        .WithMany()
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.AnimalGroup", "Group")
                        .WithMany("Animals")
                        .HasForeignKey("GroupId");

                    b.HasOne("JurassicPark.Core.DataSchemas.Position", "PointOfInterest")
                        .WithMany()
                        .HasForeignKey("PointOfInterestId");

                    b.HasOne("JurassicPark.Core.DataSchemas.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Animals")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimalType");

                    b.Navigation("Group");

                    b.Navigation("PointOfInterest");

                    b.Navigation("Position");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.AnimalGroup", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.AnimalType", "GroupType")
                        .WithMany()
                        .HasForeignKey("GroupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.Position", "NextPointOfInterest")
                        .WithMany()
                        .HasForeignKey("NextPointOfInterestId");

                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("AnimalGroups")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupType");

                    b.Navigation("NextPointOfInterest");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Discovered", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.Animal", "Animal")
                        .WithMany("DiscoveredMapObjects")
                        .HasForeignKey("AnimalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.MapObject", "MapObject")
                        .WithMany("DiscoveredByAnimals")
                        .HasForeignKey("MapObjectId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Animal");

                    b.Navigation("MapObject");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Jeep", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.JeepRoute", "Route")
                        .WithMany()
                        .HasForeignKey("RouteId");

                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Jeeps")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Route");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.JeepRoute", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany()
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.MapObject", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.MapObjectType", "MapObjectType")
                        .WithMany()
                        .HasForeignKey("MapObjectTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("MapObjects")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MapObjectType");

                    b.Navigation("Position");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Position", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.JeepRoute", null)
                        .WithMany("RoutePositions")
                        .HasForeignKey("JeepRouteId");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Transaction", b =>
                {
                    b.HasOne("JurassicPark.Core.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Transactions")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.Animal", b =>
                {
                    b.Navigation("DiscoveredMapObjects");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.AnimalGroup", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.JeepRoute", b =>
                {
                    b.Navigation("RoutePositions");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.MapObject", b =>
                {
                    b.Navigation("DiscoveredByAnimals");
                });

            modelBuilder.Entity("JurassicPark.Core.DataSchemas.SavedGame", b =>
                {
                    b.Navigation("AnimalGroups");

                    b.Navigation("Animals");

                    b.Navigation("Jeeps");

                    b.Navigation("MapObjects");

                    b.Navigation("Transactions");
                });
#pragma warning restore 612, 618
        }
    }
}
