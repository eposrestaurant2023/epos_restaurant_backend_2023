﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eAPIClient;

namespace eAPIClient.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20210205104332_xxxxd")]
    partial class xxxxd
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .UseIdentityColumns()
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.0");

            modelBuilder.Entity("eAPIClient.Models.ConfigDataModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<Guid>("business_branch_id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("data")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("note")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.HasKey("id");

                    b.ToTable("tbl_config_data");
                });

            modelBuilder.Entity("eAPIClient.Models.DocumentNumberModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<int>("counter")
                        .HasColumnType("int");

                    b.Property<string>("counter_digit")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("document_name")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("format")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int>("outlet_id")
                        .HasColumnType("int");

                    b.Property<string>("prefix")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.HasKey("id");

                    b.ToTable("tbl_document_number");
                });

            modelBuilder.Entity("eAPIClient.Models.MenuModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("background_color")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("menu_name_en")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("menu_name_kh")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int?>("parent_id")
                        .HasColumnType("int");

                    b.Property<string>("text_color")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.HasKey("id");

                    b.ToTable("tbl_menu");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductMenuModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<int>("menu_id")
                        .HasColumnType("int");

                    b.Property<int>("product_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("menu_id");

                    b.HasIndex("product_id");

                    b.ToTable("tbl_product_menu");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<bool>("is_allow_discount")
                        .HasColumnType("bit");

                    b.Property<bool>("is_allow_free")
                        .HasColumnType("bit");

                    b.Property<bool>("is_inventory_product")
                        .HasColumnType("bit");

                    b.Property<string>("photo")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("product_code")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("product_name_en")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("product_name_kh")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.HasKey("id");

                    b.ToTable("tbl_product");
                });

            modelBuilder.Entity("eAPIClient.Models.UserModel", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .UseIdentityColumn();

                    b.Property<string>("full_name")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("password")
                        .HasMaxLength(20)
                        .HasColumnType("nvarchar(20)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int>("pin_code")
                        .HasColumnType("int");

                    b.Property<string>("username")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.HasKey("id");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductMenuModel", b =>
                {
                    b.HasOne("eAPIClient.Models.MenuModel", "menu")
                        .WithMany("product_menus")
                        .HasForeignKey("menu_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("eAPIClient.Models.ProductModel", "product")
                        .WithMany()
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("menu");

                    b.Navigation("product");
                });

            modelBuilder.Entity("eAPIClient.Models.MenuModel", b =>
                {
                    b.Navigation("product_menus");
                });
#pragma warning restore 612, 618
        }
    }
}