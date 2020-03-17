using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApiBackend.Dto;
using WebApiBackend.EF;
using WebApiBackend.Model;

namespace WebApiBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : BaseController<Payment, PaymentsRepository, PaymentDTO>
    {
        private readonly UserPaymentsRepository _userPaymentsRepository;

        private readonly PaymentsRepository _paymentsRepository;

        private readonly FlatRepository _flatRepository;

        private readonly UserRepository _userRepository;

        private readonly IMapper _mapper;
        public PaymentsController(PaymentsRepository paymentsRepository, UserPaymentsRepository userPaymentsRepository, FlatRepository flatRepository, UserRepository userRepository, IMapper mapper
            ) : base(paymentsRepository, mapper)
        {
            this._userPaymentsRepository = userPaymentsRepository;
            this._paymentsRepository = paymentsRepository;
            this._flatRepository = flatRepository;
            this._userRepository = userRepository;
            this._mapper = mapper;
        }

        /// <summary>
        /// GET Method - Gets all the payments that are associated with a specific user
        /// </summary>
        /// <response code="200">All payments are retrieved for user</response>
        /// <response code="401">Not an authorised user</response>
        /// <response code="404">No payments found for user</response>
        /// <returns> Payments that are for that user</returns>
        [HttpGet("User")]
        public async Task<IActionResult> GetAllPaymentsForUser()
        {
            ClaimsIdentity identity = HttpContext.User.Identity as ClaimsIdentity;

            int userID = Int16.Parse(identity.FindFirst(ClaimTypes.NameIdentifier).Value);

         
            List<Payment> payments = await _paymentsRepository.GetAll();
            List<UserPayment> userPayments = await _userPaymentsRepository.GetAll();
            payments = (from p in payments
                        join up in userPayments on p.Id equals up.PaymentId
                        where up.UserId.Equals(userID)
                        select p).ToList();

            if (payments.Count() == 0)
            {
                return NotFound();
            }
            List<PaymentDTO> paymentsDTOs = _mapper.Map<List<Payment>, List<PaymentDTO>>(payments);

            return Ok(paymentsDTOs);
        }

        /// <summary>
        /// GET Method - Gets all payments for a specific flat
        /// </summary>
        /// <param name="flatId">The Id of the flat you want payments for</param>        
        /// <response code="200">All payments are retrieved for flat</response>
        /// <response code="401">Not an authorised user</response>
        /// <response code="404">No payments found for user</response>
        /// <returns> The payments that are associated with the flat</returns>
        [HttpGet("Flat/{flatId}")]
        public async Task<IActionResult> GetPaymentsForFlat(int flatId)
        {
            List<Payment> payments = await GetAllPaymentsFromFlatId(flatId);

            if (payments == null)
            {
                return NotFound();
            }
            List<PaymentDTO> paymentDTOs = _mapper.Map<List<Payment>, List<PaymentDTO>>(payments);

            return Ok(paymentDTOs);
        }

        /// <summary>
        /// GET Method - Gets all users contributing to a payment
        /// </summary>
        /// <param name="paymentId">The id for the payment that you want all the contributors for</param>
        /// <response code="200">All users retreived for payment</response>
        /// <response code="401">Not an authorised user</response>
        /// <response code="404">No payments found with given id</response>
        /// <returns> The users associated with payment</returns>
        [HttpGet("Users")]
        public async Task<IActionResult> GetAllUsersForPayment([FromQuery] int paymentId)
        {            
            List<User> users = await _userRepository.GetAll();
            List<UserPayment> userPayments = await _userPaymentsRepository.GetAll();

            users = (from u in users
                     join up in userPayments on u.Id equals up.UserId
                     where up.PaymentId.Equals(paymentId)
                     select u).ToList();

            if(users.Count == 0)
            {
                return NotFound();
            }

            List<UserDTO> userDtos = _mapper.Map<List<User>, List<UserDTO>>(users);

            return Ok(userDtos);
        }

        /// <summary>
        /// POST Method - Creates a new payment for the flat by taking in a list of users
        /// that are meant to be in the list
        /// </summary>
        /// <param name="flatId">The Id of the flat you want to add a payment for</param>
        /// <param name="paymentDTO">The payment you want to be created</param>
        /// <param name="userIds">List of user id's to associate with the payment</param>
        /// <response code="200">Payment created</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns> The created payment is returned </returns>        
        [HttpPost("Flat/{flatId}")]
        public async Task<IActionResult> CreatePaymentForFlat(int flatId, [FromBody] PaymentDTO paymentDTO, [FromHeader] List<int> userIds)
        {
            Payment payment = _mapper.Map<PaymentDTO, Payment>(paymentDTO);

            await _paymentsRepository.Add(payment);

            List<Payment> payments = await GetAllPaymentsFromFlatId(flatId);

            Flat flat = await _flatRepository.Get(flatId);

            payments.Add(payment);

            flat.Payments = payments;

            await _flatRepository.Update(flat);

            foreach (int userId in userIds)
            {
                User user = await _userRepository.Get(userId);

                UserPayment userPayment = new UserPayment { Payment = payment, User = user, PaymentId = payment.Id, UserId = user.Id };

                user.UserPayments.Add(userPayment);

                await _userRepository.Update(user);
            }

            return Ok(paymentDTO);
        }

        /// <summary>
        /// PUT Method - Adds a user to an existing payment
        /// </summary>
        /// <param name="paymentId">Id of the payment you want to add an user to</param>
        /// <param name="userId">Id of the user you want to add</param>
        /// <response code="204">Payment was successfully updated</response>
        /// <response code="401">Not an authorised user</response>
        /// <response code="404">No payments found for user or the user with the given id is not found</response>
        /// <returns> NoContent </returns>
        [HttpPut("User/{paymentId}")]
        public async Task<IActionResult> AddUserToExistingPayment(int paymentId, [FromQuery] int userId)
        {
            Payment payment = await _paymentsRepository.Get(paymentId);
            User user = await _userRepository.Get(userId);

            UserPayment userPayment = new UserPayment { UserId = userId, PaymentId = paymentId, User = user, Payment = payment };

            await _userPaymentsRepository.Add(userPayment);

            return NoContent();
        }

        /// <summary>
        /// DELETE Method - Removes a whole Payment for the flat
        /// </summary>
        /// <param name="paymentId">Id of payment you want to delete</param>
        /// <response code="200">Payment deleted for a flat</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns> Returns the payment that got removed </returns>
        [HttpDelete("Flat")]
        public async Task<IActionResult> DeletePaymentForFlat(int paymentId)
        {
            Payment payment = await _paymentsRepository.Delete(paymentId);
            PaymentDTO paymentDTO = _mapper.Map<Payment, PaymentDTO>(payment);
            return Ok(paymentDTO);
        }

        /// <summary>
        /// DELETE Method - Removes User from a specific patyment by removing a
        /// row from UserPayments table
        /// </summary>
        /// <param name="paymentId">Id of payment you want user to be removed from</param>
        /// <param name="userId">Id of user to be removed</param>
        /// <response code="204">User removed successfully from payment</response>
        /// <response code="401">Not an authorised user</response>
        /// <returns> No content when user is removed </returns>
        [HttpDelete("User/{paymentId}")]
        public async Task<IActionResult> DeleteUserFromPayment(int paymentId, [FromQuery] int userId)
        {
            await _userPaymentsRepository.DeleteUserFromPayment(userId, paymentId);
            return NoContent();
        }

        /// <summary>
        /// Helper method to get all payments for a specific flat
        /// </summary>
        /// <param name="flatId"></param>
        /// <returns>List of payments</returns>
        private async Task<List<Payment>> GetAllPaymentsFromFlatId(int flatId)
        {
            List<Payment> payments = await _paymentsRepository.GetAll();
            List<UserPayment> userPayments = await _userPaymentsRepository.GetAll();
            List<User> users = await _userRepository.GetAll();


            payments = (from p in payments
                        join up in userPayments on p.Id equals up.PaymentId
                        join u in users on up.UserId equals u.Id
                        where u.FlatId.Equals(flatId)
                        select p).Distinct().ToList();

            return payments;
        }
    }
}