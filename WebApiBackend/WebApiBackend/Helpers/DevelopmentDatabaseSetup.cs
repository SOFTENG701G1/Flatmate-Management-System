using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApiBackend.Model;

namespace WebApiBackend.Helpers
{
    public class DevelopmentDatabaseSetup
    {
        User user1, user2, user3;
        Payment payment1, payment2;
        // Payment[NAME]1 is for electricity Payment[NAME]2 is for Rent due to many to many relationship
        UserPayment userPaymentYin1, userPaymentYin2, userPaymentBryan1,
            userPaymentBryan2, userPaymentTeresa1, userPaymentTeresa2;
        Schedule schedule1;
        Flat flat1;

        private readonly FlatManagementContext _database;

        public void InitialiseTestDataObjects()
        {
            user1 = new User
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

            user2 = new User
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

            user3 = new User
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

            payment1 = new Payment
            {
                Id = 1,
                PaymentType = PaymentType.Electricity,
                Amount = 175,
                Fixed = false,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 07),
                EndDate = new DateTime(2020, 06, 07),
            };
            payment2 = new Payment
            {
                Id = 2,
                PaymentType = PaymentType.Rent,
                Amount = 1000,
                Fixed = true,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 01),
                EndDate = new DateTime(2020, 10, 01),
            };
            userPaymentBryan1 = new UserPayment
            {
                Payment = payment1,
                User = user1,
                UserName = user1.UserName,
                PaymentId = payment1.Id
            };
            userPaymentBryan2 = new UserPayment
            {
                Payment = payment2,
                User = user3,
                UserName = user3.UserName,
                PaymentId = payment2.Id
            };

            userPaymentYin1 = new UserPayment
            {
                Payment = payment1,
                User = user3,
                UserName = user3.UserName,
                PaymentId = payment1.Id
            };

            userPaymentYin2 = new UserPayment
            {
                Payment = payment2,
                User = user1,
                UserName = user1.UserName,
                PaymentId = payment2.Id
            };

            userPaymentTeresa1 = new UserPayment
            {
                Payment = payment1,
                User = user2,
                UserName = user2.UserName,
                PaymentId = payment1.Id
            };

            userPaymentTeresa2 = new UserPayment
            {
                Payment = payment2,
                User = user2,
                UserName = user2.UserName,
                PaymentId = payment2.Id
            };
            payment1.UserPayments = new List<UserPayment> { userPaymentBryan1, userPaymentTeresa1, userPaymentYin1 };
            payment2.UserPayments = new List<UserPayment> { userPaymentBryan2, userPaymentTeresa2, userPaymentYin2 };

            schedule1 = new Schedule
            {
                UserName = "BeboBryan",
                ScheduleType = ScheduleType.Away,
                StartDate = new DateTime(2020, 04, 01),
                EndDate = new DateTime(2020, 05, 01)
            };

            flat1 = new Flat
            {
                Address = "50 Symonds Street",
                Users = new List<User> { user1, user2, user3 },
                Schedules = new List<Schedule> { schedule1 },
                Payments = new List<Payment> { payment1, payment2 }
            };

        }

        public DevelopmentDatabaseSetup(FlatManagementContext database)
        {
            _database = database;
        }

        public void SetupDevelopmentDataSet()
        {
            _database.Add(user1);
            _database.Add(user2);
            _database.Add(user3);
            _database.Add(payment1);
            _database.Add(payment2);
            _database.Add(userPaymentBryan1);
            _database.Add(userPaymentBryan2);
            _database.Add(userPaymentTeresa1);
            _database.Add(userPaymentTeresa2);
            _database.Add(userPaymentYin1);
            _database.Add(userPaymentYin2);
            _database.Add(schedule1);
            _database.Add(flat1);

            _database.SaveChanges();
        }
    }
}
