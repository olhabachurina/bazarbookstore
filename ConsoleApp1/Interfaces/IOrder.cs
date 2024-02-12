using ConsoleApp1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Interfaces
{
    public interface IOrder
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<IEnumerable<Order>> GetAllOrdersByNameAsync(string name);
        Task<IEnumerable<Order>> GetAllOrdersByAddressAsync(string address);
        Task<Order> GetOrderAsync(int id);
        Task<Order> GetOrderWithOrderLinesAndBooksAsync(int id);

        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task DeleteOrderAsync(Order order);
        Task<IEnumerable<object>> SearchOrdersAsync(string searchCriteria);
    }

}
