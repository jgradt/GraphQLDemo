using FizzWare.NBuilder;
using System;
using System.Collections.Generic;
using System.Linq;
using GraphQLDemo.Data.Entities;
using System.Diagnostics;

namespace GraphQLDemo.Data
{
    public static class DatabaseInitializer
    {

        public static void AddDatabaseSeedData(DemoDbContext dbContext)
        {
            // see: https://www.jerriepelser.com/blog/creating-test-data-with-nbuilder-and-faker/


            // customers
            List<Customer> customerList = new List<Customer>();

            if (!dbContext.Customers.Any())
            {
                var customers = Builder<Data.Entities.Customer>.CreateListOfSize(100)
                    .All()
                        .With(c => c.FirstName = Faker.Name.First())
                        .With(c => c.LastName = Faker.Name.Last())
                        .With(c => c.Email = $"{c.FirstName.ToLower()[0]}.{c.LastName.ToLower()}@{Faker.Internet.DomainName()}")
                    .Build();

                customerList = customers.ToList();

                dbContext.Customers.AddRange(customerList);
            }


            // suppliers
            List<Supplier> supplierList = new List<Supplier>();

            if (!dbContext.Suppliers.Any())
            {
                var jobTitles = new[] { "Worker", "Manager", "Director", "Vice President", "CEO" };

                var suppliers = Builder<Data.Entities.Supplier>.CreateListOfSize(20)
                    .All()
                        .With(c => c.CompanyName = Faker.Company.Name())
                        .With(c => c.ContactName = $"{Faker.Name.First()} {Faker.Name.Last()}")
                        .With(c => c.ContactTitle = Pick<string>.RandomItemFrom(jobTitles))
                        .With(c => c.StreetAddress = Faker.Address.StreetAddress())
                        .With(c => c.City = Faker.Address.City())
                        .With(c => c.State = Faker.Address.UsStateAbbr())
                        .With(c => c.PostalCode = Faker.Address.ZipCode())
                        .With(c => c.Phone = Faker.Phone.Number())
                    .Build();

                supplierList = suppliers.ToList();

                dbContext.Suppliers.AddRange(supplierList);
            }

            // orders
            List<Order> orderList = new List<Order>();
            var dayGenerator = new RandomGenerator();
            var totalGenerator = new RandomGenerator();
            var phraseGenerator = new RandomGenerator();

            if (!dbContext.Orders.Any())
            {
                var orders = Builder<Data.Entities.Order>.CreateListOfSize(500)
                    .All()
                        .With(o => o.Customer = Pick<Customer>.RandomItemFrom(customerList))
                        .With(o => o.OrderDate = DateTime.Today.AddDays(-dayGenerator.Next(0, 60)))
                        .With(o => o.DeliveredDate = o.OrderDate.AddDays(dayGenerator.Next(1,8)))
                        .With(o => o.TotalDue = totalGenerator.Next(0.01m, 500.00m))
                        .With(o => o.Comment = phraseGenerator.Phrase(30))
                    .Build();

                orderList = orders.ToList();

                dbContext.Orders.AddRange(orderList);
            }

            // products
            List<Product> productList = new List<Product>();
            var unitPriceGenerator = new RandomGenerator();

            if (!dbContext.Products.Any())
            {

                var products = Builder<Data.Entities.Product>.CreateListOfSize(100)
                    .All()
                        .With(o => o.Supplier = Pick<Supplier>.RandomItemFrom(supplierList))
                        .With(o => o.ProductName = string.Join(" ", Faker.Lorem.Words(2)))
                        .With(o => o.UnitPrice = unitPriceGenerator.Next(1.00m, 50.00m))
                        .With(o => o.Discontinued = false)
                    .Random(15)
                        .With(o => o.Discontinued = true)
                    .Build();

                productList = products.ToList();

                dbContext.Products.AddRange(productList);
            }

            // order details
            if (!dbContext.OrderDetails.Any())
            {
                var quantityGenerator = new RandomGenerator();

                var orderDetails = Builder<Data.Entities.OrderDetail>.CreateListOfSize(2000)
                    .All()
                        .With(o => o.Product = Pick<Product>.RandomItemFrom(productList))
                        //.With(o => o.Order = Pick<Order>.RandomItemFrom(orderList))
                        .With(o => o.Quantity = quantityGenerator.Next(1, 10))
                    .Build();

                var orderDetailsList = orderDetails.ToList();
                var unassignedOrderDetails = new List<OrderDetail>(orderDetailsList);

                foreach (var order in orderList)
                {
                    var detail = Pick<OrderDetail>.RandomItemFrom(orderDetailsList);
                    order.Details = new List<OrderDetail>();
                    order.Details.Add(detail);
                    unassignedOrderDetails.Remove(detail);
                }

                foreach (var od in unassignedOrderDetails)
                {
                    var order = Pick<Order>.RandomItemFrom(orderList);
                    order.Details.Add(od);
                }

                foreach(var od in orderDetailsList)
                {
                    od.UnitPrice = od.Product.UnitPrice;
                    od.TotalPrice = od.Product.UnitPrice * od.Quantity;
                }

                foreach (var order in orderList)
                {
                    order.TotalDue = order.Details.Sum(od => od.TotalPrice);
                }

                Debug.Assert(orderList.All(o => o.Details?.Count() > 0));

                dbContext.OrderDetails.AddRange(orderDetailsList);
            }

            dbContext.SaveChanges();
        }
    }
}
