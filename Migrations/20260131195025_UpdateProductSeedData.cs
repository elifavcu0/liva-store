using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace dotnet_store.Migrations
{
    /// <inheritdoc />
    public partial class UpdateProductSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 2, "A larger screen area for easier viewing and use. And an optimized user interface. Two specially designed new dials. All in a redesigned case. The most crack-resistant front crystal. IP6X dust resistance rating. WR50 water resistance rating for use in the sea or pool.", "Apple Watch Series 7 ", 18399.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 2, "Created to be indispensable. Now equipped with even more powerful features to make you feel good. Temperature sensing feature that gives you information about your overall well-being. Traffic Accident Detection that helps you get help in an emergency. Sleep Stages that help you better understand your sleep cycles. And a flawlessly beautiful design that reflects the future.", "Apple Watch Series 8", 17599.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "The more you know about your health, the easier it is to take precautions. With many apps like ECG and Vital Signs, the Apple Watch Series 11 gives you a big picture of your health, keeping you informed at all times. And now, Series 11 is opening a new chapter in heart health with an innovative feature: hypertension notifications.", "Apple Watch Series 11", 19999.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, "Discover new AI-powered photo editing options. Now you can effortlessly perfect your photos, making every image stand out. And there's more. Even if you don't capture the exact shot you want, Creative Edit lets you fill in backgrounds and make unwanted objects *poof* disappear.", "Samsung Galaxy S24 128 GB 8 GB Ram (Samsung Türkiye Warranty) Black", 38999.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, "Introducing the Galaxy A56 5G. With a thickness of 7.4 mm and a weight of 198 g, the Galaxy A56 5G offers an easy grip. Its advanced cameras are grouped to fit the new linear design. The Galaxy A56 5G is available in four colors: Anthracite, Gray, Green, and Light Pink.", "Samsung Galaxy A56 5G 8 GB RAM 256 GB Gri", 22900.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "IsActive", "Name", "Price" },
                values: new object[] { 1, "The iPhone 17 Pro's Apple A19 Pro processor runs at 4.26 GHz, delivering highly efficient performance. 12 GB of RAM ensures seamless multitasking, while 256 GB of storage provides ample capacity for various needs. AI-powered features optimize system performance for smarter and more efficient use. The iOS 26 operating system lets you take advantage of the latest features.", true, "APPLE iPhone 17 Pro 256 GB Deep Blue ", 107999.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 10, "Dual-layer foam creates our most cushioned stability shoe to date. Our midfoot support system wraps the heel and arch for optimal stability and a smooth heel-to-toe transition.", "Nike Structure Plus", 9999.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 7, "High-quality fabric and special details give the jacket a stylish look. The double-sided smooth fleece fabric offers a soft and shape-retaining feel; the drawstring at the waist allows you to adjust the silhouette as desired.", "Nike Pregame Fleece", 6599.0 });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "CategoryId", "Description", "Image", "IsActive", "IsHome", "Name", "Price" },
                values: new object[,]
                {
                    { 9, 2, "Combining style and functionality, the English Home TMK 5030 Grill and Toaster Inox offers practical solutions for your kitchen. Its large surface area allows for both toasting and grilling, making it a perfect aid for daily use and entertaining guests. Its stainless steel body makes it highly durable, while its modern inox design adds an aesthetic touch to your kitchen decor.", "9.jpg", true, true, "English Home TMK 5030 Izgara ve Tost Makinesi Inox", 3699.0 },
                    { 10, 5, "Designed for young and free spirits, English Home Sweet Séduction offers an energetic and captivating fragrance experience. Its top notes of fresh and vibrant pink pepper, orange, and honey boost your energy, while the heart notes of jasmine and orange blossom add a floral and romantic elegance. Finally, the base notes of vanilla, patchouli, and caramel leave a sweet, lasting, and sophisticated trail.", "10.jpg", true, true, "Sweet Séduction Kadın Parfümü 100 ml Lila", 1399.99 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 7 ", 20000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 1, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 8", 100000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "Description", "Name", "Price" },
                values: new object[] { "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 9", 30000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 2, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 10", 40000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 3, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 11", 50000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6,
                columns: new[] { "CategoryId", "Description", "IsActive", "Name", "Price" },
                values: new object[] { 3, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", false, "Apple Watch 12", 60000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 4, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 13", 70000.0 });

            migrationBuilder.UpdateData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8,
                columns: new[] { "CategoryId", "Description", "Name", "Price" },
                values: new object[] { 5, "Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.", "Apple Watch 14", 80000.0 });
        }
    }
}
