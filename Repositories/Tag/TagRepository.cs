using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class TagRepository : ITagRepository
    {
        private readonly ILogger<ITagRepository> _logger;
        private readonly DBContext _db;

        public TagRepository(ILogger<ITagRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<Tags> CreateAsync(Tags tag)
        {
            tag.created_at = DateTime.Now;
            await _db.tags.AddAsync(tag);
            _db.SaveChanges();
            return tag;
        }

        public async Task<Tags> UpdateAsync(int id, Tags tag)
        {
            Tags existingTag = await GetAsync(id);

            if (existingTag != null)
            {
                existingTag.name = tag.name;
                existingTag.updated_at = DateTime.Now;
                _db.tags.Update(existingTag);
                _db.SaveChanges();
            }

            return existingTag;
        }

        public async Task<Tags> DeleteAsync(int id)
        {
            Tags tag = await GetAsync(id);
            using (var context = _db)
            {
                context.tags.Remove(tag);
                context.SaveChanges();
            }

            return tag;
        }

        public async Task<Tags> SoftDeleteAsync(int id)
        {
            var tag = await _db.tags.FindAsync(id); 
            if (tag != null)
            {
                tag.deleted_at = DateTime.Now;
                _db.tags.Update(tag);
                _db.SaveChanges();
            }

            return tag;
        }

        public async Task<Tags> GetAsync(int id)
        {
            Tags tag = await _db.tags.FindAsync(id);
            return tag;
        }

        public async Task<IList<Tags>> GetListAsync()
        {
            return await _db.tags.ToListAsync();
        }

    }
}