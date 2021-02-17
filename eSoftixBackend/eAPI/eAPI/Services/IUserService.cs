using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eModels;
using eAPI.Helpers;


namespace eAPI.Services
{
    public interface IUserService
    {
        Task<UserModel> Authenticate(string username, string password);
        Task<IEnumerable<UserModel>> GetAll();
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;
        private List<UserModel> _users = new List<UserModel>();
        public UserService(ApplicationDbContext _db)
        {
            db = _db;
            _users = db.Users.ToList();
        }
        // users hardcoded for simplicity, store in a db with hashed passwords in production applications       

        public async Task<UserModel> Authenticate(string username, string password)
        {
            var user = await Task.Run(() => _users.SingleOrDefault(x => x.username == username && x.password == password));

            // return null if user not found
            if (user == null)
            {
                return null;
            }
            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        public async Task<IEnumerable<UserModel>> GetAll()
        {
            return await Task.Run(() => _users.WithoutPasswords());
        }
    }
}
