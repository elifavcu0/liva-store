using Microsoft.EntityFrameworkCore;
namespace dotnet_store.Models;

public class DataContext : DbContext // Bu dosya C# ile veritabanı arasındaki iletişimi sağlar.
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    { }
    public DbSet<Product> Products { get; set; } // Product C#'taki sınıf iken Products DB'de bulunan tablodur. Product ile ilgili yapılan işlemler Products tablosuna etki eder.
                                                 // Veriye sahip bir yapı değildir, sadece veriye ulaşma yeteneği vardır.
    protected override void OnModelCreating(ModelBuilder modelBuilder) // Bir migration oluşturulunca burası çalışır
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>().HasData(
            new List<Product>
            {
                new(){ Id = 1, 
                        Name = "Apple Watch 7 " ,
                         Price= 20000, IsActive =true,Image ="1.jpeg",IsHome = true,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 2, Name = "Apple Watch 8" , Price= 100000, IsActive =false,Image ="2.jpeg",IsHome = true,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 3, Name = "Apple Watch 9" , Price= 30000, IsActive =true,Image ="3.jpeg",IsHome = true,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 4, Name = "Apple Watch 10" , Price= 40000, IsActive =false,Image ="4.jpeg",IsHome = false,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 5, Name = "Apple Watch 11" , Price= 50000, IsActive =true,Image ="5.jpeg",IsHome = true,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 6, Name = "Apple Watch 12" , Price= 60000, IsActive =false,Image ="6.jpeg",IsHome = false,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 7, Name = "Apple Watch 13" , Price= 70000, IsActive =false,Image ="7.jpeg",IsHome = false,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."},
                new(){ Id = 8, Name = "Apple Watch 14" , Price= 80000, IsActive =true,Image ="8.jpeg",IsHome = true,Description="Lorem, ipsum dolor sit amet consectetur adipisicing elit. Harum at velit possimus aliquid, quisquam quam veniam corporis! Dolor, voluptas deleniti asperiores quibusdam iste in quod."}
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