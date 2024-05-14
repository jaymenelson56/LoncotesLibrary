using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Http.Json;
using Library;
using Library.Models.DTO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// allows our api endpoints to access the database through Entity Framework Core
builder.Services.AddNpgsql<LoncotesLibraryDbContext>(builder.Configuration["LoncotesLibraryDbConnectionString"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/materials", (LoncotesLibraryDbContext db, int? materialTypeId, int? genreId) =>
{
    var query = db.Materials
        .Where(m => m.OutOfCirculationSince == null)
        .Include(m => m.MaterialType) // Include the MaterialType related entity
        .Include(m => m.Genre) // Include the Genre related entity
        .AsQueryable(); // Create a base query

    // Apply filters based on query string parameters
    if (materialTypeId.HasValue)
    {
        query = query.Where(m => m.MaterialTypeId == materialTypeId);
    }

    if (genreId.HasValue)
    {
        query = query.Where(m => m.GenreId == genreId);
    }

    // Project the results into MaterialDTO
    var materials = query.Select(m => new MaterialDTO
    {
        Id = m.Id,
        MaterialName = m.MaterialName,
        MaterialTypeId = m.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = m.MaterialType.Id,
            Name = m.MaterialType.Name,
            CheckoutDays = m.MaterialType.CheckoutDays
        },
        GenreId = m.GenreId,
        Genre = new GenreDTO
        {
            Id = m.Genre.Id,
            Name = m.Genre.Name
        },
        OutOfCirculationSince = m.OutOfCirculationSince
    }).ToList();

    return Results.Ok(materials);
});

app.MapGet("/api/materials/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    var material = db.Materials
        .Include(m => m.Genre)
        .Include(m => m.MaterialType)
        .Include(m => m.Checkouts)
            .ThenInclude(co => co.Patron)
        .SingleOrDefault(m => m.Id == id);

    if (material == null)
    {
        return Results.NotFound("Material not found.");
    }

    return Results.Ok(new MaterialDTO
    {
        Id = material.Id,
        MaterialName = material.MaterialName,
        MaterialTypeId = material.MaterialTypeId,
        MaterialType = new MaterialTypeDTO
        {
            Id = material.MaterialType.Id,
            Name = material.MaterialType.Name,
            CheckoutDays = material.MaterialType.CheckoutDays
        },
        GenreId = material.GenreId,
        Genre = new GenreDTO
        {
            Id = material.Genre.Id,
            Name = material.Genre.Name
        },
        Checkouts = material.Checkouts.Select(co => new CheckoutDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            MaterialType = new MaterialTypeDTO
            {
                Id = co.Material.MaterialType.Id,
                Name = co.Material.MaterialType.Name,
                CheckoutDays = co.Material.MaterialType.CheckoutDays
            },
            PatronId = co.PatronId,
            Patron = new PatronDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate,
            Paid = co.Paid
        }).ToList()
    });
});

app.MapPost("/api/materials", (LoncotesLibraryDbContext db, MaterialDTO materialDTO) =>
{
    // Map MaterialDTO to Material entity
    var material = new Material
    {
        MaterialName = materialDTO.MaterialName,
        MaterialTypeId = materialDTO.MaterialTypeId,
        GenreId = materialDTO.GenreId
        // You can map other properties here if needed
    };

    // Add the new material to the database
    db.Materials.Add(material);
    db.SaveChanges();

    // Return the created material
    return Results.Created($"/api/materials/{material.Id}", material);
});

app.MapPut("/api/materials/{id}/setOutOfCirculation", (LoncotesLibraryDbContext db, int id) =>
{
    // Find the material by id
    var material = db.Materials.SingleOrDefault(m => m.Id == id);

    if (material == null)
    {
        return Results.NotFound("Material not found.");
    }

    // Set the OutOfCirculationSince property to DateTime.Now
    material.OutOfCirculationSince = DateTime.Now;

    // Save the changes to the database
    db.SaveChanges();

    return Results.Ok();
});

