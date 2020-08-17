using Microsoft.EntityFrameworkCore.Internal;
using PE.DataManager.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PE.DataManager.Repository
{
    public class UserRepository : IUserRepository
    {
        public async Task<bool> AddUserAsync(User user)
        {
            using (var context = new SqLiteDbContext())
            {
                if (!context.Users.Any(x => x.Id == user.Id))
                {
                    await context.Users.AddAsync(user);
                    return await context.SaveChangesAsync() > 0;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
