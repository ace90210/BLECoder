﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using RadzenTemplate.EntityFrameworkCore.SqlServer.Contexts;

namespace RadzenTemplate.EntityFrameworkCore.SqlServer.Migrations
{
    [DbContext(typeof(RadzenTemplateContext))]
    [Migration("20210615180053_InitialMigration")]
    partial class InitialMigration
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.4")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("RadzenTemplate.EntityFrameworkCore.SqlServer.JsonBlob", b =>
                {
                    b.Property<string>("Key")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("Data")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Key");

                    b.ToTable("JsonBlobs");
                });

            modelBuilder.Entity("RadzenTemplate.EntityFrameworkCore.SqlServer.Models.UserConfiguration", b =>
                {
                    b.Property<string>("UserUniqueIdentifier")
                        .HasMaxLength(128)
                        .HasColumnType("nvarchar(128)");

                    b.Property<int>("LoginCount")
                        .HasColumnType("int");

                    b.Property<string>("PreferredTheme")
                        .HasMaxLength(32)
                        .HasColumnType("nvarchar(32)");

                    b.Property<string>("SiteNickname")
                        .HasMaxLength(16)
                        .HasColumnType("nvarchar(16)");

                    b.HasKey("UserUniqueIdentifier");

                    b.ToTable("UserConfigurations");
                });
#pragma warning restore 612, 618
        }
    }
}