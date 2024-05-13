using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace Library;

public class MaterialType
{
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int CheckoutDays { get; set; }
}
