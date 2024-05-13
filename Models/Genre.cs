using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;


namespace Library;

public class Genre
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    
}

