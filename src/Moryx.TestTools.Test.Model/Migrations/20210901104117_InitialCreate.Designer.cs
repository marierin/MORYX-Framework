﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Moryx.TestTools.Test.Model;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

namespace Moryx.TestTools.Test.Model.Migrations
{
    [DbContext(typeof(TestModelContext))]
    [Migration("20210901104117_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("public")
                .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn)
                .HasAnnotation("ProductVersion", "3.1.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            modelBuilder.Entity("Moryx.TestTools.Test.Model.CarEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("Deleted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("text");

                    b.Property<byte[]>("Image")
                        .HasColumnType("bytea");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Price")
                        .HasColumnType("integer");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Cars");

                    b.HasDiscriminator<string>("Discriminator").HasValue("CarEntity");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.HouseEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<DateTime>("Created")
                        .HasColumnType("timestamp without time zone");

                    b.Property<DateTime?>("Deleted")
                        .HasColumnType("timestamp without time zone");

                    b.Property<bool>("IsBurnedDown")
                        .HasColumnType("boolean");

                    b.Property<bool>("IsMethLabratory")
                        .HasColumnType("boolean");

                    b.Property<string>("Name")
                        .HasColumnType("text");

                    b.Property<int>("Size")
                        .HasColumnType("integer");

                    b.Property<bool>("ToRent")
                        .HasColumnType("boolean");

                    b.Property<DateTime>("Updated")
                        .HasColumnType("timestamp without time zone");

                    b.HasKey("Id");

                    b.ToTable("Houses");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.HugePocoEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<double>("Float1")
                        .HasColumnType("double precision");

                    b.Property<double>("Float2")
                        .HasColumnType("double precision");

                    b.Property<double>("Float3")
                        .HasColumnType("double precision");

                    b.Property<double>("Float4")
                        .HasColumnType("double precision");

                    b.Property<double>("Float5")
                        .HasColumnType("double precision");

                    b.Property<string>("Name1")
                        .HasColumnType("text");

                    b.Property<string>("Name2")
                        .HasColumnType("text");

                    b.Property<string>("Name3")
                        .HasColumnType("text");

                    b.Property<string>("Name4")
                        .HasColumnType("text");

                    b.Property<string>("Name5")
                        .HasColumnType("text");

                    b.Property<int>("Number1")
                        .HasColumnType("integer");

                    b.Property<int>("Number2")
                        .HasColumnType("integer");

                    b.Property<int>("Number3")
                        .HasColumnType("integer");

                    b.Property<int>("Number4")
                        .HasColumnType("integer");

                    b.Property<int>("Number5")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.ToTable("HugePocos");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.JsonEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<string>("JsonData")
                        .HasColumnType("text");

                    b.HasKey("Id");

                    b.ToTable("Jsons");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.WheelEntity", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasAnnotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

                    b.Property<long?>("CarId")
                        .HasColumnType("bigint");

                    b.Property<int>("WheelType")
                        .HasColumnType("integer");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.ToTable("Wheels");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.SportCarEntity", b =>
                {
                    b.HasBaseType("Moryx.TestTools.Test.Model.CarEntity");

                    b.Property<int>("Performance")
                        .HasColumnType("integer");

                    b.HasDiscriminator().HasValue("SportCarEntity");
                });

            modelBuilder.Entity("Moryx.TestTools.Test.Model.WheelEntity", b =>
                {
                    b.HasOne("Moryx.TestTools.Test.Model.CarEntity", "Car")
                        .WithMany("Wheels")
                        .HasForeignKey("CarId");
                });
#pragma warning restore 612, 618
        }
    }
}
