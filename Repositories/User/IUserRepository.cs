using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using movie_api.Models;

namespace movie_api.Repositories
{
    public interface IUserRepository
    {
        Task<Users> CreateAsync(Users model);

        Task<Users> UpdateAsync(int id, UsersUpdate model);

        Task<Users> SoftDeleteAsync(int id);

        Task<Users> DeleteAsync(int id);

        Task<Users> GetAsync(int id);

        Task<Users> GetLoginAsync(Login model);
        
        Task<Users> GetActivateAsync(UserActivation model);

        Task<IList<Users>> GetListAsync();
    }
}
