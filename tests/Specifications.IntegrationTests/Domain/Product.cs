using System;
using System.Collections.Generic;
using System.Text;

namespace Specifications.IntegrationTests.Domain
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public decimal Amount { get => Price * Quantity; protected set { } }

        public int OrderId { get; set; }
    }
}
