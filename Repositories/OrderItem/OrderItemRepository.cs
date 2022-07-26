using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace movie_api.Repositories
{
    public class OrderItemRepository : IOrderItemRepository
    {
        private readonly ILogger<IOrderItemRepository> _logger;
        private readonly DBContext _db;

        public OrderItemRepository(ILogger<IOrderItemRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<OrderItems> CreateAsync(OrderItems orderItem)
        {
            orderItem.created_at = DateTime.Now;
            await _db.orderItems.AddAsync(orderItem);
            _db.SaveChanges();
            return orderItem;
        }

        public async Task<OrderItems> UpdateAsync(int id, OrderItems orderItem)
        {
            OrderItems existingOrderItem = await GetAsync(id);

            if (existingOrderItem != null)
            {
                existingOrderItem.order_id = orderItem.order_id;
                existingOrderItem.movie_schedule_id = orderItem.movie_schedule_id;
                existingOrderItem.qty = orderItem.qty;
                existingOrderItem.price = orderItem.price;
                existingOrderItem.sub_total_price = orderItem.sub_total_price;
                existingOrderItem.updated_at = DateTime.Now;
                _db.orderItems.Update(existingOrderItem);
                _db.SaveChanges();
            }

            return existingOrderItem;
        }

        public async Task<OrderItems> DeleteAsync(int id)
        {
            OrderItems orderItem = await GetAsync(id);
            using (var context = _db)
            {
                context.orderItems.Remove(orderItem);
            }

            return orderItem;
        }

        public async Task<OrderItems> SoftDeleteAsync(int id)
        {
            var orderItem = await _db.orderItems.FindAsync(id); 
            if (orderItem != null)
            {
                orderItem.deleted_at = DateTime.Now;
                _db.orderItems.Update(orderItem);
                _db.SaveChanges();
            }

            return orderItem;
        }

        public async Task<OrderItems> GetAsync(int id)
        {
            OrderItems orderItem = await _db.orderItems.FindAsync(id);
            return orderItem;
        }

        public async Task<IList<OrderItems>> GetListAsync()
        {
            return await _db.orderItems.Include(x => x.order).Include(y => y.movieSchedule).ToListAsync();
        }

    }
}