app.MapGet("/api/materialtypes", (LoncotesLibraryDbContext db) =>
{
    var materialTypes = db.MaterialTypes.ToList();

    if (materialTypes == null || materialTypes.Count == 0)
    {
        return Results.NotFound("No material types found.");
    }

    return Results.Ok(materialTypes);
});

app.MapGet("/api/genres", (LoncotesLibraryDbContext db) =>
{
    var genres = db.Genres.ToList();

    if (genres == null || genres.Count == 0)
    {
        return Results.NotFound("No genres found.");
    }

    return Results.Ok(genres);
});

app.MapGet("/api/patrons", (LoncotesLibraryDbContext db) =>
{
    var patrons = db.Patrons.ToList();

    if (patrons == null || patrons.Count == 0)
    {
        return Results.NotFound("No patrons found.");
    }

    return Results.Ok(patrons);
});


app.MapGet("/api/patrons/{id}", (LoncotesLibraryDbContext db, int id) =>
{
    var patron = db.Patrons
        .Include(p => p.Checkouts)
            .ThenInclude(co => co.Material)
                .ThenInclude(m => m.MaterialType)
        .SingleOrDefault(p => p.Id == id);

    if (patron == null)
    {
        return Results.NotFound("Patron not found.");
    }

    // Map Patron entity to PatronDTO
    var patronDTO = new PatronDTO
    {
        Id = patron.Id,
        FirstName = patron.FirstName,
        LastName = patron.LastName,
        Address = patron.Address,
        Email = patron.Email,
        IsActive = patron.IsActive,
        Checkouts = patron.Checkouts?.Select(co => new CheckoutWithLateFeeDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            PatronId = co.PatronId,
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate,
            Paid = co.Paid,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                // Add other properties of MaterialDTO if needed
                MaterialType = new MaterialTypeDTO
                {
                    // Add properties of MaterialTypeDTO
                    Id = co.Material.MaterialType.Id,
                    // Add other properties of MaterialTypeDTO if needed
                }
            }
        }).ToList()
    };

    // Return the mapped PatronDTO
    return Results.Ok(patronDTO);
});

app.MapPut("/api/patrons/{id}/updateaddress", (LoncotesLibraryDbContext db, int id, PatronDTO updatePatronDTO) =>
{
    var patron = db.Patrons.FirstOrDefault(p => p.Id == id);

    if (patron == null)
    {
        return Results.NotFound("Patron not found.");
    }

    patron.Address = updatePatronDTO.Address ?? patron.Address;
    patron.Email = updatePatronDTO.Email ?? patron.Email;

    db.SaveChanges();

    return Results.Ok();
});

app.MapPut("/api/patrons/{id}/toggleActive", (LoncotesLibraryDbContext db, int id) =>
{
    var patron = db.Patrons.FirstOrDefault(p => p.Id == id);

    if (patron == null)
    {
        return Results.NotFound("Patron not found.");
    }

    patron.IsActive = !patron.IsActive; // Toggle IsActive property

    db.SaveChanges();

    return Results.Ok();
});

app.MapPost("/api/checkout", (LoncotesLibraryDbContext db, CheckoutDTO newCheckoutDTO) =>
{

    var checkout = new Checkout
    {
        MaterialId = newCheckoutDTO.MaterialId,
        PatronId = newCheckoutDTO.PatronId,
        Paid = newCheckoutDTO.Paid,
        CheckoutDate = DateTime.Today // Set checkout date to today
        // You can add other properties if needed
    };

    db.Checkouts.Add(checkout);
    db.SaveChanges();

    return Results.Created($"/api/checkout/{checkout.Id}", checkout);
});

app.MapPut("/api/checkout/{id}/return", (LoncotesLibraryDbContext db, int id) =>
{
    var checkout = db.Checkouts.Find(id);

    if (checkout == null)
    {
        return Results.NotFound("Checkout not found.");
    }

    checkout.ReturnDate = DateTime.Today;

    db.SaveChanges();

    return Results.Ok();
});

