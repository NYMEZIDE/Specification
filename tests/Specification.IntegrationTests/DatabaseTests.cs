using Microsoft.EntityFrameworkCore;
using Specification.IntegrationTests.Domain;
using Specification.IntegrationTests.Infrastructure;
using Specification.IntegrationTests.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Specification.IntegrationTests
{
    public class DatabaseTests
    {
        public DatabaseTests()
        {
            using var db = new OrdersDbContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            var orderOne = new Order
            {
                Id = 1,
                Amount = 1000,
                DeliveryAmount = 300,
                Status = OrderStatus.accepted,
                Products = new List<Product>
                {
                    new Product
                    {
                        Id = 1,
                        Name = "product 1",
                        OrderId = 1,
                        Price = 100,
                        Quantity = 1
                    },
                    new Product
                    {
                        Id = 2,
                        Name = "product 2",
                        OrderId = 1,
                        Price = 150,
                        Quantity = 2
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "product 3",
                        OrderId = 1,
                        Price = 200,
                        Quantity = 3
                    }
                }
            };

            var orderTwo = new Order
            {
                Id = 2,
                Amount = 1400,
                DeliveryAmount = 0,
                Status = OrderStatus.packed,
                Products = new List<Product>
                {
                    new Product
                    {
                        Id = 2,
                        Name = "product 2",
                        OrderId = 2,
                        Price = 150,
                        Quantity = 2
                    },
                    new Product
                    {
                        Id = 3,
                        Name = "product 3",
                        OrderId = 2,
                        Price = 200,
                        Quantity = 3
                    },
                    new Product
                    {
                        Id = 4,
                        Name = "product 4",
                        OrderId = 2,
                        Price = 500,
                        Quantity = 1
                    }
                }
            };

            db.Orders.AddRange(orderOne, orderTwo);
            db.SaveChanges();
        }

        [Fact]
        [ResetDatabase]
        public void When_NoSpec_Then_TwoOrders()
        {
            using var db = new OrdersDbContext();

            var orders = db.Orders.ToList();

            Assert.Equal(2, orders.Count());
        }

        [Fact]
        [ResetDatabase]
        public void When_Spec_for_FirstOrDefault_Then_OneOrder()
        {
            var spec = new Spec<Order>(x => x.Status == OrderStatus.packed);
            using var db = new OrdersDbContext();
            
            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();

            var order = query.FirstOrDefault();

            Assert.NotNull(order);
            Assert.Equal(2, order.Id);
            Assert.Equal(3, order.Products.Count());

            Assert.Contains("\"Status\" = 'packed'", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_Spec_for_FirstOrDefault_Then_Null()
        {
            var spec = new Spec<Order>(x => x.Status == OrderStatus.completed);
            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();

            var order = query.FirstOrDefault(spec);

            Assert.Null(order);

            Assert.Contains("\"Status\" = 'completed'", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_Spec_for_Where_Then_TwoOrders()
        {
            var spec = new Spec<Order>(x => x.Amount > 500);
            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();
            
            var orders = query.ToList();

            Assert.Equal(2, orders.Count);
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 1).Products.Count());
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 2).Products.Count());

            Assert.Contains("500", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_Spec_AnyProducts_Then_TwoOrders()
        {
            var spec = new Spec<Order>(x => x.Products.Any());
            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();

            var orders = query.ToList();

            Assert.Equal(2, orders.Count);
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 1).Products.Count());
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 2).Products.Count());

            Assert.Contains("WHERE EXISTS", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_Spec_OneProductById_Then_OneOrder()
        {
            var spec = new Spec<Order>(x => x.Products.Any(p => p.Id == 1));
            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();

            var orders = query.ToList();

            Assert.Single(orders);
            Assert.Equal(1, orders[0].Id);
            Assert.Equal(3, orders[0].Products.Count());

            Assert.Contains("\"Id\" = 1", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_TwoSpec_OrderWithProduct_Then_OneOrder()
        {
            var spec1 = new Spec<Order>(x => x.DeliveryAmount == 0);
            var spec2 = new Spec<Order>(x => x.Products.Any(p => p.Price >= 500));

            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec1 & spec2);

            var sql = query.ToSql();

            var orders = query.ToList();

            Assert.Single(orders);
            Assert.Equal(2, orders[0].Id);
            Assert.Equal(3, orders[0].Products.Count());

            Assert.Contains("\"DeliveryAmount\" = '0", sql);
            Assert.Contains("\"Price\", '500", sql);
        }

        [Fact]
        [ResetDatabase]
        public void When_TwoSpec_for_ignoredProperties_Then_OneOrder()
        {
            var spec1 = new Spec<Order>(x => x.FullAmount == 1300);
            var spec2 = new Spec<Order>(x => x.Products.All(p => p.Amount >= 100 && p.Amount <= 1000));

            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec1 & spec2);

            var sql = query.ToSql();

            var orders = query.ToList();

            Assert.Single(orders);
            Assert.Equal(1, orders[0].Id);
            Assert.Equal(3, orders[0].Products.Count());

            Assert.Contains("\"FullAmount\" = '1300", sql);
            Assert.Contains("\"Amount\", '100", sql);
            Assert.Contains("\"Amount\", '1000", sql);
        }
        
        [Fact]
        [ResetDatabase]
        public void When_NotSpec_for_FirstOrDefault_Then_OneOrder()
        {
            var spec = (new Spec<Order>(x => x.Status == OrderStatus.packed)).Not();
            using var db = new OrdersDbContext();
            
            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec);

            var sql = query.ToSql();

            var order = query.FirstOrDefault();

            Assert.NotNull(order);
            Assert.Equal(1, order.Id);
            Assert.Equal(3, order.Products.Count());

            Assert.Matches("NOT .+\"Status\" = 'packed'", sql); 
        }
        
        [Fact]
        [ResetDatabase]
        public void When_TwoSpecs_with_Or_Then_TwoOrders()
        {
            var spec1 = (new Spec<Order>(x => x.Id == 1));
            var spec2 = (new Spec<Order>(x => x.Status == OrderStatus.packed));

            using var db = new OrdersDbContext();

            var query = db.Orders
                .Include(o => o.Products)
                .Where(spec1.Or(spec2));

            var sql = query.ToSql();

            var orders = query.ToList();

            Assert.Equal(2, orders.Count);
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 1).Products.Count());
            Assert.Equal(3, orders.FirstOrDefault(o => o.Id == 2).Products.Count());

            Assert.Matches("\"Id\" = 1.+OR.+\"Status\" = 'packed'", sql); 
        }
    }
}
