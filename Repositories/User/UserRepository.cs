using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Contexts;
using movie_api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace movie_api.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ILogger<IUserRepository> _logger;
        private readonly DBContext _db;

        public UserRepository(ILogger<IUserRepository> logger, DBContext db)
        {
            _logger = logger;
            _db     = db;
        }

        public async Task<Users> CreateAsync(Users users)
        {
            Users createUser = new Users{
                name = users.name,
                email = users.email,
                password = users.password,
                avatar = users.avatar,
                activation_key = users.activation_key,
                is_admin = users.is_admin
            };

            createUser.created_at = DateTime.Now;
            await _db.users.AddAsync(createUser);
            _db.SaveChanges();
            return users;
        }

        public async Task<Users> UpdateAsync(int id, UsersUpdate user)
        {
            var existingUser = await GetAsync(id);

            if (existingUser != null)
            {
                existingUser.name = user.name != null ? user.name : existingUser.name;
                existingUser.email = user.email != null ? user.email : existingUser.email;
                existingUser.avatar = user.avatar != null ? user.avatar : existingUser.avatar;
                existingUser.password = user.password != null ? user.password : existingUser.password;
                existingUser.updated_at = DateTime.Now;
                _db.users.Update(existingUser);
                _db.SaveChanges();
            }

            return existingUser;
        }

        public async Task<Users> DeleteAsync(int id)
        {
            Users user = await GetAsync(id);
            using (var context = _db)
            {
                context.users.Remove(user);
                context.SaveChanges();
            }

            return user;
        }

        public async Task<Users> SoftDeleteAsync(int id)
        {
            var user = await _db.users.FindAsync(id); 
            if (user != null)
            {
                user.deleted_at = DateTime.Now;
                _db.users.Update(user);
                _db.SaveChanges();
            }

            return user;
        }

        public async Task<Users> GetLoginAsync(Login login) 
        {
            Users user = await _db.users.SingleOrDefaultAsync(user => user.email.Equals(login.email));
            return user;
        }
        public async Task<Users> GetActivateAsync(UserActivation activation) 
        {
            Users user = await _db.users.SingleOrDefaultAsync(user => user.email.Equals(activation.email) && user.activation_key.Equals(activation.activation_key));

            if (user != null)
            {
                user.is_confirmed = 1;
                _db.users.Update(user);
                _db.SaveChanges();
            }

            return user;
        }

        public async Task<Users> GetAsync(int id)
        {
            Users user = await _db.users.FindAsync(id);
            return user;
        }

        public async Task<IList<Users>> GetListAsync()
        {
            return await _db.users.ToListAsync();
        }

    }
}