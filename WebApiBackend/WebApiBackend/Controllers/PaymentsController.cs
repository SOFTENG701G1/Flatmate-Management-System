using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController<Payment, PaymentsRepository, PaymentDTO>
    {
        private readonly UserPaymentsRepository userPaymentsRepository;

        private readonly PaymentsRepository paymentsRepository;

        private readonly FlatRepository flatRepository;

        private readonly UserRepository userRepository;

        private readonly IMapper mapper;
        public PaymentsController(PaymentsRepository paymentsRepository, UserPaymentsRepository userPaymentsRepository, FlatRepository flatRepository, UserRepository userRepository, IMapper mapper
            ) : base(paymentsRepository, mapper)
        {
            this.userPaymentsRepository = userPaymentsRepository;
            this.paymentsRepository = paymentsRepository;
            this.flatRepository = flatRepository;
            this.userRepository = userRepository;
            this.mapper = mapper;
        }
        /// <summary>
        /// GET Method - Gets all the payments that are associated with a specific user
        /// </summary>
        /// <param name="userId"></param>
        /// <returns> Payments that are for that user</returns>
        // TODO: Change the method to use authentication instead of userId
        [HttpGet("User/{userId}")]
        public async Task<IActionResult> GetAllPaymentForUser(int userId)
        {
            List<Payment> payments = await paymentsRepository.GetAll();
            List<UserPayment> userPayments = await userPaymentsRepository.GetAll();
            payments = (from p in payments
                        join up in userPayments on p.Id equals up.PaymentId
                        where up.UserId.Equals(userId)
                        select p).ToList();

            if (payments.Count() == 0)
            {
                return NotFound();
            }
            List<PaymentDTO> paymentsDTOs = mapper.Map<List<Payment>, List<PaymentDTO>>(payments);
            return Ok(paymentsDTOs);
        }
        /// <summary>
        /// GET Method - Gets all payments for a specific flat
        /// </summary>
        /// <param name="flatId"></param>
        /// <returns> The payments that are associated with the flat</returns>
        [HttpGet("Flat/{flatId}")]
        public async Task<IActionResult> GetPaymentsForFlat(int flatId)
        {
            List<Payment> payments = await GetAllPaymentsFromFlatId(flatId);

            if (payments == null)
            {
                return NotFound();
            }
            List<PaymentDTO> paymentDTOs = mapper.Map<List<Payment>, List<PaymentDTO>>(payments);
            return Ok(paymentDTOs);
        }

        /// <summary>
        /// POST Method - Creates a new payment for the flat by taking in a list of users
        /// that are meant to be in the list
        /// </summary>
        /// <param name="flatId"></param>
        /// <param name="paymentDTO"></param>
        /// <param name="userIds"></param>
        /// <returns> The paymentDTO that is created</returns>        
        [HttpPost("Flat/{flatId}")]
        public async Task<IActionResult> CreatePaymentForFlat(int flatId, [FromBody] PaymentDTO paymentDTO, [FromHeader] List<int> userIds)
        {
            Payment payment = mapper.Map<PaymentDTO, Payment>(paymentDTO);

            await paymentsRepository.Add(payment);

            List<Payment> payments = await GetAllPaymentsFromFlatId(flatId);

            Flat flat = await flatRepository.Get(flatId);

            payments.Add(payment);

            flat.Payments = payments;

            await flatRepository.Update(flat);

            foreach (int userId in userIds)
            {
                User user = await userRepository.Get(userId);

                UserPayment userPayment = new UserPayment { Payment = payment, User = user, PaymentId = payment.Id, UserId = user.Id };

                user.UserPayments.Add(userPayment);

                await userRepository.Update(user);
            }

            return Ok(paymentDTO);
        }
        /// <summary>
        /// PUT Method - Adds a user to an existing payment
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="userID"></param>
        /// <returns> NoContent </returns>
        [HttpPut("User/{paymentId}")]
        public async Task<IActionResult> AddUserToExistingPayment(int paymentId,[FromQuery] int userID)
        {
            Payment payment = await paymentsRepository.Get(paymentId);
            User user = await userRepository.Get(userID);

            UserPayment userPayment = new UserPayment { UserId = userID, PaymentId = paymentId, User = user, Payment = payment };

            await userPaymentsRepository.Add(userPayment);

            return NoContent();
        } 

        /// <summary>
        /// DELETE Method - Removes a whole Payment for the flat
        /// </summary>
        /// <param name="paymentId"></param>
        /// <returns> Returns the payment that got removed </returns>
        [HttpDelete("Flat/{flatId}")]
        public async Task<IActionResult> DeletePaymentForFlat(int paymentId)
        {
            Payment payment = await paymentsRepository.Delete(paymentId);
            PaymentDTO paymentDTO = mapper.Map<Payment, PaymentDTO>(payment);
            return Ok(paymentDTO);
        }

        /// <summary>
        /// DELETE Method - Removes User from a specific patyment by removing a
        /// row from UserPayments table
        /// </summary>
        /// <param name="paymentId"></param>
        /// <param name="userId"></param>
        /// <returns> 200 OK if deleted successfully </returns>
        [HttpDelete("User/{paymentId}")]
        public async Task<IActionResult> DeleteUserFromPayment(int paymentId, [FromQuery] int userId)
        {
            await userPaymentsRepository.DeleteUserFromPayment(userId, paymentId);
            return NoContent();
        }
        /// <summary>
        /// Helper method to get all payments for a specific flat
        /// </summary>
        /// <param name="flatId"></param>
        /// <returns>List of payments</returns>
        private async Task<List<Payment>> GetAllPaymentsFromFlatId(int flatId)
        {
            List<Payment> payments = await paymentsRepository.GetAll();
            List<UserPayment> userPayments = await userPaymentsRepository.GetAll();
            List<User> users = await userRepository.GetAll();


            payments = (from p in payments
                        join up in userPayments on p.Id equals up.PaymentId
                        join u in users on up.UserId equals u.Id
                        where u.FlatId.Equals(flatId)
                        select p).Distinct().ToList();

            return payments;
        }
    }
}