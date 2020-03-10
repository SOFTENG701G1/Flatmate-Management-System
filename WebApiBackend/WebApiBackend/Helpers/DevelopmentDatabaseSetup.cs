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
        public void initialiseUsers()
        {

        }

        public void SetupDevelopmentDataSet()
        {
            User user1 = new User
            {
                UserName = "bob101",
                FirstName = "Bob",
                LastName = "Sun",
                DateOfBirth = new DateTime(1998, 05, 16),
                PhoneNumber = "0218934932",
                Email = "bobsun@gmail.com",
                MedicalInformation = "N/A",
                Password = "bobsthebest303"
            };

            User user2 = new User
            {
                UserName = "YinWang",
                FirstName = "Yin",
                LastName = "Wang",
                DateOfBirth = new DateTime(1994, 12, 23),
                PhoneNumber = "0279284492",
                Email = "YinWang@qq.com",
                MedicalInformation = "N/A",
                Password = "MustacheMan22"
            };

            User user3 = new User
            {
                UserName = "TreesAreGreen",
                FirstName = "Teresa",
                LastName = "Green",
                DateOfBirth = new DateTime(1996, 02, 12),
                PhoneNumber = "0228937228",
                Email = "GreenTrees@Yahoo.com",
                MedicalInformation = "Vegan, Gluten-Free, Lactose Intolerant",
                Password = "TreestheBest"
            };

            User user4 = new User
            {
                UserName = "ALargeSalmon",
                FirstName = "Clayton",
                LastName = "Lan",
                DateOfBirth = new DateTime(1984, 02, 09),
                PhoneNumber = "0239876228",
                Email = "clyanine@hotmail.com",
                MedicalInformation = "Dementia",
                Password = "Sickgainz123"
            };

            User user5 = new User
            {
                UserName = "BeboBryan",
                FirstName = "Bryan",
                LastName = "Ang",
                DateOfBirth = new DateTime(1984, 02, 09),
                PhoneNumber = "02243926392",
                Email = "ZianYangAng@Gmail.com",
                MedicalInformation = "COVID-19, Extra Chromosome",
                Password = "StolenGirlfriend123"
            };
            Payment payment1 = new Payment
            {
                PaymentType = PaymentType.Electricity,
                Amount = 175,
                Fixed = false,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 07),
                EndDate = new DateTime(2020, 06, 07),
                Users = new List<User> { user1, user2, user3, user4, user5 },
            };
            Payment payment2 = new Payment
            {
                PaymentType = PaymentType.Rent,
                Amount = 1000,
                Fixed = true,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 01),
                EndDate = new DateTime(2020, 10, 01),
                Users = new List<User> { user1, user2, user3, user4, user5 },
            };

            Schedule schedule1 = new Schedule
            {
                UserName = "BeboBryan",
                ScheduleType = ScheduleType.Away,
                StartDate = new DateTime(2020, 04, 01),
                EndDate = new DateTime(2020, 05, 01)
            };


            Flat flat1 = new Flat
            {
                Address = "50 Symonds Street",
                Users = new List<User> { user1, user2, user3, user4, user5 },
                Schedules = new List<Schedule> { schedule1 },
                Payments = new List<Payment> { payment1, payment2 }
            };

            _database.Add(
                user1
                );
            _database.Add(
                user2
                );
            _database.Add(
                user3
                );
            _database.Add(
                user4
                );
            _database.Add(
                user5
                );
            _database.Add(
                flat1
                );
            _database.Add(
                payment1
                );
            _database.Add(
                payment2
                );
            _database.Add(
                schedule1
                );

            _database.SaveChanges();
        }
    }
}
