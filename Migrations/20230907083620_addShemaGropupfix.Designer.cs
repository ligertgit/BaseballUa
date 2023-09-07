﻿// <auto-generated />
using System;
using BaseballUa.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace BaseballUa.Migrations
{
    [DbContext(typeof(BaseballUaDbContext))]
    [Migration("20230907083620_addShemaGropupfix")]
    partial class addShemaGropupfix
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.10")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("BaseballUa.Models.Category", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ShortName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("nvarchar(15)");

                    b.HasKey("Id");

                    b.ToTable("Categories");
                });

            modelBuilder.Entity("BaseballUa.Models.Event", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime?>("EndDate")
                        .HasColumnType("datetime2");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int>("TournamentId")
                        .HasColumnType("int");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("TournamentId");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("BaseballUa.Models.EventSchemaItem", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventId")
                        .HasColumnType("int");

                    b.Property<int>("Order")
                        .HasColumnType("int");

                    b.Property<int>("SchemaItem")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.ToTable("EventSchemaItems");
                });

            modelBuilder.Entity("BaseballUa.Models.Game", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("AdditionalInfo")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<string>("ConditionHome")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("ConditionVisitor")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("GameStatus")
                        .HasColumnType("int");

                    b.Property<int?>("HalfinningsPlayed")
                        .HasColumnType("int");

                    b.Property<string>("PlacedAt")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int?>("PointsHome")
                        .HasColumnType("int");

                    b.Property<int?>("PointsVisitor")
                        .HasColumnType("int");

                    b.Property<int?>("RunsHome")
                        .HasColumnType("int");

                    b.Property<int?>("RunsVisitor")
                        .HasColumnType("int");

                    b.Property<int>("SchemaGroupId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("StartDate")
                        .HasColumnType("datetime2");

                    b.Property<int?>("Tour")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SchemaGroupId");

                    b.ToTable("Games");
                });

            modelBuilder.Entity("BaseballUa.Models.SchemaGroup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("EventSchemaItemId")
                        .HasColumnType("int");

                    b.Property<string>("GroupName")
                        .IsRequired()
                        .HasMaxLength(3)
                        .HasColumnType("nvarchar(3)");

                    b.HasKey("Id");

                    b.HasIndex("EventSchemaItemId");

                    b.ToTable("SchemaGroups");
                });

            modelBuilder.Entity("BaseballUa.Models.Tournament", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CategoryId")
                        .HasColumnType("int");

                    b.Property<string>("Description")
                        .HasMaxLength(250)
                        .HasColumnType("nvarchar(250)");

                    b.Property<bool>("IsAnual")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsFun")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsInternational")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<bool>("IsOfficial")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bit")
                        .HasDefaultValue(false);

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<int>("Sport")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasDefaultValue(3);

                    b.HasKey("Id");

                    b.HasIndex("CategoryId");

                    b.ToTable("Tournaments");
                });

            modelBuilder.Entity("BaseballUa.Models.Event", b =>
                {
                    b.HasOne("BaseballUa.Models.Tournament", "Tournament")
                        .WithMany("Events")
                        .HasForeignKey("TournamentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Tournament");
                });

            modelBuilder.Entity("BaseballUa.Models.EventSchemaItem", b =>
                {
                    b.HasOne("BaseballUa.Models.Event", "Event")
                        .WithMany("EventSchemaItems")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");
                });

            modelBuilder.Entity("BaseballUa.Models.Game", b =>
                {
                    b.HasOne("BaseballUa.Models.SchemaGroup", "SchemaGroup")
                        .WithMany("Games")
                        .HasForeignKey("SchemaGroupId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("SchemaGroup");
                });

            modelBuilder.Entity("BaseballUa.Models.SchemaGroup", b =>
                {
                    b.HasOne("BaseballUa.Models.EventSchemaItem", "EventSchemaItem")
                        .WithMany("SchemaGroups")
                        .HasForeignKey("EventSchemaItemId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("EventSchemaItem");
                });

            modelBuilder.Entity("BaseballUa.Models.Tournament", b =>
                {
                    b.HasOne("BaseballUa.Models.Category", "Category")
                        .WithMany("Tournaments")
                        .HasForeignKey("CategoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Category");
                });

            modelBuilder.Entity("BaseballUa.Models.Category", b =>
                {
                    b.Navigation("Tournaments");
                });

            modelBuilder.Entity("BaseballUa.Models.Event", b =>
                {
                    b.Navigation("EventSchemaItems");
                });

            modelBuilder.Entity("BaseballUa.Models.EventSchemaItem", b =>
                {
                    b.Navigation("SchemaGroups");
                });

            modelBuilder.Entity("BaseballUa.Models.SchemaGroup", b =>
                {
                    b.Navigation("Games");
                });

            modelBuilder.Entity("BaseballUa.Models.Tournament", b =>
                {
                    b.Navigation("Events");
                });
#pragma warning restore 612, 618
        }
    }
}
