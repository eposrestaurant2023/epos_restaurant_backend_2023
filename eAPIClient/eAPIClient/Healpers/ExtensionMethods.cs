using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using eAPIClient.Models;

namespace eAPIClient.Helpers
{
    public static class ExtensionMethods
    {
        public static IEnumerable<UserModel> WithoutPasswords(this IEnumerable<UserModel> users)
        {
            return users.Select(x => x.WithoutPassword());
        }

        public static UserModel WithoutPassword(this UserModel user)
        {
            user.password = null;
            return user;
        }
    }
}
