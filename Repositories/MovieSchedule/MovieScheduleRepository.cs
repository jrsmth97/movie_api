using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace movie_api.Repositories
{
    public class MovieScheduleRepository : IMovieScheduleRepository
    {
        private readonly ILogger<IMovieScheduleRepository> _logger;
        private readonly DBContext _db;

        public MovieScheduleRepository(ILogger<IMovieScheduleRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<MovieSchedules> CreateAsync(MovieSchedules movieSchedule)
        {
            movieSchedule.created_at = DateTime.Now;
            await _db.movieSchedules.AddAsync(movieSchedule);
            _db.SaveChanges();
            return movieSchedule;
        }

        public async Task<MovieSchedules> UpdateAsync(int id, MovieSchedules movieSchedule)
        {
            MovieSchedules existingMovieSchedule = await GetAsync(id);

            if (existingMovieSchedule != null)
            {
                existingMovieSchedule.movie_id = movieSchedule.movie_id;
                existingMovieSchedule.studio_id = movieSchedule.studio_id;
                existingMovieSchedule.start_time = movieSchedule.start_time;
                existingMovieSchedule.end_time = movieSchedule.end_time;
                existingMovieSchedule.price = movieSchedule.price;
                existingMovieSchedule.date = movieSchedule.date;
                existingMovieSchedule.updated_at = DateTime.Now;
                _db.movieSchedules.Update(existingMovieSchedule);
                _db.SaveChanges();
            }

            return existingMovieSchedule;
        }

        public async Task<MovieSchedules> DeleteAsync(int id)
        {
            MovieSchedules movieSchedule = await GetAsync(id);
            using (var context = _db)
            {
                context.movieSchedules.Remove(movieSchedule);
                context.SaveChanges();
            }

            return movieSchedule;
        }

        public async Task<MovieSchedules> SoftDeleteAsync(int id)
        {
            var movieSchedule = await _db.movieSchedules.FindAsync(id); 
            if (movieSchedule != null)
            {
                movieSchedule.deleted_at = DateTime.Now;
                _db.movieSchedules.Update(movieSchedule);
                _db.SaveChanges();
            }

            return movieSchedule;
        }

        public async Task<MovieSchedules> GetAsync(int id)
        {
            MovieSchedules movieSchedule = await _db.movieSchedules.FindAsync(id);
            return movieSchedule;
        }

        public async Task<IList<MovieSchedules>> GetListAsync()
        {
            return await _db.movieSchedules.Include(x => x.movie).Include(y => y.studio).ToListAsync();
        }

    }
}
    