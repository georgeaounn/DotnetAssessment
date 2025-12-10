using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class OrderItem
    {
        public Guid Id { get; set; }
        public Guid OrderId { get; set; }
        public Order Order { get; set; } = new Order();
        public Guid ItemId { get; set; }
        public Item Item { get; set; } = new Item();
        public decimal UnitPrice { get; set; }
    }
}
