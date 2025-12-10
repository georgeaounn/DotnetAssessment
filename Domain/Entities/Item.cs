using System.ComponentModel.DataAnnotations;

namespace Domain.Entities
{
    public class Item
    {
        public Guid Id { get; set; }
        [Required, MaxLength(150)]
        public string Name { get; set; } = "";
        [Required]
        public Guid ProductId { get; set; }
        public byte[] RowVersion { get; set; } = Array.Empty<byte>();
        public bool IsSold { get; set; } = false;
        public Product Product { get; set; } = new Product();
    }
}
