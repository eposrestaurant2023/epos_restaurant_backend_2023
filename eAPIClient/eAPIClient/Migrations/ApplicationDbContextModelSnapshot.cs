﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using eAPIClient;

namespace eAPIClient.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    partial class ApplicationDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("eAPIClient.Models.ProductModifierModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("modifier_name")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<decimal>("price")
                        .HasColumnType("decimal(16,4)");

                    b.Property<int>("product_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("product_id");

                    b.ToTable("tbl_product_modifier");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductPortionModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<decimal>("cost")
                        .HasColumnType("decimal(16,4)");

                    b.Property<decimal>("multiplier")
                        .HasColumnType("decimal(16,4)");

                    b.Property<string>("portion_name")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("prices")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int>("product_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("product_id");

                    b.ToTable("tbl_product_portion");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductPriceModel", b =>
                {
                    b.Property<Guid>("id")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("prices")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int>("product_portion_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.ToTable("tbl_product_price");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductPrinterModel", b =>
                {
                    b.Property<int>("id")
                        .HasColumnType("int");

                    b.Property<string>("ip_address_port")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<string>("printer_name")
                        .HasColumnType("nvarchar(max)")
                        .UseCollation("Khmer_100_BIN");

                    b.Property<int>("product_id")
                        .HasColumnType("int");

                    b.HasKey("id");

                    b.HasIndex("product_id");

                    b.ToTable("tbl_product_printer");
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

            modelBuilder.Entity("eAPIClient.Models.ProductModifierModel", b =>
                {
                    b.HasOne("eAPIClient.Models.ProductModel", "product")
                        .WithMany("product_modifiers")
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductPortionModel", b =>
                {
                    b.HasOne("eAPIClient.Models.ProductModel", "product")
                        .WithMany("product_portions")
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductPrinterModel", b =>
                {
                    b.HasOne("eAPIClient.Models.ProductModel", "product")
                        .WithMany("product_printers")
                        .HasForeignKey("product_id")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("product");
                });

            modelBuilder.Entity("eAPIClient.Models.MenuModel", b =>
                {
                    b.Navigation("product_menus");
                });

            modelBuilder.Entity("eAPIClient.Models.ProductModel", b =>
                {
                    b.Navigation("product_modifiers");

                    b.Navigation("product_portions");

                    b.Navigation("product_printers");
                });
#pragma warning restore 612, 618
        }
    }
}
