using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_store.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Products",
                type: "TEXT",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsHome",
                table: "Products",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Description", "Image", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "1.jpeg", true });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Description", "Image", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "2.jpeg", true });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Image", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "3.jpeg", true });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "Description", "Image", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "4.jpeg", false });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "Description", "Image", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "5.jpeg", true });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "Description", "Image", "IsActive", "IsHome" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "6.jpeg", false, false });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Description", "Image", "IsActive", "IsHome", "Name", "Price" },
                values: new object[,]
                {
                    { 7, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "7.jpeg", false, false, "Apple Watch 13", 70000.0 },
                    { 8, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "8.jpeg", true, true, "Apple Watch 14", 80000.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "IsHome",
                table: "Products");

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                column: "IsActive",
                value: true);
        }
    }
}
