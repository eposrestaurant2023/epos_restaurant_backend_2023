using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eAPIClient.Models;
using eAPIClient.Helpers;
using System.Text.Json;

namespace eAPIClient.Services
{
    public interface IUserService
    {
        Task<UserModel> Authenticate(string username, string password);
     
    }
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext db;
        private readonly AppService app;
      
      
        public UserService(ApplicationDbContext _db, AppService _app)
        {
            db = _db;
            app = _app;
        }
       
        public async Task<UserModel> Authenticate(string username, string password)
        {

            var data_user = await app.AllUsers();
            var user = data_user.SingleOrDefault(x => x.username == username && x.password == password);


            // return null if user not found
            if (user == null)
            {
                return null;
            }
            // authentication successful so return user details without password
            return user.WithoutPassword();
        }

        
    }
}
