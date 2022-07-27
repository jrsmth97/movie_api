using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly ILogger<IMovieRepository> _logger;
        private readonly DBContext _db;

        public MovieRepository(ILogger<IMovieRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<Movies> CreateAsync(Movies movie)
        {
            movie.created_at = DateTime.Now;
            await _db.movies.AddAsync(movie);
            _db.SaveChanges();
            return movie;
        }

        public async Task<Movies> UpdateAsync(int id, Movies movie)
        {
            Movies existingMovie = await GetAsync(id);

            if (existingMovie != null)
            {
                existingMovie.title = movie.title;
                existingMovie.overview = movie.overview;
                existingMovie.poster = movie.poster;
                existingMovie.play_until = movie.play_until;
                existingMovie.updated_at = DateTime.Now;
                _db.movies.Update(existingMovie);
                _db.SaveChanges();
            }

            return existingMovie;
        }

        public async Task<Movies> DeleteAsync(int id)
        {
            Movies movie = await GetAsync(id);
            using (var context = _db)
            {
                context.movies.Remove(movie);
            }

            return movie;
        }

        public async Task<Movies> SoftDeleteAsync(int id)
        {
            var movie = await _db.movies.FindAsync(id); 
            if (movie != null)
            {
                movie.deleted_at = DateTime.Now;
                _db.movies.Update(movie);
                _db.SaveChanges();
            }

            return movie;
        }

        public async Task<Movies> GetAsync(int id)
        {
            Movies movie = await _db.movies.FindAsync(id);
            return movie;
        }

        public async Task<IList<Movies>> GetListAsync(QueryParamsMovies queryParams)
        {
            int offset = (queryParams.page - 1) * 10;

            return await _db.movies.Where(x => x.title.Contains(queryParams.title))
                                    .Skip(offset)
                                    .Take(queryParams.limit)
                                    .ToListAsync();
        }

        public async Task<int> GetCountListAsync(QueryParamsMovies queryParams)
        {
            int offset = (queryParams.page - 1) * 10;

            return await _db.movies.Where(x => x.title.Contains(queryParams.title))
                                    .Skip(offset)
                                    .Take(queryParams.limit)
                                    .CountAsync();
        }

    }
}