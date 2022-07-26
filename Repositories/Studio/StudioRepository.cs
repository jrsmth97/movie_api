using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class StudioRepository : IStudioRepository
    {
        private readonly ILogger<IStudioRepository> _logger;
        private readonly DBContext _db;

        public StudioRepository(ILogger<IStudioRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<Studios> CreateAsync(Studios studio)
        {
            studio.created_at = DateTime.Now;
            await _db.studios.AddAsync(studio);
            _db.SaveChanges();
            return studio;
        }

        public async Task<Studios> UpdateAsync(int id, Studios studio)
        {
            Studios existingStudio = await GetAsync(id);

            if (existingStudio != null)
            {
                existingStudio.studio_number = studio.studio_number;
                existingStudio.seat_capacity = studio.seat_capacity;
                existingStudio.updated_at = DateTime.Now;
                _db.studios.Update(existingStudio);
                _db.SaveChanges();
            }

            return existingStudio;
        }

        public async Task<Studios> DeleteAsync(int id)
        {
            Studios studio = await GetAsync(id);
            using (var context = _db)
            {
                context.studios.Remove(studio);
            }

            return studio;
        }

        public async Task<Studios> SoftDeleteAsync(int id)
        {
            var studio = await _db.studios.FindAsync(id); 
            if (studio != null)
            {
                studio.deleted_at = DateTime.Now;
                _db.studios.Update(studio);
                _db.SaveChanges();
            }

            return studio;
        }

        public async Task<Studios> GetAsync(int id)
        {
            Studios studio = await _db.studios.FindAsync(id);
            return studio;
        }

        public async Task<IList<Studios>> GetListAsync()
        {
            return await _db.studios.ToListAsync();
        }

    }
}