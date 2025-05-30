﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using pos_system.Data;

#nullable disable

namespace pos_system.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20250528074247_InitialIdentity")]
    partial class InitialIdentity
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex")
                        .HasFilter("[NormalizedName] IS NOT NULL");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("RoleId")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Name")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Value")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("nvarchar(450)");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("int");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("bit");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("bit");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex")
                        .HasFilter("[NormalizedUserName] IS NOT NULL");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblOrder", b =>
                {
                    b.Property<string>("OrderId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("OrderID");

                    b.Property<DateTime>("OrderDate")
                        .HasColumnType("datetime");

                    b.Property<string>("OrderNumber")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<decimal>("TotalPrice")
                        .HasColumnType("decimal(24, 6)");

                    b.HasKey("OrderId")
                        .HasName("PK__tblOrder__C3905BAF81A836DE");

                    b.ToTable("tblOrder", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblOrderItem", b =>
                {
                    b.Property<string>("OrderItemId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("OrderItemID");

                    b.Property<string>("OrderId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("OrderID");

                    b.Property<string>("ProductId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ProductID");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("decimal(24, 6)");

                    b.Property<decimal>("UnitPrice")
                        .HasColumnType("decimal(24, 6)");

                    b.Property<string>("VariantId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("VariantID");

                    b.HasKey("OrderItemId")
                        .HasName("PK__tblOrder__57ED06A1BDB0209E");

                    b.HasIndex("OrderId");

                    b.ToTable("tblOrderItem", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblOrderItemAddon", b =>
                {
                    b.Property<string>("ItemAddonId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ItemAddonID");

                    b.Property<string>("AddonId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("AddonID");

                    b.Property<decimal>("AddonPrice")
                        .HasColumnType("decimal(24, 6)");

                    b.Property<string>("OrderItemId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("OrderItemID");

                    b.Property<int>("Quantity")
                        .HasColumnType("int");

                    b.Property<decimal>("SubTotal")
                        .HasColumnType("decimal(24, 6)");

                    b.HasKey("ItemAddonId")
                        .HasName("PK__tblOrder__41D7485D2833476D");

                    b.HasIndex("OrderItemId");

                    b.ToTable("tblOrderItemAddon", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblOrderNumberTracker", b =>
                {
                    b.Property<DateTime>("DateID")
                        .HasColumnType("datetime2")
                        .HasColumnName("DateID");

                    b.Property<int>("LastNumber")
                        .HasColumnType("int");

                    b.HasKey("DateID");

                    b.ToTable("tblOrderNumberTracker", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblProduct", b =>
                {
                    b.Property<string>("ProductId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ProductID");

                    b.Property<string>("CategoryId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("CategoryID");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<string>("ImageType")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLimitedStock")
                        .HasColumnType("bit");

                    b.Property<double?>("Price")
                        .HasColumnType("float");

                    b.Property<string>("ProductCode")
                        .HasMaxLength(4)
                        .IsUnicode(false)
                        .HasColumnType("char(4)")
                        .IsFixedLength();

                    b.Property<string>("ProductDescription")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("ProductImage")
                        .IsUnicode(false)
                        .HasColumnType("varchar(max)");

                    b.Property<string>("ProductName")
                        .IsRequired()
                        .HasMaxLength(100)
                        .IsUnicode(false)
                        .HasColumnType("varchar(100)");

                    b.Property<double?>("ProductStock")
                        .HasColumnType("float");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("ProductId")
                        .HasName("PK__tblProdu__B40CC6EDB8236FAB");

                    b.HasIndex("CategoryId");

                    b.ToTable("tblProduct", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblProductAddon", b =>
                {
                    b.Property<string>("AddonId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("AddonID");

                    b.Property<string>("AddonName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.Property<double>("AddonPrice")
                        .HasColumnType("float");

                    b.Property<int>("AddonStock")
                        .HasColumnType("int");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLimitedStock")
                        .HasColumnType("bit");

                    b.Property<string>("ProductId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ProductID");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.HasKey("AddonId")
                        .HasName("PK__tblProdu__74289513EF806551");

                    b.HasIndex("ProductId");

                    b.ToTable("tblProductAddon", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblProductCategory", b =>
                {
                    b.Property<string>("CategoryId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("CategoryID");

                    b.Property<string>("CategoryDescription")
                        .HasMaxLength(255)
                        .IsUnicode(false)
                        .HasColumnType("varchar(255)");

                    b.Property<string>("CategoryName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("CategoryId")
                        .HasName("PK__tblProdu__19093A2BF9827246");

                    b.ToTable("tblProductCategory", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblProductVariant", b =>
                {
                    b.Property<string>("VariantId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("VariantID");

                    b.Property<DateTime>("CreatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<bool>("IsAvailable")
                        .HasColumnType("bit");

                    b.Property<bool>("IsLimitedStock")
                        .HasColumnType("bit");

                    b.Property<string>("ProductId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ProductID");

                    b.Property<string>("Sku")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)")
                        .HasColumnName("SKU");

                    b.Property<DateTime>("UpdatedAt")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime")
                        .HasDefaultValueSql("(getdate())");

                    b.Property<double?>("VariantPrice")
                        .HasColumnType("float");

                    b.Property<int?>("VariantStock")
                        .HasColumnType("int");

                    b.HasKey("VariantId")
                        .HasName("PK__tblProdu__0EA233E4A7B8783B");

                    b.HasIndex("ProductId");

                    b.ToTable("tblProductVariant", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblVariantGroup", b =>
                {
                    b.Property<string>("GroupId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("GroupID");

                    b.Property<string>("ProductId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("ProductID");

                    b.Property<string>("VariantName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("GroupId")
                        .HasName("PK__tblVaria__149AF30A45778920");

                    b.HasIndex("ProductId");

                    b.ToTable("tblVariantGroup", (string)null);
                });

            modelBuilder.Entity("pos_system.Models.TblVariantOption", b =>
                {
                    b.Property<string>("OptionId")
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("OptionID");

                    b.Property<string>("GroupId")
                        .IsRequired()
                        .HasMaxLength(36)
                        .IsUnicode(false)
                        .HasColumnType("varchar(36)")
                        .HasColumnName("GroupID");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(50)
                        .IsUnicode(false)
                        .HasColumnType("varchar(50)");

                    b.HasKey("OptionId")
                        .HasName("PK__tblVaria__92C7A1DFD8B9C901");

                    b.HasIndex("GroupId");

                    b.ToTable("tblVariantOption", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("pos_system.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("pos_system.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("pos_system.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("pos_system.Models.ApplicationUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });

            modelBuilder.Entity("pos_system.Models.TblOrderItem", b =>
                {
                    b.HasOne("pos_system.Models.TblOrder", "Order")
                        .WithMany("TblOrderItems")
                        .HasForeignKey("OrderId")
                        .IsRequired()
                        .HasConstraintName("FK_OrderItem_Order");

                    b.Navigation("Order");
                });

            modelBuilder.Entity("pos_system.Models.TblOrderItemAddon", b =>
                {
                    b.HasOne("pos_system.Models.TblOrderItem", "OrderItem")
                        .WithMany("TblOrderItemAddons")
                        .HasForeignKey("OrderItemId")
                        .IsRequired()
                        .HasConstraintName("FK_ItemAddon_OrderItem");

                    b.Navigation("OrderItem");
                });

            modelBuilder.Entity("pos_system.Models.TblProduct", b =>
                {
                    b.HasOne("pos_system.Models.TblProductCategory", "Category")
                        .WithMany("TblProducts")
                        .HasForeignKey("CategoryId")
                        .IsRequired()
                        .HasConstraintName("FK__tblProduc__Categ__73BA3083");

                    b.Navigation("Category");
                });

            modelBuilder.Entity("pos_system.Models.TblProductAddon", b =>
                {
                    b.HasOne("pos_system.Models.TblProduct", "Product")
                        .WithMany("TblProductAddons")
                        .HasForeignKey("ProductId");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("pos_system.Models.TblProductVariant", b =>
                {
                    b.HasOne("pos_system.Models.TblProduct", "Product")
                        .WithMany("TblProductVariants")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__tblProduc__Produ__7E37BEF6");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("pos_system.Models.TblVariantGroup", b =>
                {
                    b.HasOne("pos_system.Models.TblProduct", "Product")
                        .WithMany("TblVariantGroups")
                        .HasForeignKey("ProductId")
                        .HasConstraintName("FK__tblVarian__Produ__76969D2E");

                    b.Navigation("Product");
                });

            modelBuilder.Entity("pos_system.Models.TblVariantOption", b =>
                {
                    b.HasOne("pos_system.Models.TblVariantGroup", "Group")
                        .WithMany("TblVariantOptions")
                        .HasForeignKey("GroupId")
                        .IsRequired()
                        .HasConstraintName("FK__tblVarian__Group__797309D9");

                    b.Navigation("Group");
                });

            modelBuilder.Entity("pos_system.Models.TblOrder", b =>
                {
                    b.Navigation("TblOrderItems");
                });

            modelBuilder.Entity("pos_system.Models.TblOrderItem", b =>
                {
                    b.Navigation("TblOrderItemAddons");
                });

            modelBuilder.Entity("pos_system.Models.TblProduct", b =>
                {
                    b.Navigation("TblProductAddons");

                    b.Navigation("TblProductVariants");

                    b.Navigation("TblVariantGroups");
                });

            modelBuilder.Entity("pos_system.Models.TblProductCategory", b =>
                {
                    b.Navigation("TblProducts");
                });

            modelBuilder.Entity("pos_system.Models.TblVariantGroup", b =>
                {
                    b.Navigation("TblVariantOptions");
                });
#pragma warning restore 612, 618
        }
    }
}
