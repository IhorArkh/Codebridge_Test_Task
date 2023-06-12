﻿// <auto-generated />
using DogsAPI.DB;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DogsAPI.Migrations
{
    [DbContext(typeof(DogsContext))]
    [Migration("20230612122701_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("DogsAPI.DB.Models.Dog", b =>
                {
                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)")
                        .HasColumnName("name");

                    b.Property<string>("Color")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)")
                        .HasColumnName("color");

                    b.Property<int>("TailLength")
                        .HasColumnType("int")
                        .HasColumnName("tail_length");

                    b.Property<int>("Weight")
                        .HasColumnType("int")
                        .HasColumnName("weight");

                    b.HasKey("Name");

                    b.ToTable("Dogs");

                    b.HasData(
                        new
                        {
                            Name = "Jessy",
                            Color = "red & amber",
                            TailLength = 22,
                            Weight = 32
                        },
                        new
                        {
                            Name = "Neo",
                            Color = "black & whiteeee",
                            TailLength = 7,
                            Weight = 14
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
