﻿using Microsoft.EntityFrameworkCore;
using ProvaPub.Models;
using ProvaPub.Repository;

namespace ProvaPub.Services
{
    public class CustomerService
    {
        private readonly TestDbContext _ctx;
        private const int PageLimit = 10;

        public CustomerService(TestDbContext ctx)
        {
            _ctx = ctx;
        }

        public CustomerList ListCustomers(int page)
        {
            int skipCount = (page - 1) * PageLimit;
            var customers = _ctx.Customers.Skip(skipCount).Take(PageLimit).ToList();

            return new CustomerList
            {
                HasNext = customers.Count == PageLimit,
                TotalCount = _ctx.Customers.Count(),
                Customers = customers
            };
        }


        public async Task<bool> CanPurchase(int? customerId, decimal? purchaseValue)
        {
                if (customerId <= 0) throw new ArgumentOutOfRangeException($"Customer Id {nameof(customerId)} is out of Range");

                if (purchaseValue <= 0) throw new ArgumentOutOfRangeException($"Purchase Value {nameof(purchaseValue)} is out of Range");

                //Business Rule: Non registered Customers cannot purchase
                var customer = await _ctx.Customers.FindAsync(customerId);
                if (customer == null) throw new InvalidOperationException($"Customer Id {customerId} does not exists");

                //Business Rule: A customer can purchase only a single time per month
                var baseDate = DateTime.UtcNow.AddMonths(-1);
                var ordersInThisMonth = await _ctx.Orders.CountAsync(s => s.CustomerId == customerId && s.OrderDate >= baseDate);
                if (ordersInThisMonth > 0)
                    return false;

                //Business Rule: A customer that never bought before can make a first purchase of maximum 100,00
                var haveBoughtBefore = await _ctx.Customers.CountAsync(s => s.Id == customerId && s.Orders.Any());
                if (haveBoughtBefore == 0 && purchaseValue > 100)
                    return false;

                return true;
        }

    }
}
