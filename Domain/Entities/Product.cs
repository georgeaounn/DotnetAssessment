using System.ComponentModel.DataAnnotations;

namespace Domain.Entities;

public class Product
{
    public Guid Id { get; set; }
    [Required, MaxLength(150)]
    public string Name { get; set; } = "";
    [Required, Range(0,double.MaxValue)]
    public decimal BasePrice { get; set; }
    public bool IsActive { get; set; } = true;

}

