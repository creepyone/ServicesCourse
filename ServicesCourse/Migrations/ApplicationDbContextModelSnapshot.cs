﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ServicesCourse.Models;

#nullable disable

namespace ServicesCourse.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("ServicesCourse.Models.History", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime>("AccessTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("CURRENT_TIMESTAMP");

                    b.Property<int>("ServiceId")
                        .HasColumnType("int");

                    b.HasKey("Login", "AccessTime");

                    b.HasIndex("ServiceId");

                    b.ToTable("History");
                });

            modelBuilder.Entity("ServicesCourse.Models.Section", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("SectionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Section");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SectionName = "Общее"
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.Service", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("AboutService")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("ActivityStatus")
                        .HasColumnType("bit");

                    b.Property<string>("ServiceName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("SubsectionId")
                        .HasColumnType("int");

                    b.Property<string>("Version")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SubsectionId");

                    b.ToTable("Service");
                });

            modelBuilder.Entity("ServicesCourse.Models.Sex", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("SexName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Sex");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SexName = "Мужской"
                        },
                        new
                        {
                            Id = 2,
                            SexName = "Женский"
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.Subsection", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<int>("SectionId")
                        .HasColumnType("int");

                    b.Property<string>("SubscetionName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Subsection");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            SectionId = 1,
                            SubscetionName = "Общее"
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.User", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<bool>("ActivityStatus")
                        .HasColumnType("bit");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)");

                    b.Property<int>("UserTypeId")
                        .HasColumnType("int");

                    b.HasKey("Login");

                    b.HasIndex("UserTypeId");

                    b.ToTable("User");

                    b.HasData(
                        new
                        {
                            Login = "admin",
                            ActivityStatus = true,
                            Password = "admin",
                            UserTypeId = 1
                        },
                        new
                        {
                            Login = "user",
                            ActivityStatus = true,
                            Password = "user",
                            UserTypeId = 2
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.UserProfile", b =>
                {
                    b.Property<string>("Login")
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTime?>("BirthDate")
                        .HasColumnType("date");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Patronymic")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("SexId")
                        .HasColumnType("int");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Login");

                    b.HasIndex("SexId");

                    b.ToTable("UserProfile");

                    b.HasData(
                        new
                        {
                            Login = "admin"
                        },
                        new
                        {
                            Login = "user"
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.UserType", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"), 1L, 1);

                    b.Property<string>("UserTypeName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("UserType");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            UserTypeName = "Администратор"
                        },
                        new
                        {
                            Id = 2,
                            UserTypeName = "Пользователь"
                        });
                });

            modelBuilder.Entity("ServicesCourse.Models.History", b =>
                {
                    b.HasOne("ServicesCourse.Models.User", "User")
                        .WithMany("HistoryRecords")
                        .HasForeignKey("Login")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicesCourse.Models.Service", "Service")
                        .WithMany("HistoryRecords")
                        .HasForeignKey("ServiceId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Service");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServicesCourse.Models.Service", b =>
                {
                    b.HasOne("ServicesCourse.Models.Subsection", null)
                        .WithMany("Services")
                        .HasForeignKey("SubsectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesCourse.Models.Subsection", b =>
                {
                    b.HasOne("ServicesCourse.Models.Section", null)
                        .WithMany("Subsections")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("ServicesCourse.Models.User", b =>
                {
                    b.HasOne("ServicesCourse.Models.UserType", "UserType")
                        .WithMany("Users")
                        .HasForeignKey("UserTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("UserType");
                });

            modelBuilder.Entity("ServicesCourse.Models.UserProfile", b =>
                {
                    b.HasOne("ServicesCourse.Models.User", "User")
                        .WithOne("UserProfile")
                        .HasForeignKey("ServicesCourse.Models.UserProfile", "Login")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ServicesCourse.Models.Sex", null)
                        .WithMany("UserProfiles")
                        .HasForeignKey("SexId");

                    b.Navigation("User");
                });

            modelBuilder.Entity("ServicesCourse.Models.Section", b =>
                {
                    b.Navigation("Subsections");
                });

            modelBuilder.Entity("ServicesCourse.Models.Service", b =>
                {
                    b.Navigation("HistoryRecords");
                });

            modelBuilder.Entity("ServicesCourse.Models.Sex", b =>
                {
                    b.Navigation("UserProfiles");
                });

            modelBuilder.Entity("ServicesCourse.Models.Subsection", b =>
                {
                    b.Navigation("Services");
                });

            modelBuilder.Entity("ServicesCourse.Models.User", b =>
                {
                    b.Navigation("HistoryRecords");

                    b.Navigation("UserProfile");
                });

            modelBuilder.Entity("ServicesCourse.Models.UserType", b =>
                {
                    b.Navigation("Users");
                });
#pragma warning restore 612, 618
        }
    }
}
