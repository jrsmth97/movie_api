using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface ITagRepository
    {
        Task<Tags> CreateAsync(Tags model);

        Task<Tags> UpdateAsync(int id, Tags model);

        Task<Tags> DeleteAsync(int id);
        
        Task<Tags> SoftDeleteAsync(int id);

        Task<Tags> GetAsync(int id);
        
        Task<IList<Tags>> GetListAsync();
    }
}
