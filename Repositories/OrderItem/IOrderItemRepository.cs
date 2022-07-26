using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IOrderItemRepository
    {
        Task<OrderItems> CreateAsync(OrderItems model);

        Task<OrderItems> UpdateAsync(int id, OrderItems model);

        Task<OrderItems> DeleteAsync(int id);
        
        Task<OrderItems> SoftDeleteAsync(int id);

        Task<OrderItems> GetAsync(int id);
        
        Task<IList<OrderItems>> GetListAsync();
    }
}
