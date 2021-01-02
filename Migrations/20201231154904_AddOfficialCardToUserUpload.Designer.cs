﻿// <auto-generated />
using System;
using Backend.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Backend.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20201231154904_AddOfficialCardToUserUpload")]
    partial class AddOfficialCardToUserUpload
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.1");

            modelBuilder.Entity("Backend.Entities.User", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("AccountNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BVN")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("BankName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ContributionLimit")
                        .HasColumnType("float");

                    b.Property<bool>("ContributionLimitRequested")
                        .HasColumnType("bit");

                    b.Property<bool>("ContributionLimitSet")
                        .HasColumnType("bit");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateEdited")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateOfBirth")
                        .HasColumnType("datetime2");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("EmailConfirmationAttempts")
                        .HasColumnType("int");

                    b.Property<int>("EmailConfirmationCode")
                        .HasColumnType("int");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("FirstName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HiDee")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("HomeAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ImageNames")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Integration")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("LastSeen")
                        .HasColumnType("datetime2");

                    b.Property<string>("Lga")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty0")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<byte[]>("PasswordHash")
                        .HasColumnType("varbinary(max)");

                    b.Property<byte[]>("PasswordSalt")
                        .HasColumnType("varbinary(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceOfWorkAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PlaceOfWorkName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RelatedAccounts")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Role")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("State")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SubAccountCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Surname")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserCookie")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserCookieChangeCounter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserNumberOfRelatedAccounts")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("Backend.Entities.UserUpload", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("BankStatement")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateAdded")
                        .HasColumnType("datetime2");

                    b.Property<string>("ImageNames")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty0")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty1")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty2")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty3")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty4")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty5")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty6")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty7")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty8")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MyProperty9")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("OfficialIDCard")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProfilePicture")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("UtilityBill")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("WorkIDCard")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("UserUploads");
                });

            modelBuilder.Entity("Backend.Entities.UserUpload", b =>
                {
                    b.HasOne("Backend.Entities.User", "User")
                        .WithMany("UserUpload")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("User");
                });

            modelBuilder.Entity("Backend.Entities.User", b =>
                {
                    b.Navigation("UserUpload");
                });
#pragma warning restore 612, 618
        }
    }
}
