using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ILogger<IOrderRepository> _logger;
        private readonly DBContext _db;

        public OrderRepository(ILogger<IOrderRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<Orders> CreateAsync(Orders order)
        {
            order.created_at = DateTime.Now;
            await _db.orders.AddAsync(order);
            _db.SaveChanges();
            return order;
        }

        public async Task<Orders> UpdateAsync(int id, Orders order)
        {
            Orders existingOrder = await GetAsync(id);

            if (existingOrder != null)
            {
                existingOrder.user_id = order.user_id;
                existingOrder.payment_method = order.payment_method;
                existingOrder.total_item_price = order.total_item_price;
                existingOrder.updated_at = DateTime.Now;
                _db.orders.Update(existingOrder);
                _db.SaveChanges();
            }

            return existingOrder;
        }

        public async Task<Orders> DeleteAsync(int id)
        {
            Orders order = await GetAsync(id);
            using (var context = _db)
            {
                context.orders.Remove(order);
                context.SaveChanges();
            }

            return order;
        }

        public async Task<Orders> SoftDeleteAsync(int id)
        {
            var order = await _db.orders.FindAsync(id); 
            if (order != null)
            {
                order.deleted_at = DateTime.Now;
                _db.orders.Update(order);
                _db.SaveChanges();
            }

            return order;
        }

        public async Task<Orders> GetAsync(int id)
        {
            Orders order = await _db.orders.FindAsync(id);
            return order;
        }

        public async Task<IList<Orders>> GetListAsync()
        {
            return await _db.orders.Include(x => x.user).ToListAsync();
        }

    }
}