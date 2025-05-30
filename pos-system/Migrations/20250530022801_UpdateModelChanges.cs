using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace pos_system.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModelChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblOrder",
                columns: table => new
                {
                    OrderID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    OrderDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    OrderNumber = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(24,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblOrder__C3905BAF81A836DE", x => x.OrderID);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderNumberTracker",
                columns: table => new
                {
                    DateID = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastNumber = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblOrderNumberTracker", x => x.DateID);
                });

            migrationBuilder.CreateTable(
                name: "tblProductCategory",
                columns: table => new
                {
                    CategoryID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    CategoryName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    CategoryDescription = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblProdu__19093A2BF9827246", x => x.CategoryID);
                });

            migrationBuilder.CreateTable(
                name: "tblOrderItem",
                columns: table => new
                {
                    OrderItemID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    OrderID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    ProductID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    VariantID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: true),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    UnitPrice = table.Column<decimal>(type: "decimal(24,6)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(24,6)", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblOrder__57ED06A1BDB0209E", x => x.OrderItemID);
                    table.ForeignKey(
                        name: "FK_OrderItem_Order",
                        column: x => x.OrderID,
                        principalTable: "tblOrder",
                        principalColumn: "OrderID");
                });

            migrationBuilder.CreateTable(
                name: "tblProduct",
                columns: table => new
                {
                    ProductID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    CategoryID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    ProductCode = table.Column<string>(type: "char(4)", unicode: false, fixedLength: true, maxLength: 4, nullable: true),
                    ProductName = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    ProductDescription = table.Column<string>(type: "varchar(255)", unicode: false, maxLength: 255, nullable: false),
                    ProductStock = table.Column<double>(type: "float", nullable: true),
                    IsLimitedStock = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    ProductImage = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true),
                    ImageType = table.Column<string>(type: "varchar(max)", unicode: false, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblProdu__B40CC6EDB8236FAB", x => x.ProductID);
                    table.ForeignKey(
                        name: "FK__tblProduc__Categ__73BA3083",
                        column: x => x.CategoryID,
                        principalTable: "tblProductCategory",
                        principalColumn: "CategoryID");
                });

            migrationBuilder.CreateTable(
                name: "tblOrderItemAddon",
                columns: table => new
                {
                    ItemAddonID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    OrderItemID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    AddonID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    AddonPrice = table.Column<decimal>(type: "decimal(24,6)", nullable: false),
                    SubTotal = table.Column<decimal>(type: "decimal(24,6)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblOrder__41D7485D2833476D", x => x.ItemAddonID);
                    table.ForeignKey(
                        name: "FK_ItemAddon_OrderItem",
                        column: x => x.OrderItemID,
                        principalTable: "tblOrderItem",
                        principalColumn: "OrderItemID");
                });

            migrationBuilder.CreateTable(
                name: "tblProductAddon",
                columns: table => new
                {
                    AddonID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    ProductID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: true),
                    AddonName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    AddonPrice = table.Column<double>(type: "float", nullable: false),
                    AddonStock = table.Column<int>(type: "int", nullable: false),
                    IsLimitedStock = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblProdu__74289513EF806551", x => x.AddonID);
                    table.ForeignKey(
                        name: "FK_tblProductAddon_tblProduct_ProductID",
                        column: x => x.ProductID,
                        principalTable: "tblProduct",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "tblProductVariant",
                columns: table => new
                {
                    VariantID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    ProductID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: true),
                    SKU = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false),
                    VariantPrice = table.Column<double>(type: "float", nullable: true),
                    VariantStock = table.Column<int>(type: "int", nullable: true),
                    IsLimitedStock = table.Column<bool>(type: "bit", nullable: false),
                    IsAvailable = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "(getdate())")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblProdu__0EA233E4A7B8783B", x => x.VariantID);
                    table.ForeignKey(
                        name: "FK__tblProduc__Produ__7E37BEF6",
                        column: x => x.ProductID,
                        principalTable: "tblProduct",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "tblVariantGroup",
                columns: table => new
                {
                    GroupID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    ProductID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: true),
                    VariantName = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblVaria__149AF30A45778920", x => x.GroupID);
                    table.ForeignKey(
                        name: "FK__tblVarian__Produ__76969D2E",
                        column: x => x.ProductID,
                        principalTable: "tblProduct",
                        principalColumn: "ProductID");
                });

            migrationBuilder.CreateTable(
                name: "tblVariantOption",
                columns: table => new
                {
                    OptionID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    GroupID = table.Column<string>(type: "varchar(36)", unicode: false, maxLength: 36, nullable: false),
                    Value = table.Column<string>(type: "varchar(50)", unicode: false, maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__tblVaria__92C7A1DFD8B9C901", x => x.OptionID);
                    table.ForeignKey(
                        name: "FK__tblVarian__Group__797309D9",
                        column: x => x.GroupID,
                        principalTable: "tblVariantGroup",
                        principalColumn: "GroupID");
                });

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItem_OrderID",
                table: "tblOrderItem",
                column: "OrderID");

            migrationBuilder.CreateIndex(
                name: "IX_tblOrderItemAddon_OrderItemID",
                table: "tblOrderItemAddon",
                column: "OrderItemID");

            migrationBuilder.CreateIndex(
                name: "IX_tblProduct_CategoryID",
                table: "tblProduct",
                column: "CategoryID");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductAddon_ProductID",
                table: "tblProductAddon",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_tblProductVariant_ProductID",
                table: "tblProductVariant",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_tblVariantGroup_ProductID",
                table: "tblVariantGroup",
                column: "ProductID");

            migrationBuilder.CreateIndex(
                name: "IX_tblVariantOption_GroupID",
                table: "tblVariantOption",
                column: "GroupID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblOrderItemAddon");

            migrationBuilder.DropTable(
                name: "tblOrderNumberTracker");

            migrationBuilder.DropTable(
                name: "tblProductAddon");

            migrationBuilder.DropTable(
                name: "tblProductVariant");

            migrationBuilder.DropTable(
                name: "tblVariantOption");

            migrationBuilder.DropTable(
                name: "tblOrderItem");

            migrationBuilder.DropTable(
                name: "tblVariantGroup");

            migrationBuilder.DropTable(
                name: "tblOrder");

            migrationBuilder.DropTable(
                name: "tblProduct");

            migrationBuilder.DropTable(
                name: "tblProductCategory");
        }
    }
}
