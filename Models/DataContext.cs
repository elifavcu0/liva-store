using Microsoft.EntityFrameworkCore;
namespace dotnet_store.Models;

public class DataContext : DbContext // Bu dosya C# ile veritabanı arasındaki iletişimi sağlar.
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Product> Products { get; set; } // Product C#'taki sınıf iken Products DB'de bulunan tablodur. Product ile ilgili yapılan işlemler Products tablosuna etki eder.
                                                 // Veriye sahip bir yapı değildir, sadece veriye ulaşma yeteneği vardır.
    public DbSet<Category> Categories { get; set; }
    public DbSet<Slider> Sliders { get; set; }
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
                        Name = "Apple Watch 7 " ,
                        Price= 20000,
                        IsActive =true,
                        Image ="1.jpeg",
                        IsHome = true,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 1
                    },
                new()
                    {
                        Id = 2,
                        Name = "Apple Watch 8" ,
                        Price= 100000,
                        IsActive =false,
                        Image ="2.jpeg",
                        IsHome = true,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 1
                    },
                new()
                    {
                        Id = 3,
                        Name = "Apple Watch 9" ,
                        Price= 30000,
                        IsActive =true,
                        Image ="3.jpeg",
                        IsHome = true,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 4,
                        Name = "Apple Watch 10" ,
                        Price= 40000,
                        IsActive =false,
                        Image ="4.jpeg",
                        IsHome = false,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 2
                    },
                new()
                    {
                        Id = 5,
                        Name = "Apple Watch 11" ,
                        Price= 50000,
                        IsActive =true,
                        Image ="5.jpeg",
                        IsHome = true,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 3
                    },
                new()
                    {
                        Id = 6,
                        Name = "Apple Watch 12" ,
                        Price= 60000,
                        IsActive =false,
                        Image ="6.jpeg",
                        IsHome = false,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 3
                    },
                new()
                    {
                        Id = 7,
                        Name = "Apple Watch 13" ,
                        Price= 70000,
                        IsActive =false,
                        Image ="7.jpeg",
                        IsHome = false,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 4
                    },
                new()
                    {
                        Id = 8,
                        Name = "Apple Watch 14" ,
                        Price= 80000,
                        IsActive =true,
                        Image ="8.jpeg",
                        IsHome = true,
                        Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod.",
                        CategoryId = 5
                    }
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