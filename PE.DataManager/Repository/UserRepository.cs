using Microsoft.EntityFrameworkCore;
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
        public User GetLastLoggedInUser()
        {
            using (var context = new SqLiteDbContext())
            {
                if (!context.Users.Any())
                    return null;

                return context.Users.OrderBy(x => x.LastLoginDate).Last();
            }
        }

        public async Task<User> GetUserByAuthId(string authId)
        {
            using (var context = new SqLiteDbContext())
            {
                return await context.Users.SingleOrDefaultAsync(x => x.AuthUserId == authId);
            }
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            using (var context = new SqLiteDbContext())
            {
                return await context.Users.SingleOrDefaultAsync(x => x.EmailAddress == email);
            }
        }

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

        public async Task<bool> UpdateUserAsync(User user)
        {
            using (var context = new SqLiteDbContext())
            {
                var userToUpdate = context.Users.SingleOrDefault(x => x.AuthUserId == user.AuthUserId);
                if (userToUpdate is null)
                {
                    return false;
                }

                userToUpdate.RefreshToken = user.RefreshToken;
                userToUpdate.Token = user.Token;
                userToUpdate.LastLoginDate = user.LastLoginDate;

                context.Users.Update(userToUpdate);
                return await context.SaveChangesAsync() > 0;
            }
        }
    }
}
