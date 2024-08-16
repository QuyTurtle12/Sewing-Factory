using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace SewingFactory.Repository.Migrations
{
    /// <inheritdoc />
    public partial class ModifiedProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_productID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_userID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_categoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_groupID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Orders_orderID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_creatorID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_groupID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_roleID",
                table: "Users");

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("474bde6d-decf-4a70-b660-1d5b35f93b39"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("5143d0e9-733c-45e8-a6e8-cdac75ee17f5"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("b993825b-8b6f-45ce-984a-52c0d7c601aa"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("bbb84064-85bf-491b-99d9-b0c1bfcd83e7"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "ID",
                keyValue: new Guid("c956dcdc-6e00-4285-b605-5eb7877638b9"));

            migrationBuilder.RenameColumn(
                name: "username",
                table: "Users",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "salary",
                table: "Users",
                newName: "Salary");

            migrationBuilder.RenameColumn(
                name: "roleID",
                table: "Users",
                newName: "RoleID");

            migrationBuilder.RenameColumn(
                name: "password",
                table: "Users",
                newName: "Password");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Users",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "groupID",
                table: "Users",
                newName: "GroupID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_roleID",
                table: "Users",
                newName: "IX_Users_RoleID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_groupID",
                table: "Users",
                newName: "IX_Users_GroupID");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Tasks",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "orderID",
                table: "Tasks",
                newName: "OrderID");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Tasks",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "groupID",
                table: "Tasks",
                newName: "GroupID");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Tasks",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "deadline",
                table: "Tasks",
                newName: "Deadline");

            migrationBuilder.RenameColumn(
                name: "creatorID",
                table: "Tasks",
                newName: "CreatorID");

            migrationBuilder.RenameColumn(
                name: "createdDate",
                table: "Tasks",
                newName: "CreatedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_orderID",
                table: "Tasks",
                newName: "IX_Tasks_OrderID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_groupID",
                table: "Tasks",
                newName: "IX_Tasks_GroupID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_creatorID",
                table: "Tasks",
                newName: "IX_Tasks_CreatorID");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Roles",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Products",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Products",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "categoryID",
                table: "Products",
                newName: "CategoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_categoryID",
                table: "Products",
                newName: "IX_Products_CategoryID");

            migrationBuilder.RenameColumn(
                name: "userID",
                table: "Orders",
                newName: "UserID");

            migrationBuilder.RenameColumn(
                name: "totalAmount",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "status",
                table: "Orders",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "quantity",
                table: "Orders",
                newName: "Quantity");

            migrationBuilder.RenameColumn(
                name: "productID",
                table: "Orders",
                newName: "ProductID");

            migrationBuilder.RenameColumn(
                name: "orderDate",
                table: "Orders",
                newName: "OrderDate");

            migrationBuilder.RenameColumn(
                name: "finishedDate",
                table: "Orders",
                newName: "FinishedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_userID",
                table: "Orders",
                newName: "IX_Orders_UserID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_productID",
                table: "Orders",
                newName: "IX_Orders_ProductID");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Groups",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_ProductID",
                table: "Orders",
                column: "ProductID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders",
                column: "UserID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products",
                column: "CategoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_GroupID",
                table: "Tasks",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Orders_OrderID",
                table: "Tasks",
                column: "OrderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_CreatorID",
                table: "Tasks",
                column: "CreatorID",
                principalTable: "Users",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_GroupID",
                table: "Users",
                column: "GroupID",
                principalTable: "Groups",
                principalColumn: "ID");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users",
                column: "RoleID",
                principalTable: "Roles",
                principalColumn: "ID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Products_ProductID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Users_UserID",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Products_Categories_CategoryID",
                table: "Products");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Groups_GroupID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Orders_OrderID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Tasks_Users_CreatorID",
                table: "Tasks");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Groups_GroupID",
                table: "Users");

            migrationBuilder.DropForeignKey(
                name: "FK_Users_Roles_RoleID",
                table: "Users");

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

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Users",
                newName: "username");

            migrationBuilder.RenameColumn(
                name: "Salary",
                table: "Users",
                newName: "salary");

            migrationBuilder.RenameColumn(
                name: "RoleID",
                table: "Users",
                newName: "roleID");

            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "password");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Users",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "GroupID",
                table: "Users",
                newName: "groupID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_RoleID",
                table: "Users",
                newName: "IX_Users_roleID");

            migrationBuilder.RenameIndex(
                name: "IX_Users_GroupID",
                table: "Users",
                newName: "IX_Users_groupID");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Tasks",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "OrderID",
                table: "Tasks",
                newName: "orderID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Tasks",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "GroupID",
                table: "Tasks",
                newName: "groupID");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Tasks",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Deadline",
                table: "Tasks",
                newName: "deadline");

            migrationBuilder.RenameColumn(
                name: "CreatorID",
                table: "Tasks",
                newName: "creatorID");

            migrationBuilder.RenameColumn(
                name: "CreatedDate",
                table: "Tasks",
                newName: "createdDate");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_OrderID",
                table: "Tasks",
                newName: "IX_Tasks_orderID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_GroupID",
                table: "Tasks",
                newName: "IX_Tasks_groupID");

            migrationBuilder.RenameIndex(
                name: "IX_Tasks_CreatorID",
                table: "Tasks",
                newName: "IX_Tasks_creatorID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Roles",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "Products",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Products",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "CategoryID",
                table: "Products",
                newName: "categoryID");

            migrationBuilder.RenameIndex(
                name: "IX_Products_CategoryID",
                table: "Products",
                newName: "IX_Products_categoryID");

            migrationBuilder.RenameColumn(
                name: "UserID",
                table: "Orders",
                newName: "userID");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                table: "Orders",
                newName: "totalAmount");

            migrationBuilder.RenameColumn(
                name: "Status",
                table: "Orders",
                newName: "status");

            migrationBuilder.RenameColumn(
                name: "Quantity",
                table: "Orders",
                newName: "quantity");

            migrationBuilder.RenameColumn(
                name: "ProductID",
                table: "Orders",
                newName: "productID");

            migrationBuilder.RenameColumn(
                name: "OrderDate",
                table: "Orders",
                newName: "orderDate");

            migrationBuilder.RenameColumn(
                name: "FinishedDate",
                table: "Orders",
                newName: "finishedDate");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_UserID",
                table: "Orders",
                newName: "IX_Orders_userID");

            migrationBuilder.RenameIndex(
                name: "IX_Orders_ProductID",
                table: "Orders",
                newName: "IX_Orders_productID");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Groups",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Categories",
                newName: "name");

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "ID", "name" },
                values: new object[,]
                {
                    { new Guid("474bde6d-decf-4a70-b660-1d5b35f93b39"), "Cashier" },
                    { new Guid("5143d0e9-733c-45e8-a6e8-cdac75ee17f5"), "Staff Manager" },
                    { new Guid("b993825b-8b6f-45ce-984a-52c0d7c601aa"), "Order Manager" },
                    { new Guid("bbb84064-85bf-491b-99d9-b0c1bfcd83e7"), "Sewing Staff" },
                    { new Guid("c956dcdc-6e00-4285-b605-5eb7877638b9"), "Product Manager" }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Products_productID",
                table: "Orders",
                column: "productID",
                principalTable: "Products",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Users_userID",
                table: "Orders",
                column: "userID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Products_Categories_categoryID",
                table: "Products",
                column: "categoryID",
                principalTable: "Categories",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Groups_groupID",
                table: "Tasks",
                column: "groupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Orders_orderID",
                table: "Tasks",
                column: "orderID",
                principalTable: "Orders",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Tasks_Users_creatorID",
                table: "Tasks",
                column: "creatorID",
                principalTable: "Users",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Groups_groupID",
                table: "Users",
                column: "groupID",
                principalTable: "Groups",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_Roles_roleID",
                table: "Users",
                column: "roleID",
                principalTable: "Roles",
                principalColumn: "ID",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
