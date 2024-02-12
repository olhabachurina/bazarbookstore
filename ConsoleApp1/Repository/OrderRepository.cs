﻿using ConsoleApp1.Data;
using ConsoleApp1.Interfaces;
using ConsoleApp1.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Repository
{
    public class OrderRepository : IOrder
    {
        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders.Include(e => e.Lines).ThenInclude(e => e.Book).ToListAsync();
            }
        }

        public async Task<Order> GetOrderAsync(int id)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders.Include(e => e.Lines).FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task AddOrderAsync(Order order)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                context.Orders.Add(order);
                await context.SaveChangesAsync();
            }
        }

        public async Task DeleteOrderAsync(Order order)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                context.Orders.Remove(order);
                await context.SaveChangesAsync();
            }
        }

        public async Task UpdateOrderAsync(Order order)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                var currentOrder = await context.Orders.Include(e => e.Lines).FirstOrDefaultAsync(e => e.Id == order.Id);
                if (currentOrder is not null)
                {
                    currentOrder.CustomerName = order.CustomerName;
                    currentOrder.Address = order.Address;
                    currentOrder.City = order.City;
                    currentOrder.Shipped = order.Shipped;
                    currentOrder.Lines = new List<OrderLine>();
                    foreach (OrderLine line in order.Lines)
                    {
                        currentOrder.Lines.Add(line);
                    }
                    context.Orders.Update(currentOrder);
                    await context.SaveChangesAsync();
                }
            }
        }

        public async Task<Order> GetOrderWithOrderLinesAndBooksAsync(int id)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders.Include(e => e.Lines).ThenInclude(e => e.Book).ThenInclude(e => e.Promotion).FirstOrDefaultAsync(e => e.Id == id);
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByNameAsync(string name)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders.Where(e => e.CustomerName.Contains(name)).ToListAsync();
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersByAddressAsync(string address)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders.Where(e => e.Address.Contains(address)).ToListAsync();
            }
        }
        public async Task<IEnumerable<object>> SearchOrdersAsync(string searchCriteria)
        {
            using (ApplicationContext context = Program.DbContext())
            {
                return await context.Orders
                    .Where(o => o.CustomerName.Contains(searchCriteria) || o.Address.Contains(searchCriteria))
                    .Select(o => new
                    {
                        o.Id,
                        o.CustomerName,
                        o.Address,
                        o.City,
                        o.Shipped
                    })
                    .ToListAsync();
            }
        }

    }
}
    
