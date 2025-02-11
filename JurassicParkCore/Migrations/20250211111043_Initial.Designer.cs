﻿// <auto-generated />
using System;
using JurassicParkCore.DataSchemas;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace JurassicParkCore.Migrations
{
    [DbContext(typeof(JurassicParkDbContext))]
    [Migration("20250211111043_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "9.0.1");

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Animal", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("AnimalTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("GroupId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Health")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("HungerLevel")
                        .HasColumnType("TEXT");

                    b.Property<int?>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("ThirstLevel")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("AnimalTypeId");

                    b.HasIndex("GroupId");

                    b.HasIndex("PositionId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("AnimalTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.AnimalGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("GroupTypeId")
                        .HasColumnType("INTEGER");

                    b.Property<int?>("NextPointOfInterestId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("GroupTypeId");

                    b.HasIndex("NextPointOfInterestId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("AnimalGroupTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.AnimalType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("EatingHabit")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Price")
                        .HasColumnType("TEXT");

                    b.Property<decimal>("VisitorSatisfaction")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("AnimalTypeTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Jeep", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("PositionId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SeatedVisitors")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("PositionId");

                    b.HasIndex("SavedGameId");

                    b.ToTable("JeepTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.MapObject", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavedGameId");

                    b.ToTable("MapObjectTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Position", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<double>("X")
                        .HasColumnType("REAL");

                    b.Property<double>("Y")
                        .HasColumnType("REAL");

                    b.HasKey("Id");

                    b.ToTable("PositionTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.SavedGame", b =>
                {
                    b.Property<int>("Id")
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

                    b.Property<string>("MapSeed")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("MapSizeId")
                        .HasColumnType("INTEGER");

                    b.Property<TimeOnly>("TimeOfDay")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("MapSizeId");

                    b.ToTable("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Transaction", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("Amount")
                        .HasColumnType("TEXT");

                    b.Property<bool>("IsCheckpoint")
                        .HasColumnType("INTEGER");

                    b.Property<int>("SavedGameId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("Type")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("SavedGameId");

                    b.ToTable("TransactionTable");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Animal", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.AnimalType", "AnimalType")
                        .WithMany()
                        .HasForeignKey("AnimalTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicParkCore.DataSchemas.AnimalGroup", "Group")
                        .WithMany("Animals")
                        .HasForeignKey("GroupId");

                    b.HasOne("JurassicParkCore.DataSchemas.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("JurassicParkCore.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Animals")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AnimalType");

                    b.Navigation("Group");

                    b.Navigation("Position");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.AnimalGroup", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.AnimalType", "GroupType")
                        .WithMany()
                        .HasForeignKey("GroupTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("JurassicParkCore.DataSchemas.Position", "NextPointOfInterest")
                        .WithMany()
                        .HasForeignKey("NextPointOfInterestId");

                    b.HasOne("JurassicParkCore.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("AnimalGroups")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("GroupType");

                    b.Navigation("NextPointOfInterest");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Jeep", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.Position", "Position")
                        .WithMany()
                        .HasForeignKey("PositionId");

                    b.HasOne("JurassicParkCore.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Jeeps")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Position");

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.MapObject", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("MapObjects")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.SavedGame", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.Position", "MapSize")
                        .WithMany()
                        .HasForeignKey("MapSizeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("MapSize");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.Transaction", b =>
                {
                    b.HasOne("JurassicParkCore.DataSchemas.SavedGame", "SavedGame")
                        .WithMany("Transactions")
                        .HasForeignKey("SavedGameId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SavedGame");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.AnimalGroup", b =>
                {
                    b.Navigation("Animals");
                });

            modelBuilder.Entity("JurassicParkCore.DataSchemas.SavedGame", b =>
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
