using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Model;

namespace WebApiBackend.Helpers
{
    public class DevelopmentDatabaseSetup
    {
        private readonly FlatManagementContext _database;

        public DevelopmentDatabaseSetup(FlatManagementContext database)
        {
            _database = database;
        }

        public void SetupDevelopmentDataSet()
        {
            _database.TestItems.Add(new TestModelItem
            {
                Name = "A Test Item"
            });

            AddTestUsers();

            _database.SaveChanges();
        }

        private void AddTestUsers()
        {
            var hasher = new PasswordHasher<User>();

            var user = new User
            {
                UserName = "user",
                Email = "email@email.com"
            };

            var hashedPassword = hasher.HashPassword(user, "password");

            user.Password = hashedPassword;

            _database.User.Add(user);
        }
    }
}
