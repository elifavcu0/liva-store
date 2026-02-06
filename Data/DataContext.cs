using dotnet_store.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
namespace dotnet_store.Data;

public class DataContext : IdentityDbContext<AppUser, AppRole, int> //Primary Key (Birincil Anahtar) tipini int olarak belirledik.
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Product> Products { get; set; } // Product C#'taki sınıf iken Products DB'de bulunan tablodur. Product ile ilgili yapılan işlemler Products tablosuna etki eder.
                                                 // Veriye sahip bir yapı değildir, sadece veriye ulaşma yeteneği vardır.
    public DbSet<Category> Categories { get; set; }
    public DbSet<Slider> Sliders { get; set; }
    public DbSet<Address> Addresses { get; set; }
    public DbSet<Cart> Carts { get; set; }
    public DbSet<Order> Orders { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder) // Bir migration oluşturulunca burası çalışır
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Slider>().HasData(
            new List<Slider>
            {
                new()
                {
                    Id = 1, Title ="Slider 1 Title",Description ="Slider 1 Description",Image = "slider-1.jpeg",Index = 0,IsActive =true
                },
                new()
                {
                    Id = 2, Title ="Slider 2 Title",Description ="Slider 2 Description",Image = "slider-2.jpeg",Index = 1,IsActive =true
                },
                new()
                {
                    Id = 3, Title ="Slider 3 Title",Description ="Slider 3 Description",Image = "slider-3.jpeg",Index = 2,IsActive =true
                },
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new List<Category>
            {
                new()
                {
                    Id = 1, Name ="Phone",Url ="phone"
                },
                new()
                {
                    Id = 2, Name ="Electronics",Url ="electronics"
                },
                new()
                {
                    Id = 3, Name ="White Goods",Url ="white-goods"
                },
                new()
                {
                    Id = 4, Name ="Clothing",Url ="clothing"
                },
                new()
                {
                    Id = 5, Name ="Cosmetics",Url ="cosmetics"
                },
                new()
                {
                    Id = 6, Name ="Mother & Child",Url ="mother-child"
                },
                new()
                {
                    Id = 7, Name ="Women",Url ="Women"
                },
                new()
                {
                    Id = 8, Name ="Male",Url ="Male"
                },
                new()
                {
                    Id = 9, Name ="Home & Living",Url ="home-living"
                },
                new()
                {
                    Id = 10, Name ="Shoes & Bags",Url ="shoes-bags"
                },
            }
        );

        modelBuilder.Entity<Product>().HasData(
            new List<Product>
            {
                new()
                    {
                        Id = 1,
                        Name = "Apple Watch Series 7 " ,
                        Price= 18399,
                        IsActive = true,
                        Image ="1.jpeg",
                        IsHome = true,
                        Description="A larger screen area for easier viewing and use. And an optimized user interface. Two specially designed new dials. All in a redesigned case. The most crack-resistant front crystal. IP6X dust resistance rating. WR50 water resistance rating for use in the sea or pool.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 2,
                        Name = "Apple Watch Series 8" ,
                        Price= 17599,
                        IsActive = false,
                        Image ="2.jpeg",
                        IsHome = true,
                        Description="Created to be indispensable. Now equipped with even more powerful features to make you feel good. Temperature sensing feature that gives you information about your overall well-being. Traffic Accident Detection that helps you get help in an emergency. Sleep Stages that help you better understand your sleep cycles. And a flawlessly beautiful design that reflects the future.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 3,
                        Name = "Apple Watch Series 11" ,
                        Price= 19999,
                        IsActive =true,
                        Image ="3.jpeg",
                        IsHome = true,
                        Description="The more you know about your health, the easier it is to take precautions. With many apps like ECG and Vital Signs, the Apple Watch Series 11 gives you a big picture of your health, keeping you informed at all times. And now, Series 11 is opening a new chapter in heart health with an innovative feature: hypertension notifications.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 4,
                        Name = "Samsung Galaxy S24 128 GB 8 GB Ram (Samsung Türkiye Warranty) Black" ,
                        Price= 38999,
                        IsActive =false,
                        Image ="4.jpeg",
                        IsHome = false,
                        Description="Discover new AI-powered photo editing options. Now you can effortlessly perfect your photos, making every image stand out. And there's more. Even if you don't capture the exact shot you want, Creative Edit lets you fill in backgrounds and make unwanted objects *poof* disappear.",
                        CategoryId = 1
                    },
                new()
                    {
                        Id = 5,
                        Name = "Samsung Galaxy A56 5G 8 GB RAM 256 GB Gray" ,
                        Price= 22900,
                        IsActive =true,
                        Image ="5.jpeg",
                        IsHome = true,
                        Description="Introducing the Galaxy A56 5G. With a thickness of 7.4 mm and a weight of 198 g, the Galaxy A56 5G offers an easy grip. Its advanced cameras are grouped to fit the new linear design. The Galaxy A56 5G is available in four colors: Anthracite, Gray, Green, and Light Pink.",
                        CategoryId = 1
                    },
                new()
                    {
                        Id = 6,
                        Name = "APPLE iPhone 17 Pro 256 GB Deep Blue " ,
                        Price= 107999,
                        IsActive = true,
                        Image ="6.jpeg",
                        IsHome = false,
                        Description="The iPhone 17 Pro's Apple A19 Pro processor runs at 4.26 GHz, delivering highly efficient performance. 12 GB of RAM ensures seamless multitasking, while 256 GB of storage provides ample capacity for various needs. AI-powered features optimize system performance for smarter and more efficient use. The iOS 26 operating system lets you take advantage of the latest features.",
                        CategoryId = 1
                    },
                new()
                    {
                        Id = 7,
                        Name = "Nike Structure Plus" ,
                        Price= 9999,
                        IsActive =false,
                        Image ="7.jpeg",
                        IsHome = false,
                        Description="Dual-layer foam creates our most cushioned stability shoe to date. Our midfoot support system wraps the heel and arch for optimal stability and a smooth heel-to-toe transition.",
                        CategoryId = 10
                    },
                new()
                    {
                        Id = 8,
                        Name = "Nike Pregame Fleece" ,
                        Price= 6599,
                        IsActive =true,
                        Image ="8.jpeg",
                        IsHome = true,
                        Description="High-quality fabric and special details give the jacket a stylish look. The double-sided smooth fleece fabric offers a soft and shape-retaining feel; the drawstring at the waist allows you to adjust the silhouette as desired.",
                        CategoryId = 7
                    },
                new()
                    {
                        Id = 9,
                        Name = "English Home TMK 5030 Izgara ve Tost Makinesi Inox" ,
                        Price= 3699,
                        IsActive =true,
                        Image ="9.jpg",
                        IsHome = true,
                        Description="Combining style and functionality, the English Home TMK 5030 Grill and Toaster Inox offers practical solutions for your kitchen. Its large surface area allows for both toasting and grilling, making it a perfect aid for daily use and entertaining guests. Its stainless steel body makes it highly durable, while its modern inox design adds an aesthetic touch to your kitchen decor.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 10,
                        Name = "Sweet Séduction Kadın Parfümü 100 ml Lila" ,
                        Price= 1399.99m,
                        IsActive =true,
                        Image ="10.jpg",
                        IsHome = true,
                        Description="Designed for young and free spirits, English Home Sweet Séduction offers an energetic and captivating fragrance experience. Its top notes of fresh and vibrant pink pepper, orange, and honey boost your energy, while the heart notes of jasmine and orange blossom add a floral and romantic elegance. Finally, the base notes of vanilla, patchouli, and caramel leave a sweet, lasting, and sophisticated trail.",
                        CategoryId = 5
                    },
            }
        );
    }
};

/*
dotnet ef database update komutunu yazdığımızda, OnModelCreating içindeki kodlar çalışır ve Apple Watch verilerini alıp DB'e yazar ve işi biter.
Bu noktada C# kodundaki (DataContext.cs) DbSet satırında hiçbir değişiklik olmaz.

Çalışma Anı (Runtime): Programı çalıştırıp _context.Products.ToList() dediğimizde;
DbSet satırı veritabanına erişir, ne görürse onu çeker.
DB, önceden (Migration ile) doldurulduğu için, Apple Watch verileri çekilir.*/