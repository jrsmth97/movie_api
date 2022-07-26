using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IStudioRepository
    {
        Task<Studios> CreateAsync(Studios model);

        Task<Studios> UpdateAsync(int id, Studios model);

        Task<Studios> DeleteAsync(int id);
        
        Task<Studios> SoftDeleteAsync(int id);

        Task<Studios> GetAsync(int id);
        
        Task<IList<Studios>> GetListAsync();
    }
}
