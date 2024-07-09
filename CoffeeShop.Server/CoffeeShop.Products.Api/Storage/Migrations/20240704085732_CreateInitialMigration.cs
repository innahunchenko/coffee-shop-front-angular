using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoffeeShop.Products.Api.Storage.Migrations
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
                    { 1, 5, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(727), "WholeBean_Product_1", 39.900515695959m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(770) },
                    { 2, 5, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(778), "WholeBean_Product_2", 27.7233179468474m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(780) },
                    { 3, 5, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(783), "WholeBean_Product_3", 21.4492731899077m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(784) },
                    { 4, 5, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(788), "WholeBean_Product_4", 87.5436179397262m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(789) },
                    { 5, 5, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(792), "WholeBean_Product_5", 20.9755794449348m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(794) },
                    { 6, 6, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(799), "Ground_Product_6", 77.2337385251688m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(800) },
                    { 7, 6, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(804), "Ground_Product_7", 99.4697145783609m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(805) },
                    { 8, 6, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(809), "Ground_Product_8", 47.6249653452261m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(811) },
                    { 9, 6, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(814), "Ground_Product_9", 87.8453042904237m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(815) },
                    { 10, 6, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(820), "Ground_Product_10", 15.2604993210595m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(822) },
                    { 11, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(826), "Instant_Product_11", 45.0656410259566m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(827) },
                    { 12, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(887), "Instant_Product_12", 9.10626866750053m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(889) },
                    { 13, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(892), "Instant_Product_13", 51.6553701825023m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(894) },
                    { 14, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(897), "Instant_Product_14", 54.1629988663512m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(899) },
                    { 15, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(902), "Instant_Product_15", 31.3115645095148m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(903) },
                    { 16, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(906), "Instant_Product_16", 25.7590855880902m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(908) },
                    { 17, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(911), "Instant_Product_17", 86.7257898597764m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(913) },
                    { 18, 7, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(920), "Instant_Product_18", 26.7804580804978m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(921) },
                    { 19, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(925), "Pods_Product_19", 28.6867303756965m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(927) },
                    { 20, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(930), "Pods_Product_20", 8.46554550753944m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(932) },
                    { 21, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(935), "Pods_Product_21", 25.0133804200061m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(937) },
                    { 22, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(941), "Pods_Product_22", 43.0978803926869m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(943) },
                    { 23, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(946), "Pods_Product_23", 17.6113770298199m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(948) },
                    { 24, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(951), "Pods_Product_24", 7.23779954710363m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(953) },
                    { 25, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(956), "Pods_Product_25", 38.4893279548927m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(957) },
                    { 26, 8, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(961), "Pods_Product_26", 2.57694696862686m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(962) },
                    { 27, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(966), "Specialty_Product_27", 84.5433456497868m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(967) },
                    { 28, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(971), "Specialty_Product_28", 39.1717788587936m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(973) },
                    { 29, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(976), "Specialty_Product_29", 1.96970810203454m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(978) },
                    { 30, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(981), "Specialty_Product_30", 59.2657138030157m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(982) },
                    { 31, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(986), "Specialty_Product_31", 90.7701928239306m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(987) },
                    { 32, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(990), "Specialty_Product_32", 1.35810423015359m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(992) },
                    { 33, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(995), "Specialty_Product_33", 55.2511913481889m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(996) },
                    { 34, 9, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1000), "Specialty_Product_34", 55.387527498995m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1002) },
                    { 35, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1005), "CoffeeMakers_Product_35", 48.3351137939554m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1007) },
                    { 36, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1010), "CoffeeMakers_Product_36", 83.4532813759349m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1012) },
                    { 37, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1015), "CoffeeMakers_Product_37", 5.12909510038918m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1016) },
                    { 38, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1019), "CoffeeMakers_Product_38", 2.35097590063488m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1021) },
                    { 39, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1024), "CoffeeMakers_Product_39", 97.0290024969362m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1026) },
                    { 40, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1029), "CoffeeMakers_Product_40", 0.946592644843325m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1030) },
                    { 41, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1033), "CoffeeMakers_Product_41", 38.2958624999305m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1035) },
                    { 42, 10, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1038), "CoffeeMakers_Product_42", 9.95560788376686m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1040) },
                    { 43, 11, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1044), "Grinders_Product_43", 90.7701054071467m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1045) },
                    { 44, 11, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1049), "Grinders_Product_44", 83.4931190900829m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1050) },
                    { 45, 11, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1053), "Grinders_Product_45", 73.2967812010567m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1055) },
                    { 46, 11, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1058), "Grinders_Product_46", 20.5173922158827m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1059) },
                    { 47, 11, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1063), "Grinders_Product_47", 97.1484934648714m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1064) },
                    { 48, 12, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1068), "Mugs_Product_48", 9.92279861743112m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1069) },
                    { 49, 12, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1072), "Mugs_Product_49", 46.2172959083072m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1074) },
                    { 50, 12, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1077), "Mugs_Product_50", 13.1942286737632m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1078) },
                    { 51, 12, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1082), "Mugs_Product_51", 95.80965211291m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1084) },
                    { 52, 12, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1088), "Mugs_Product_52", 37.356956350505m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1090) },
                    { 53, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1095), "Filters_Product_53", 20.867670089016m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1097) },
                    { 54, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1101), "Filters_Product_54", 1.76144535880296m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1103) },
                    { 55, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1107), "Filters_Product_55", 74.2241742278283m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1109) },
                    { 56, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1112), "Filters_Product_56", 0.832253239474223m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1114) },
                    { 57, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1118), "Filters_Product_57", 86.4766102048189m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1120) },
                    { 58, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1124), "Filters_Product_58", 12.4801751105648m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1126) },
                    { 59, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1130), "Filters_Product_59", 12.1089477301959m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1132) },
                    { 60, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1158), "Filters_Product_60", 96.407711891893m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1160) },
                    { 61, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1164), "Filters_Product_61", 53.9614055583014m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1166) },
                    { 62, 13, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1169), "Filters_Product_62", 26.6239686847944m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1170) },
                    { 63, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1174), "Pastries_Product_63", 84.2221969718261m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1175) },
                    { 64, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1179), "Pastries_Product_64", 77.4154511428757m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1180) },
                    { 65, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1183), "Pastries_Product_65", 82.4542187359255m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1185) },
                    { 66, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1189), "Pastries_Product_66", 61.4370134209941m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1191) },
                    { 67, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1194), "Pastries_Product_67", 1.49691187136946m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1196) },
                    { 68, 14, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1199), "Pastries_Product_68", 30.8376853854718m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1200) },
                    { 69, 15, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1204), "Snacks_Product_69", 96.206813742679m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1205) },
                    { 70, 15, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1209), "Snacks_Product_70", 26.2249517605355m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1210) },
                    { 71, 15, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1213), "Snacks_Product_71", 53.7697321136391m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1215) },
                    { 72, 15, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1218), "Snacks_Product_72", 64.8840652622763m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1219) },
                    { 73, 15, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1223), "Snacks_Product_73", 38.3092621340722m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1224) },
                    { 74, 16, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1228), "Syrups_Product_74", 89.6013793822942m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1230) },
                    { 75, 16, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1234), "Syrups_Product_75", 62.213402406816m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1236) },
                    { 76, 16, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1240), "Syrups_Product_76", 26.7024711378894m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1242) },
                    { 77, 16, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1246), "Syrups_Product_77", 2.58616055041865m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1248) },
                    { 78, 16, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1252), "Syrups_Product_78", 33.2890924691161m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1254) },
                    { 79, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1258), "MilkAlternatives_Product_79", 43.412004692526m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1260) },
                    { 80, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1264), "MilkAlternatives_Product_80", 43.5707966102519m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1266) },
                    { 81, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1270), "MilkAlternatives_Product_81", 71.2231927471381m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1272) },
                    { 82, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1276), "MilkAlternatives_Product_82", 85.738483465914m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1278) },
                    { 83, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1282), "MilkAlternatives_Product_83", 42.6139904560449m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1284) },
                    { 84, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1288), "MilkAlternatives_Product_84", 7.43867489148471m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1290) },
                    { 85, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1294), "MilkAlternatives_Product_85", 90.4962373368914m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1296) },
                    { 86, 17, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1299), "MilkAlternatives_Product_86", 0.195085747373325m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1301) },
                    { 87, 18, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1306), "Merchandise_Product_87", 28.7324651182295m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1307) },
                    { 88, 18, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1311), "Merchandise_Product_88", 9.28959084902047m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1313) },
                    { 89, 18, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1316), "Merchandise_Product_89", 79.5036800931876m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1318) },
                    { 90, 18, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1321), "Merchandise_Product_90", 10.8738081387147m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1322) },
                    { 91, 18, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1325), "Merchandise_Product_91", 30.0191063787569m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1327) },
                    { 92, 19, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1330), "Subscriptions_Product_92", 81.7429766490015m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1332) },
                    { 93, 19, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1335), "Subscriptions_Product_93", 83.3831724259632m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1336) },
                    { 94, 19, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1340), "Subscriptions_Product_94", 99.4189285471502m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1341) },
                    { 95, 19, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1344), "Subscriptions_Product_95", 38.0154198548681m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1346) },
                    { 96, 19, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1349), "Subscriptions_Product_96", 6.44054916033342m, new DateTime(2024, 7, 4, 11, 57, 32, 201, DateTimeKind.Local).AddTicks(1351) }
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
