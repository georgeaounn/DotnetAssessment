using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Order
    {
        public Guid Id { get; set; }
        [Required]
        public Guid CustomerId { get; set; }
        public User Customer { get; set; } = new User();
        public DateTime CreatedAt { get; set; }
        [Required, Range(0, double.MaxValue)]
        public decimal TotalPrice { get; set; }

        private readonly List<OrderItem> _items = new();
        public IEnumerable<OrderItem> Items => _items;

        public void AddItem(Guid itemId, decimal unitPrice)
        {
            _items.Add(new OrderItem() { Id = Id, ItemId = itemId, UnitPrice =  unitPrice });
            RecalculateTotal();
        }

        private void RecalculateTotal()
        {
            TotalPrice = _items.Sum(i => i.UnitPrice);
        }

    }
}
