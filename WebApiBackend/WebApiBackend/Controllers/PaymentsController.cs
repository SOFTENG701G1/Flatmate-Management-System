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

        [HttpGet("Flat/{flatId}")]
        public async Task<IActionResult> GetPaymentsForFlat(int flatId)
        {
            List<Payment> payments = await paymentsRepository.GetAll();
            List<UserPayment> userPayments = await userPaymentsRepository.GetAll();
            List<User> users = await userRepository.GetAll();


            payments = (from p in payments
                        join up in userPayments on p.Id equals up.PaymentId
                        join u in users on up.UserId equals u.Id
                        where u.FlatId.Equals(flatId)
                        select p).Distinct().ToList();


            if (payments == null)
            {
                return NotFound();
            }
            List<PaymentDTO> paymentDTOs = mapper.Map<List<Payment>, List<PaymentDTO>>(payments);
            return Ok(paymentDTOs);
        }

        [HttpPost("Flat/{flatId}")]
        public async Task<IActionResult> CreatePaymentForFlat(int flatId, [FromBody] PaymentDTO paymentDTO)
        {
            Payment payment = mapper.Map<PaymentDTO, Payment>(paymentDTO);

            await paymentsRepository.Add(payment);

            Flat flat = await flatRepository.Get(flatId);

            flat.Payments.Add(payment);

            await flatRepository.Update(flat);

            foreach (User user in flat.Users)
            {
                UserPayment userPayment = new UserPayment { Payment = payment, User = user, PaymentId = payment.Id, UserId = user.Id };

                user.UserPayments.Add(userPayment);

                await userRepository.Update(user);
            }

            return Ok(paymentDTO);
        }

        [HttpDelete("Flat/{flatId}")]
        public async Task<IActionResult> DeletePaymentForFlat(int flatId, int paymentId)
        {

            //Payment payment = await paymentsRepository.Get(paymentId);
            Payment payment = await paymentsRepository.Delete(paymentId);
            PaymentDTO paymentDTO = mapper.Map<Payment, PaymentDTO>(payment);
            return Ok(paymentDTO);
        }

        [HttpDelete("User/{userId}")]
        public async Task<IActionResult> DeleteUserFromPayment(int paymentId, int userId)
        {
            await userPaymentsRepository.DeleteUserFromPayment(userId, paymentId);
            //Payment payment = await paymentsRepository.Get(paymentId);
            //PaymentDTO paymentDTO = mapper.Map<Payment, PaymentDTO>(payment);
            return Ok();
        }


    }
}