using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IMovieScheduleRepository
    {
        Task<MovieSchedules> CreateAsync(MovieSchedules model);

        Task<MovieSchedules> UpdateAsync(int id, MovieSchedules model);

        Task<MovieSchedules> DeleteAsync(int id);
        
        Task<MovieSchedules> SoftDeleteAsync(int id);

        Task<MovieSchedules> GetAsync(int id);
        
        Task<IList<MovieSchedules>> GetListAsync();
    }
}
