using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeShop.Products.Api.Repository.Migrations
{
    /// <inheritdoc />
    public partial class CreateInitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParentCategoryId = table.Column<int>(type: "int", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Categories_Categories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CategoryId = table.Column<int>(type: "int", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(8,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Products_Categories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "Categories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "CreatedAt", "Name", "ParentCategoryId", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Coffee", null, null },
                    { 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Accessories", null, null },
                    { 3, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "FoodItems", null, null },
                    { 4, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Others", null, null },
                    { 5, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "WholeBean", 1, null },
                    { 6, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Ground", 1, null },
                    { 7, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Instant", 1, null },
                    { 8, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pods", 1, null },
                    { 9, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Specialty", 1, null },
                    { 10, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "CoffeeMakers", 2, null },
                    { 11, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Grinders", 2, null },
                    { 12, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Mugs", 2, null },
                    { 13, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Filters", 2, null },
                    { 14, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Pastries", 3, null },
                    { 15, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Snacks", 3, null },
                    { 16, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Syrups", 3, null },
                    { 17, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "MilkAlternatives", 3, null },
                    { 18, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Merchandise", 4, null },
                    { 19, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Subscriptions", 4, null }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "CreatedAt", "Name", "Price", "UpdatedAt" },
                values: new object[,]
                {
                    { 1, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6525), "WholeBean_Product_1", 85.6075758591491m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6592) },
                    { 2, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6609), "WholeBean_Product_2", 28.2907567223371m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6613) },
                    { 3, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6622), "WholeBean_Product_3", 20.9744044792183m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6626) },
                    { 4, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6634), "WholeBean_Product_4", 63.3736996522108m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6638) },
                    { 5, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6646), "WholeBean_Product_5", 13.4279297544984m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6650) },
                    { 6, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6661), "WholeBean_Product_6", 78.5552142109849m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6665) },
                    { 7, 5, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6673), "WholeBean_Product_7", 59.2770685572971m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6677) },
                    { 8, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6687), "Ground_Product_8", 1.5609820809328m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6690) },
                    { 9, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6699), "Ground_Product_9", 46.199528812862m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6703) },
                    { 10, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6716), "Ground_Product_10", 15.1380326616443m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6719) },
                    { 11, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6728), "Ground_Product_11", 37.2237921965464m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6732) },
                    { 12, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6740), "Ground_Product_12", 97.1920774243794m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6744) },
                    { 13, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6752), "Ground_Product_13", 1.35792072450455m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6756) },
                    { 14, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6764), "Ground_Product_14", 20.2653402194981m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6767) },
                    { 15, 6, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6775), "Ground_Product_15", 58.1959379706093m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6779) },
                    { 16, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6787), "Instant_Product_16", 86.038783843338m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6791) },
                    { 17, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6799), "Instant_Product_17", 1.726234337222m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6803) },
                    { 18, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6814), "Instant_Product_18", 90.5054342786104m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6818) },
                    { 19, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6827), "Instant_Product_19", 83.3277169583034m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6831) },
                    { 20, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6839), "Instant_Product_20", 65.6900096248406m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6843) },
                    { 21, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6851), "Instant_Product_21", 15.2951137309292m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6855) },
                    { 22, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6863), "Instant_Product_22", 11.6781454224071m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6866) },
                    { 23, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6874), "Instant_Product_23", 15.7468984871992m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6878) },
                    { 24, 7, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6886), "Instant_Product_24", 89.2213592498892m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6890) },
                    { 25, 8, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6898), "Pods_Product_25", 19.4631051963757m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6902) },
                    { 26, 8, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6909), "Pods_Product_26", 3.36373337338941m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6913) },
                    { 27, 8, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6921), "Pods_Product_27", 56.0011706751356m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6925) },
                    { 28, 8, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6932), "Pods_Product_28", 26.1559361410841m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6936) },
                    { 29, 8, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6944), "Pods_Product_29", 57.6485058521143m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6947) },
                    { 30, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6956), "Specialty_Product_30", 56.6043884265326m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6960) },
                    { 31, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6968), "Specialty_Product_31", 79.8832222328689m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6972) },
                    { 32, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6980), "Specialty_Product_32", 31.4333145486446m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6984) },
                    { 33, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6992), "Specialty_Product_33", 11.0694536702073m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(6996) },
                    { 34, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7006), "Specialty_Product_34", 14.7326430288659m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7010) },
                    { 35, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7018), "Specialty_Product_35", 11.490028357617m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7022) },
                    { 36, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7030), "Specialty_Product_36", 62.0371333397594m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7034) },
                    { 37, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7091), "Specialty_Product_37", 46.1404016156078m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7095) },
                    { 38, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7104), "Specialty_Product_38", 22.5691127883755m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7108) },
                    { 39, 9, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7115), "Specialty_Product_39", 35.0280179780895m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7119) },
                    { 40, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7128), "CoffeeMakers_Product_40", 85.6970536770584m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7131) },
                    { 41, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7140), "CoffeeMakers_Product_41", 81.092970133816m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7143) },
                    { 42, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7151), "CoffeeMakers_Product_42", 68.5567015421524m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7155) },
                    { 43, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7163), "CoffeeMakers_Product_43", 89.7884228994036m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7167) },
                    { 44, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7175), "CoffeeMakers_Product_44", 59.7676686838956m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7179) },
                    { 45, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7187), "CoffeeMakers_Product_45", 94.6247976547816m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7190) },
                    { 46, 10, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7199), "CoffeeMakers_Product_46", 79.3922591720656m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7202) },
                    { 47, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7211), "Grinders_Product_47", 11.125933462475m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7214) },
                    { 48, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7223), "Grinders_Product_48", 93.1611463802165m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7226) },
                    { 49, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7235), "Grinders_Product_49", 32.6823529970965m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7238) },
                    { 50, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7246), "Grinders_Product_50", 31.6090340944141m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7250) },
                    { 51, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7258), "Grinders_Product_51", 43.9368961326137m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7261) },
                    { 52, 11, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7269), "Grinders_Product_52", 33.1948625417296m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7273) },
                    { 53, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7281), "Mugs_Product_53", 27.3090894594954m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7285) },
                    { 54, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7293), "Mugs_Product_54", 94.2113881320084m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7297) },
                    { 55, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7305), "Mugs_Product_55", 64.8626023032981m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7309) },
                    { 56, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7317), "Mugs_Product_56", 93.0890766424439m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7320) },
                    { 57, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7328), "Mugs_Product_57", 50.1925073325685m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7332) },
                    { 58, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7340), "Mugs_Product_58", 31.8941411345948m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7344) },
                    { 59, 12, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7351), "Mugs_Product_59", 23.7518659771261m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7355) },
                    { 60, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7364), "Filters_Product_60", 4.79475469428498m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7367) },
                    { 61, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7375), "Filters_Product_61", 84.8611387171619m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7379) },
                    { 62, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7387), "Filters_Product_62", 60.6382650272542m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7390) },
                    { 63, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7399), "Filters_Product_63", 67.4073157648788m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7402) },
                    { 64, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7410), "Filters_Product_64", 57.8442382254183m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7414) },
                    { 65, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7421), "Filters_Product_65", 13.5862667955025m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7425) },
                    { 66, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7436), "Filters_Product_66", 99.7117859173012m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7440) },
                    { 67, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7448), "Filters_Product_67", 26.0184142653336m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7451) },
                    { 68, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7460), "Filters_Product_68", 3.6832538571831m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7464) },
                    { 69, 13, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7472), "Filters_Product_69", 96.9116747914363m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7476) },
                    { 70, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7484), "Pastries_Product_70", 45.3893953151105m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7488) },
                    { 71, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7496), "Pastries_Product_71", 9.38858622736738m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7500) },
                    { 72, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7507), "Pastries_Product_72", 32.9258549957175m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7511) },
                    { 73, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7519), "Pastries_Product_73", 19.4866780654345m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7522) },
                    { 74, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7530), "Pastries_Product_74", 1.70476620685823m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7533) },
                    { 75, 14, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7541), "Pastries_Product_75", 40.2650638766267m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7544) },
                    { 76, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7553), "Snacks_Product_76", 73.4230387335389m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7556) },
                    { 77, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7564), "Snacks_Product_77", 40.9589861230304m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7568) },
                    { 78, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7576), "Snacks_Product_78", 34.2816199122908m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7579) },
                    { 79, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7587), "Snacks_Product_79", 88.7010792459029m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7591) },
                    { 80, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7599), "Snacks_Product_80", 26.4281691808729m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7602) },
                    { 81, 15, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7610), "Snacks_Product_81", 88.971088520811m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7614) },
                    { 82, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7623), "Syrups_Product_82", 45.968058202647m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7626) },
                    { 83, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7634), "Syrups_Product_83", 42.8769318191842m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7638) },
                    { 84, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7690), "Syrups_Product_84", 7.77713452014083m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7694) },
                    { 85, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7703), "Syrups_Product_85", 0.0132762090122474m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7707) },
                    { 86, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7714), "Syrups_Product_86", 49.0439865094317m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7718) },
                    { 87, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7726), "Syrups_Product_87", 1.49661703104601m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7729) },
                    { 88, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7737), "Syrups_Product_88", 89.5492494566897m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7741) },
                    { 89, 16, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7749), "Syrups_Product_89", 77.7295231461012m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7753) },
                    { 90, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7761), "MilkAlternatives_Product_90", 60.6160353986163m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7764) },
                    { 91, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7773), "MilkAlternatives_Product_91", 88.8079728076507m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7776) },
                    { 92, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7784), "MilkAlternatives_Product_92", 8.75318977763622m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7788) },
                    { 93, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7796), "MilkAlternatives_Product_93", 64.8705342105622m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7800) },
                    { 94, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7808), "MilkAlternatives_Product_94", 3.09442380119083m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7811) },
                    { 95, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7819), "MilkAlternatives_Product_95", 82.6454214163608m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7823) },
                    { 96, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7831), "MilkAlternatives_Product_96", 75.2482343496904m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7834) },
                    { 97, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7842), "MilkAlternatives_Product_97", 4.00503533595246m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7846) },
                    { 98, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7854), "MilkAlternatives_Product_98", 1.2209191250697m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7858) },
                    { 99, 17, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7866), "MilkAlternatives_Product_99", 82.550260628718m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7869) },
                    { 100, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7878), "Merchandise_Product_100", 78.7215372265757m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7882) },
                    { 101, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7890), "Merchandise_Product_101", 89.9504724509384m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7893) },
                    { 102, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7901), "Merchandise_Product_102", 98.8268198353156m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7905) },
                    { 103, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7913), "Merchandise_Product_103", 37.254289283409m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7917) },
                    { 104, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7925), "Merchandise_Product_104", 39.8099004261286m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7928) },
                    { 105, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7936), "Merchandise_Product_105", 60.5305385040899m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7940) },
                    { 106, 18, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7948), "Merchandise_Product_106", 94.1961781870686m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7952) },
                    { 107, 19, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7960), "Subscriptions_Product_107", 63.5957284723047m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7964) },
                    { 108, 19, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7972), "Subscriptions_Product_108", 65.906797081904m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7976) },
                    { 109, 19, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7984), "Subscriptions_Product_109", 32.642403838893m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7988) },
                    { 110, 19, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(7997), "Subscriptions_Product_110", 92.9914496791189m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(8000) },
                    { 111, 19, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(8008), "Subscriptions_Product_111", 46.0817995979737m, new DateTime(2024, 7, 11, 14, 21, 33, 247, DateTimeKind.Local).AddTicks(8012) }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentCategoryId",
                table: "Categories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Products_CategoryId",
                table: "Products",
                column: "CategoryId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Categories");
        }
    }
}
