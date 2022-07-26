using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IOrderRepository
    {
        Task<Orders> CreateAsync(Orders model);

        Task<Orders> UpdateAsync(int id, Orders model);

        Task<Orders> DeleteAsync(int id);
        
        Task<Orders> SoftDeleteAsync(int id);

        Task<Orders> GetAsync(int id);
        
        Task<IList<Orders>> GetListAsync();
    }
}
