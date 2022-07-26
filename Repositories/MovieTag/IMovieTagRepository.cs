using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IMovieTagRepository
    {
        Task<MovieTags> CreateAsync(MovieTags model);

        Task<MovieTags> UpdateAsync(int id, MovieTags model);

        Task<MovieTags> DeleteAsync(int id);
        
        Task<MovieTags> SoftDeleteAsync(int id);

        Task<MovieTags> GetAsync(int id);
        
        Task<IList<MovieTags>> GetListAsync();
    }
}
