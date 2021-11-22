using System;
using System.Collections.Generic;
using System.Text;

namespace Specification.IntegrationTests.Domain
{
    public class Order
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public ICollection<Product> Products { get; set; }

        public decimal Amount { get; set; }

        public decimal DeliveryAmount { get; set; }

        public decimal FullAmount { get => Amount + DeliveryAmount; protected set { } }
    }
}