app.MapGet("/api/materials/available", (LoncotesLibraryDbContext db) =>
{
    var materialsWithCheckouts = db.Materials
        .Where(m => m.OutOfCirculationSince == null &&
                    (!m.Checkouts.Any() || m.Checkouts.All(co => co.ReturnDate != null)))
        .Select(material => new MaterialDTO
        {
            Id = material.Id,
            MaterialName = material.MaterialName,
            MaterialTypeId = material.MaterialTypeId,
            GenreId = material.GenreId,
            OutOfCirculationSince = material.OutOfCirculationSince,
            Checkouts = material.Checkouts
                .Select(co => new CheckoutDTO
                {
                    Id = co.Id,
                    MaterialId = co.MaterialId,
                    PatronId = co.PatronId,
                    CheckoutDate = co.CheckoutDate,
                    ReturnDate = co.ReturnDate,
                    Paid = co.Paid,
                    Patron = new PatronDTO
                    {
                        Id = co.Patron.Id,
                        FirstName = co.Patron.FirstName,
                        LastName = co.Patron.LastName,
                        Address = co.Patron.Address,
                        Email = co.Patron.Email,
                        IsActive = co.Patron.IsActive
                    }
                })
                .ToList()
        })
        .ToList();

    return Results.Ok(materialsWithCheckouts);
});

app.MapGet("/api/checkouts", (LoncotesLibraryDbContext db) =>
{
    var checkouts = db.Checkouts
        .Include(co => co.Material)
            .ThenInclude(m => m.MaterialType)
        .Include(co => co.Patron)
        .Select(co => new CheckoutWithLateFeeDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            PatronId = co.PatronId,
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate,
            Paid = co.Paid,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                // Add other properties of MaterialDTO if needed
                MaterialType = new MaterialTypeDTO
                {
                    // Add properties of MaterialTypeDTO
                    Id = co.Material.MaterialType.Id,
                    // Add other properties of MaterialTypeDTO if needed
                }
            }
        })
        .ToList();

    return Results.Ok(checkouts);
});

app.MapGet("/api/checkouts/overdue", (LoncotesLibraryDbContext db) =>
{
    return db.Checkouts
    .Include(p => p.Patron)
    .Include(co => co.Material)
    .ThenInclude(m => m.MaterialType)
    .Where(co =>
        (DateTime.Today - co.CheckoutDate).Days >
        co.Material.MaterialType.CheckoutDays &&
        co.ReturnDate == null)
        .Select(co => new CheckoutWithLateFeeDTO
        {
            Id = co.Id,
            MaterialId = co.MaterialId,
            Material = new MaterialDTO
            {
                Id = co.Material.Id,
                MaterialName = co.Material.MaterialName,
                MaterialTypeId = co.Material.MaterialTypeId,
                MaterialType = new MaterialTypeDTO
                {
                    Id = co.Material.MaterialTypeId,
                    Name = co.Material.MaterialType.Name,
                    CheckoutDays = co.Material.MaterialType.CheckoutDays
                },
                GenreId = co.Material.GenreId,
                OutOfCirculationSince = co.Material.OutOfCirculationSince
            },
            PatronId = co.PatronId,
            Patron = new PatronDTO
            {
                Id = co.Patron.Id,
                FirstName = co.Patron.FirstName,
                LastName = co.Patron.LastName,
                Address = co.Patron.Address,
                Email = co.Patron.Email,
                IsActive = co.Patron.IsActive
            },
            CheckoutDate = co.CheckoutDate,
            ReturnDate = co.ReturnDate,
            Paid = co.Paid
        })
    .ToList();
});

app.MapPut("/api/patrons/{id}/updateCheckoutsPaid", (LoncotesLibraryDbContext db, int id) =>
{
    var patron = db.Patrons
        .Include(p => p.Checkouts)
        .SingleOrDefault(p => p.Id == id);

    if (patron == null)
    {
        return Results.NotFound("Patron not found.");
    }

    foreach (var checkout in patron.Checkouts)
    {
        if (checkout.ReturnDate != null)
        {
            checkout.Paid = true;
        }
    }

    db.SaveChanges();

    return Results.Ok();
});

app.Run();

