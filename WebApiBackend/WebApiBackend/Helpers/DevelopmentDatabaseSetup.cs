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
        User yin, teresa, bryan;
        Payment payment1, payment2;
        // Payment[NAME]1 is for electricity Payment[NAME]2 is for Rent due to many to many relationship
        UserPayment userPaymentYin1, userPaymentYin2, userPaymentBryan1,
            userPaymentBryan2, userPaymentTeresa1, userPaymentTeresa2;
        Schedule schedule1;
        Flat flat1;

        private readonly FlatManagementContext _database;

        public void InitialiseTestDataObjects()
        {
            yin = new User
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

            teresa = new User
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

            bryan = new User
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
                User = bryan,
                UserName = bryan.UserName,
                PaymentId = payment1.Id
            };
            userPaymentBryan2 = new UserPayment
            {
                Payment = payment2,
                User = bryan,
                UserName = bryan.UserName,
                PaymentId = payment2.Id
            };

            userPaymentYin1 = new UserPayment
            {
                Payment = payment1,
                User = yin,
                UserName = yin.UserName,
                PaymentId = payment1.Id
            };

            userPaymentYin2 = new UserPayment
            {
                Payment = payment2,
                User = yin,
                UserName = yin.UserName,
                PaymentId = payment2.Id
            };

            userPaymentTeresa1 = new UserPayment
            {
                Payment = payment1,
                User = teresa,
                UserName = teresa.UserName,
                PaymentId = payment1.Id
            };

            userPaymentTeresa2 = new UserPayment
            {
                Payment = payment2,
                User = teresa,
                UserName = teresa.UserName,
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
                Users = new List<User> { yin, teresa, bryan },
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
            // This function could also be called in the unit tests if not called here
            InitialiseTestDataObjects();

            _database.Add(yin);
            _database.Add(teresa);
            _database.Add(bryan);
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
