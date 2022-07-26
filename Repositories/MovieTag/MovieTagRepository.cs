using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class MovieTagRepository : IMovieTagRepository
    {
        private readonly ILogger<IMovieTagRepository> _logger;
        private readonly DBContext _db;

        public MovieTagRepository(ILogger<IMovieTagRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<MovieTags> CreateAsync(MovieTags movieTag)
        {
            movieTag.created_at = DateTime.Now;
            await _db.movieTags.AddAsync(movieTag);
            _db.SaveChanges();
            return movieTag;
        }

        public async Task<MovieTags> UpdateAsync(int id, MovieTags movieTag)
        {
            MovieTags existingMovieTag = await GetAsync(id);

            if (existingMovieTag != null)
            {
                existingMovieTag.movie_id = movieTag.movie_id;
                existingMovieTag.tag_id = movieTag.tag_id;
                existingMovieTag.updated_at = DateTime.Now;
                _db.movieTags.Update(existingMovieTag);
                _db.SaveChanges();
            }

            return existingMovieTag;
        }

        public async Task<MovieTags> DeleteAsync(int id)
        {
            MovieTags movieTag = await GetAsync(id);
            using (var context = _db)
            {
                context.movieTags.Remove(movieTag);
            }

            return movieTag;
        }

        public async Task<MovieTags> SoftDeleteAsync(int id)
        {
            var movieTag = await _db.movieTags.FindAsync(id); 
            if (movieTag != null)
            {
                movieTag.deleted_at = DateTime.Now;
                _db.movieTags.Update(movieTag);
                _db.SaveChanges();
            }

            return movieTag;
        }

        public async Task<MovieTags> GetAsync(int id)
        {
            MovieTags movieTag = await _db.movieTags.Include(x => x.tag).Include(y => y.movie).FirstOrDefaultAsync(z => z.id == id);
            return movieTag;
        }

        public async Task<IList<MovieTags>> GetListAsync()
        {
            return await _db.movieTags.Include(x => x.tag).Include(y => y.movie).ToListAsync();
        }

    }
}