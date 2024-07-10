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
                    { 1, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5840), "WholeBean_Product_1", 81.6067333484314m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5893) },
                    { 2, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5904), "WholeBean_Product_2", 45.1314224486633m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5907) },
                    { 3, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5912), "WholeBean_Product_3", 10.6145763725502m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5915) },
                    { 4, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5921), "WholeBean_Product_4", 98.2392922583436m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5923) },
                    { 5, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5929), "WholeBean_Product_5", 88.2227239652338m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5931) },
                    { 6, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5938), "WholeBean_Product_6", 49.4043652849489m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5941) },
                    { 7, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5946), "WholeBean_Product_7", 41.9169384436571m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5949) },
                    { 8, 5, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5955), "WholeBean_Product_8", 61.107232387303m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5957) },
                    { 9, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5963), "Ground_Product_9", 58.8177451961799m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5965) },
                    { 10, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5974), "Ground_Product_10", 81.5861204760637m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5977) },
                    { 11, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5983), "Ground_Product_11", 64.6856602191068m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5985) },
                    { 12, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5990), "Ground_Product_12", 85.4344787472334m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5993) },
                    { 13, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(5998), "Ground_Product_13", 60.817610446475m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6001) },
                    { 14, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6006), "Ground_Product_14", 22.6052073293656m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6009) },
                    { 15, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6014), "Ground_Product_15", 78.0221387851453m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6017) },
                    { 16, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6022), "Ground_Product_16", 87.6589588728787m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6024) },
                    { 17, 6, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6030), "Ground_Product_17", 59.6695983795678m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6032) },
                    { 18, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6039), "Instant_Product_18", 18.7204377179728m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6042) },
                    { 19, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6047), "Instant_Product_19", 32.5601733546598m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6050) },
                    { 20, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6055), "Instant_Product_20", 56.5176904794962m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6058) },
                    { 21, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6063), "Instant_Product_21", 10.2473023266033m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6065) },
                    { 22, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6071), "Instant_Product_22", 70.5546851472502m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6073) },
                    { 23, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6078), "Instant_Product_23", 86.1797021032742m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6081) },
                    { 24, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6086), "Instant_Product_24", 12.0237213308709m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6089) },
                    { 25, 7, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6094), "Instant_Product_25", 52.2621937209356m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6097) },
                    { 26, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6102), "Pods_Product_26", 81.3474761981931m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6104) },
                    { 27, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6110), "Pods_Product_27", 92.1703729189075m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6112) },
                    { 28, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6117), "Pods_Product_28", 73.9866027309082m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6119) },
                    { 29, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6125), "Pods_Product_29", 96.2389276842317m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6127) },
                    { 30, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6133), "Pods_Product_30", 30.157653900386m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6135) },
                    { 31, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6140), "Pods_Product_31", 32.1417895615493m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6143) },
                    { 32, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6148), "Pods_Product_32", 39.0398640281992m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6151) },
                    { 33, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6156), "Pods_Product_33", 25.1183859464384m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6158) },
                    { 34, 8, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6165), "Pods_Product_34", 61.9970556973045m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6168) },
                    { 35, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6173), "Specialty_Product_35", 1.73436805430758m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6176) },
                    { 36, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6182), "Specialty_Product_36", 68.7269975617305m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6184) },
                    { 37, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6190), "Specialty_Product_37", 77.5090844613122m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6192) },
                    { 38, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6198), "Specialty_Product_38", 16.1257409961252m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6200) },
                    { 39, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6205), "Specialty_Product_39", 37.0374057055212m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6208) },
                    { 40, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6213), "Specialty_Product_40", 32.2092272069999m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6215) },
                    { 41, 9, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6221), "Specialty_Product_41", 20.2905761369988m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6223) },
                    { 42, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6229), "CoffeeMakers_Product_42", 10.4281346441457m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6231) },
                    { 43, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6237), "CoffeeMakers_Product_43", 70.7425247197932m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6239) },
                    { 44, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6245), "CoffeeMakers_Product_44", 73.5315587427293m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6247) },
                    { 45, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6252), "CoffeeMakers_Product_45", 88.0380119388771m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6255) },
                    { 46, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6260), "CoffeeMakers_Product_46", 56.2478960826131m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6262) },
                    { 47, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6268), "CoffeeMakers_Product_47", 86.734093930808m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6270) },
                    { 48, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6276), "CoffeeMakers_Product_48", 24.3143281385188m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6278) },
                    { 49, 10, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6320), "CoffeeMakers_Product_49", 87.6152766834918m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6322) },
                    { 50, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6328), "Grinders_Product_50", 13.073577632694m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6331) },
                    { 51, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6336), "Grinders_Product_51", 78.7955038175101m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6339) },
                    { 52, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6344), "Grinders_Product_52", 17.026252989094m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6347) },
                    { 53, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6352), "Grinders_Product_53", 51.2892285862515m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6354) },
                    { 54, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6360), "Grinders_Product_54", 45.5301787791826m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6362) },
                    { 55, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6367), "Grinders_Product_55", 28.5035681572001m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6370) },
                    { 56, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6375), "Grinders_Product_56", 74.0515734355406m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6378) },
                    { 57, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6383), "Grinders_Product_57", 38.600952790862m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6386) },
                    { 58, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6391), "Grinders_Product_58", 8.37704035626912m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6394) },
                    { 59, 11, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6399), "Grinders_Product_59", 30.279133200143m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6402) },
                    { 60, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6407), "Mugs_Product_60", 17.539989478398m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6410) },
                    { 61, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6415), "Mugs_Product_61", 16.9185074085059m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6418) },
                    { 62, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6423), "Mugs_Product_62", 15.372347621284m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6425) },
                    { 63, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6431), "Mugs_Product_63", 87.8197292730764m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6433) },
                    { 64, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6438), "Mugs_Product_64", 85.4631128012597m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6441) },
                    { 65, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6446), "Mugs_Product_65", 11.585713236114m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6449) },
                    { 66, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6456), "Mugs_Product_66", 61.8582475644492m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6459) },
                    { 67, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6464), "Mugs_Product_67", 59.6651809410901m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6466) },
                    { 68, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6472), "Mugs_Product_68", 58.6693202716045m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6474) },
                    { 69, 12, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6480), "Mugs_Product_69", 41.6498365560763m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6482) },
                    { 70, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6488), "Filters_Product_70", 96.3351858751315m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6490) },
                    { 71, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6495), "Filters_Product_71", 39.9011259876373m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6498) },
                    { 72, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6503), "Filters_Product_72", 88.4498689875424m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6506) },
                    { 73, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6511), "Filters_Product_73", 5.48606881162508m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6514) },
                    { 74, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6519), "Filters_Product_74", 19.2441254685083m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6521) },
                    { 75, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6527), "Filters_Product_75", 10.8593087967208m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6529) },
                    { 76, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6535), "Filters_Product_76", 71.069521940502m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6537) },
                    { 77, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6543), "Filters_Product_77", 4.45473693133672m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6545) },
                    { 78, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6550), "Filters_Product_78", 93.6444864677507m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6553) },
                    { 79, 13, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6558), "Filters_Product_79", 42.4257307997533m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6561) },
                    { 80, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6566), "Pastries_Product_80", 7.08284968456827m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6569) },
                    { 81, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6574), "Pastries_Product_81", 88.6536406788253m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6576) },
                    { 82, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6582), "Pastries_Product_82", 30.4256851962955m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6585) },
                    { 83, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6590), "Pastries_Product_83", 14.4061722285589m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6592) },
                    { 84, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6598), "Pastries_Product_84", 23.0464309029277m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6600) },
                    { 85, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6606), "Pastries_Product_85", 75.6546613901226m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6608) },
                    { 86, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6613), "Pastries_Product_86", 79.7888194200414m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6616) },
                    { 87, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6621), "Pastries_Product_87", 42.2257699562835m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6624) },
                    { 88, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6629), "Pastries_Product_88", 47.0571372660194m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6631) },
                    { 89, 14, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6636), "Pastries_Product_89", 86.5710737421892m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6639) },
                    { 90, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6643), "Snacks_Product_90", 14.5590265036287m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6646) },
                    { 91, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6650), "Snacks_Product_91", 32.0677201108052m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6652) },
                    { 92, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6657), "Snacks_Product_92", 39.7958009090183m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6659) },
                    { 93, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6664), "Snacks_Product_93", 40.5697078155407m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6666) },
                    { 94, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6670), "Snacks_Product_94", 14.9476745805661m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6672) },
                    { 95, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6677), "Snacks_Product_95", 60.1485354276361m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6680) },
                    { 96, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6685), "Snacks_Product_96", 83.2901932772973m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6687) },
                    { 97, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6722), "Snacks_Product_97", 47.2676652418351m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6724) },
                    { 98, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6730), "Snacks_Product_98", 96.1850965794082m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6732) },
                    { 99, 15, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6738), "Snacks_Product_99", 7.6550571779209m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6741) },
                    { 100, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6746), "Syrups_Product_100", 53.1416840551104m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6748) },
                    { 101, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6753), "Syrups_Product_101", 72.6596884651607m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6756) },
                    { 102, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6760), "Syrups_Product_102", 53.3726379636248m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6763) },
                    { 103, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6768), "Syrups_Product_103", 83.0273076876185m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6770) },
                    { 104, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6775), "Syrups_Product_104", 81.739835173706m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6777) },
                    { 105, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6782), "Syrups_Product_105", 2.81876414883974m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6784) },
                    { 106, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6789), "Syrups_Product_106", 13.5958386562073m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6792) },
                    { 107, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6797), "Syrups_Product_107", 46.2636243991428m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6799) },
                    { 108, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6804), "Syrups_Product_108", 27.1332038693933m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6807) },
                    { 109, 16, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6812), "Syrups_Product_109", 60.8475504500594m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6814) },
                    { 110, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6820), "MilkAlternatives_Product_110", 81.2768480193317m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6822) },
                    { 111, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6828), "MilkAlternatives_Product_111", 15.1093387935069m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6830) },
                    { 112, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6835), "MilkAlternatives_Product_112", 73.6882202009531m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6837) },
                    { 113, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6842), "MilkAlternatives_Product_113", 30.7499155305255m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6844) },
                    { 114, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6849), "MilkAlternatives_Product_114", 31.1161288748902m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6851) },
                    { 115, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6856), "MilkAlternatives_Product_115", 93.3740649764787m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6858) },
                    { 116, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6863), "MilkAlternatives_Product_116", 91.3777150301613m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6866) },
                    { 117, 17, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6871), "MilkAlternatives_Product_117", 36.5930401815748m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6873) },
                    { 118, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6878), "Merchandise_Product_118", 86.0904729002193m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6880) },
                    { 119, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6886), "Merchandise_Product_119", 10.5930336292517m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6888) },
                    { 120, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6895), "Merchandise_Product_120", 38.6390445223981m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6897) },
                    { 121, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6902), "Merchandise_Product_121", 91.7443889761065m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6904) },
                    { 122, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6909), "Merchandise_Product_122", 17.8338283628759m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6911) },
                    { 123, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6916), "Merchandise_Product_123", 99.3317844047726m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6918) },
                    { 124, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6923), "Merchandise_Product_124", 63.0370180072504m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6926) },
                    { 125, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6931), "Merchandise_Product_125", 80.2406407842932m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6933) },
                    { 126, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6938), "Merchandise_Product_126", 7.6433179483614m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6941) },
                    { 127, 18, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6946), "Merchandise_Product_127", 62.2676823343731m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6948) },
                    { 128, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6953), "Subscriptions_Product_128", 0.897609781596431m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6955) },
                    { 129, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6960), "Subscriptions_Product_129", 55.4653987240797m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6962) },
                    { 130, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6969), "Subscriptions_Product_130", 96.5956200887607m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6971) },
                    { 131, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6977), "Subscriptions_Product_131", 57.289829259453m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6980) },
                    { 132, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6985), "Subscriptions_Product_132", 93.7539763582518m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6987) },
                    { 133, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6992), "Subscriptions_Product_133", 83.4179562764324m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6994) },
                    { 134, 19, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(6999), "Subscriptions_Product_134", 63.6949976693159m, new DateTime(2024, 7, 10, 19, 14, 59, 388, DateTimeKind.Local).AddTicks(7001) }
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
