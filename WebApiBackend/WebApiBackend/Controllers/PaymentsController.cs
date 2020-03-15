using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApiBackend.EF;
using WebApiBackend.Interfaces;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController<Payment, PaymentsRepository>
    {
        private readonly UserPaymentsRepository userPaymentsRepository;

        private readonly PaymentsRepository paymentsRepository;

        private readonly FlatRepository flatRepository;

        private readonly UserRepository userRepository;

        public PaymentsController(PaymentsRepository paymentsRepository, UserPaymentsRepository userPaymentsRepository, FlatRepository flatRepository, UserRepository userRepository) : base(paymentsRepository)
        {
            this.userPaymentsRepository = userPaymentsRepository;
            this.paymentsRepository = paymentsRepository;
            this.flatRepository = flatRepository;
            this.userRepository = userRepository;
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

            return Ok(payments);
        }

        [HttpGet("Flat/{flatId}")]
        public async Task<IActionResult> GetPaymentsForFlat(int flatId)
        {
            Flat flat = await flatRepository.Get(flatId);

            if (flat == null)
            {
                return NotFound();
            }

            return Ok(flat.Payments);
        }

        [HttpPost("Flat/{flatId}")]
        public async Task<IActionResult> CreatePaymentForFlat(int flatId, [FromBody] Payment payment)
        {
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

            return Ok(payment);
        }

        [HttpDelete("Flat/{flatId}")]
        public async Task<IActionResult> DeletePaymentForFlat(int flatId, int paymentId)
        {
            Flat flat = await flatRepository.Get(flatId);
            Payment entity = await paymentsRepository.Delete(paymentId);

            //foreach()

            return Ok(entity);
        }


    }
}