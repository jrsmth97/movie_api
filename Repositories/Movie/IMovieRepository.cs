using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IMovieRepository
    {
        Task<Movies> CreateAsync(Movies model);

        Task<Movies> UpdateAsync(int id, Movies model);

        Task<Movies> DeleteAsync(int id);
        
        Task<Movies> SoftDeleteAsync(int id);

        Task<Movies> GetAsync(int id);
        
        Task<IList<Movies>> GetListAsync(QueryParamsMovies queryParams);
    }
}
