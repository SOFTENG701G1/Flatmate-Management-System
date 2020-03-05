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
            _database.Add(new TestModelItem
            {
                Name = "A Test Item"
            });

            _database.SaveChanges();
        }
    }
}
