using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using WebApiBackend.Model;

namespace WebApiBackend.Helpers
{
    public class DevelopmentDatabaseSetup
    {
        User _yin, _teresa, _bryan;
        Payment _payment1, _payment2;
        // Payment[NAME]1 is for electricity
        // Payment[NAME]2 is for Rent due to many to many relationship
        UserPayment _userPaymentYin1, _userPaymentYin2, _userPaymentBryan1,
            _userPaymentBryan2, _userPaymentTeresa1, _userPaymentTeresa2;
        Schedule _schedule1;
        Flat _flat1;

        private readonly FlatManagementContext _database;

        public void InitialiseTestDataObjects()
        {
            var hasher = new PasswordHasher<User>();

            _yin = new User
            {
                UserName = "YinWang",
                FirstName = "Yin",
                LastName = "Wang",
                DateOfBirth = new DateTime(1994, 12, 23),
                PhoneNumber = "0279284492",
                Email = "YinWang@qq.com",
                MedicalInformation = "N/A",
                BankAccount = "00-0000-0000000-000"
            };
            _yin.HashedPassword = hasher.HashPassword(_yin, "password");

            _teresa = new User
            {
                UserName = "TreesAreGreen",
                FirstName = "Teresa",
                LastName = "Green",
                DateOfBirth = new DateTime(1996, 02, 12),
                PhoneNumber = "0228937228",
                Email = "GreenTrees@Yahoo.com",
                MedicalInformation = "Vegan, Gluten-Free, Lactose Intolerant",
                BankAccount = "12-3456-1234567-123"
            };
            _teresa.HashedPassword = hasher.HashPassword(_teresa, "password");

            _bryan = new User
            {
                UserName = "BeboBryan",
                FirstName = "Bryan",
                LastName = "Ang",
                DateOfBirth = new DateTime(1984, 02, 09),
                PhoneNumber = "02243926392",
                Email = "BryanAng@Gmail.com",
                MedicalInformation = "N/A",
                BankAccount = "98-7654-3211234-210"
            };
            _bryan.HashedPassword = hasher.HashPassword(_bryan, "password");

            _payment1 = new Payment
            {
                Id = 1,
                PaymentType = PaymentType.Electricity,
                Amount = 175,
                Fixed = false,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 07),
                EndDate = new DateTime(2020, 06, 07),
                Description = "electricity"
            };

            _payment2 = new Payment
            {
                Id = 2,
                PaymentType = PaymentType.Rent,
                Amount = 1000,
                Fixed = true,
                Frequency = Frequency.Monthly,
                StartDate = new DateTime(2020, 03, 01),
                EndDate = new DateTime(2020, 10, 01),
                Description = "rent"
            };

            _userPaymentBryan1 = new UserPayment
            {
                Payment = _payment1,
                User = _bryan,
                UserId = _bryan.Id,
                PaymentId = _payment1.Id
            };

            _userPaymentBryan2 = new UserPayment
            {
                Payment = _payment2,
                User = _bryan,
                UserId = _bryan.Id,
                PaymentId = _payment2.Id
            };

            _userPaymentYin1 = new UserPayment
            {
                Payment = _payment1,
                User = _yin,
                UserId = _yin.Id,
                PaymentId = _payment1.Id
            };

            _userPaymentYin2 = new UserPayment
            {
                Payment = _payment2,
                User = _yin,
                UserId = _yin.Id,
                PaymentId = _payment2.Id
            };

            _userPaymentTeresa1 = new UserPayment
            {
                Payment = _payment1,
                User = _teresa,
                UserId = _teresa.Id,
                PaymentId = _payment1.Id
            };

            _userPaymentTeresa2 = new UserPayment
            {
                Payment = _payment2,
                User = _teresa,
                UserId = _teresa.Id,
                PaymentId = _payment2.Id
            };

            _payment1.UserPayments = new List<UserPayment> { _userPaymentBryan1, _userPaymentTeresa1, _userPaymentYin1 };
            _payment2.UserPayments = new List<UserPayment> { _userPaymentBryan2, _userPaymentTeresa2, _userPaymentYin2 };
            
            _yin.UserPayments = new List<UserPayment> { _userPaymentYin1, _userPaymentYin2 };
            _bryan.UserPayments = new List<UserPayment> { _userPaymentBryan1, _userPaymentBryan2 };
            _teresa.UserPayments = new List<UserPayment> { _userPaymentTeresa1, _userPaymentTeresa2 };

            _schedule1 = new Schedule
            {
                UserName = "BeboBryan",
                ScheduleType = ScheduleType.Away,
                StartDate = new DateTime(2020, 04, 01),
                EndDate = new DateTime(2020, 05, 01)
            };

            _flat1 = new Flat
            {
                Id = 1,
                Address = "50 Symonds Street",
                Users = new List<User> { _yin, _teresa, _bryan },
                Schedules = new List<Schedule> { _schedule1 },
                Payments = new List<Payment> { _payment1, _payment2 }
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

            _database.Add(_yin);
            _database.Add(_teresa);
            _database.Add(_bryan);
            _database.Add(_payment1);
            _database.Add(_payment2);
            _database.Add(_userPaymentBryan1);
            _database.Add(_userPaymentBryan2);
            _database.Add(_userPaymentTeresa1);
            _database.Add(_userPaymentTeresa2);
            _database.Add(_userPaymentYin1);
            _database.Add(_userPaymentYin2);
            _database.Add(_schedule1);
            _database.Add(_flat1);

            _database.SaveChanges();
        }
    }
}
