using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SewingFactory.Repository.Migrations
{
    /// <inheritdoc />
    public partial class AddNewPropertiesToProductAndUserAndOrder : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("3af9bfa0-a6a0-4fb9-b3f4-77a87cf670c0"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("9d621dc0-ff6f-47b7-87e2-758383f4b13f"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("b4811f59-6537-41a9-890d-d41f8a7475a8"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("f624ee68-31e2-465b-a719-120120c4fecf"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("fd831fa3-53fb-49d5-8521-58b8e2964a35"));

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Users",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "Products",
                type: "bit",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerName",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerPhone",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("226106cb-c589-4ecf-8a66-ebeaefc9eb2b"), "Order Manager" },
                    { new Guid("42c2e00f-54d4-4d07-8688-d00449b1b430"), "Sewing Staff" },
                    { new Guid("7bc584c7-7c67-4601-a418-f63f85966e07"), "Cashier" },
                    { new Guid("a9a731cb-fb75-4f36-a615-681caa0c2b46"), "Staff Manager" },
                    { new Guid("f626ae14-39e2-4b6d-abd9-03656243b2ac"), "Product Manager" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("226106cb-c589-4ecf-8a66-ebeaefc9eb2b"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("42c2e00f-54d4-4d07-8688-d00449b1b430"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("7bc584c7-7c67-4601-a418-f63f85966e07"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("a9a731cb-fb75-4f36-a615-681caa0c2b46"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("f626ae14-39e2-4b6d-abd9-03656243b2ac"));

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CustomerName",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CustomerPhone",
                table: "Orders");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "Name" },
                values: new object[,]
                {
                    { new Guid("3af9bfa0-a6a0-4fb9-b3f4-77a87cf670c0"), "Product Manager" },
                    { new Guid("9d621dc0-ff6f-47b7-87e2-758383f4b13f"), "Cashier" },
                    { new Guid("b4811f59-6537-41a9-890d-d41f8a7475a8"), "Order Manager" },
                    { new Guid("f624ee68-31e2-465b-a719-120120c4fecf"), "Sewing Staff" },
                    { new Guid("fd831fa3-53fb-49d5-8521-58b8e2964a35"), "Staff Manager" }
                });
        }
    }
}
