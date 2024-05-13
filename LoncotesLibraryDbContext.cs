using Microsoft.EntityFrameworkCore;
using Library;

public class LoncotesLibraryDbContext : DbContext
{

    public DbSet<Checkout> Checkouts { get; set; }
    public DbSet<Genre> Genres { get; set; }
    public DbSet<Material> Materials { get; set; }
    public DbSet<MaterialType> MaterialTypes { get; set; }
    public DbSet<Patron> Patrons { get; set; }

    public LoncotesLibraryDbContext(DbContextOptions<LoncotesLibraryDbContext> context) : base(context)
    {

    }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Seed MaterialTypes
        modelBuilder.Entity<MaterialType>().HasData(
            new MaterialType { Id = 1, Name = "Book", CheckoutDays = 14 }, // 14 days checkout period for books
            new MaterialType { Id = 2, Name = "Periodical", CheckoutDays = 7 }, // 7 days checkout period for periodicals
            new MaterialType { Id = 3, Name = "CD", CheckoutDays = 10 } // 10 days checkout period for CDs
        );

        // Seed Patrons
        modelBuilder.Entity<Patron>().HasData(
            new Patron { Id = 1, FirstName = "Gator", LastName = "Golf", Address = "123 Main St", Email = "john@example.com", IsActive = true },
            new Patron { Id = 2, FirstName = "Chicken", LastName = "Limbo", Address = "456 Elm St", Email = "jane@example.com", IsActive = true }
        );

        // Seed Genres
        modelBuilder.Entity<Genre>().HasData(
            new Genre { Id = 1, Name = "SciFi" },
            new Genre { Id = 2, Name = "History" },
            new Genre { Id = 3, Name = "Fantasy" },
            new Genre { Id = 4, Name = "Mystery" },
            new Genre { Id = 5, Name = "Romance" }
        );

        // Seed Materials (with specific titles)
        modelBuilder.Entity<Material>().HasData(
            new Material { Id = 1, MaterialName = "Clifford: The Puppy Years", MaterialTypeId = 1, GenreId = 1 }, // Book
            new Material { Id = 2, MaterialName = "Hands-On Programming with C#", MaterialTypeId = 3, GenreId = 1 }, // CD
            new Material { Id = 3, MaterialName = "National Geographic", MaterialTypeId = 2, GenreId = 2 }, // Periodical
            new Material { Id = 4, MaterialName = "Spice Girls: Greatest Hits", MaterialTypeId = 3, GenreId = 5 }, // CD
            new Material { Id = 5, MaterialName = "Arthur's New Puppy", MaterialTypeId = 1, GenreId = 3 }, // Book
            new Material { Id = 6, MaterialName = "DC Comics", MaterialTypeId = 2, GenreId = 4 }, // Periodical
            new Material { Id = 7, MaterialName = "Infest by Papa Roach", MaterialTypeId = 3, GenreId = 5 }, // CD
            new Material { Id = 8, MaterialName = "House of the Dead: Overkill", MaterialTypeId = 1, GenreId = 4 }, // Book
            new Material { Id = 9, MaterialName = "Edgar Allan Poe: The Complete Tales and Poems", MaterialTypeId = 3, GenreId = 4 }, // CD
            new Material { Id = 10, MaterialName = "The Beef Chronicles: Kendrick vs. Drake", MaterialTypeId = 1, GenreId = 5 } // Book
                                                                                                                                // Add more materials here if needed
        );
    }
}