using ExpensesAPI.Data;
using ExpensesAPI.Dto.CategoryDto;
using ExpensesAPI.Dto.PaymentMethodDto;
using ExpensesAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesAPI.Controllers
{
    [Authorize]
    [Route("api/payment-methods")]
    [ApiController]

    public class PaymentMethodController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaymentMethodController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vraca sve nacine placanja iz baze 
        [HttpGet("get-payment-methods")]
        public async Task<IActionResult> GetPaymentMethods()
        {
            var paymentMethods = await _context.PaymentMethods
                .Select(p => new PaymentMethodDto
                {
                    Id = p.Id,
                    PaymentName = p.PaymentName
                }).ToListAsync();
                 

            return Ok(paymentMethods);

        }

        // Vraca sve nacin placanja po id-ju 
        [HttpGet("get-payment-method/{id}")]
        public async Task<IActionResult> GetPaymentMethod(int id)
        {
            var paymentMethod = await _context.PaymentMethods.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (paymentMethod == null)
            {
                return NotFound(new { Message = "Payment method not found" });
            }

            var paymentMethodDto = new PaymentMethodDto
            {
                Id = paymentMethod.Id,
                PaymentName= paymentMethod.PaymentName

            };

            return Ok(paymentMethodDto);
        }

        //Dodavanje novog nacina placanja
        [HttpPost("add-payment-method")]
        public async Task<IActionResult> AddPaymentMethod([FromBody] PaymentMethodForCreationDto paymentMethod)
        {
            try
            {
                if (paymentMethod == null ||
                       string.IsNullOrWhiteSpace(paymentMethod.PaymentName))

                {
                    return BadRequest(new { Message = "Invalid input data. Please check all fields." });
                }

                var paymentMethodToCreate = new PaymentMethod
                {
                    PaymentName = paymentMethod.PaymentName
                };

                await _context.PaymentMethods.AddAsync(paymentMethodToCreate);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Payment method added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }


        }

        //Azuriranje nacina placanja 
        [HttpPut("update-payment-method/{id}")]
        public async Task<IActionResult> UpdatePaymentMethod(int id,[FromBody] PaymentMethodForUpdateDto paymentMethod)
        {
            try
            {


                var paymentMethodToUpdate = await _context.PaymentMethods.Where(n => n.Id == id).FirstOrDefaultAsync();

                if (paymentMethod == null)
                {
                    return NotFound();
                }

                paymentMethodToUpdate.PaymentName = paymentMethod.PaymentName;

                await _context.SaveChangesAsync();

                return Ok(new { Message = "Payment method updated succesfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }

        }

        //Brisanje metode placanja po id-ju 
        [HttpDelete("delete-payment-method/{id}")]
        public async Task<IActionResult> deletePaymentMethod(int id)
        {
            var paymentMethod = await _context.PaymentMethods.Where(n => n.Id == id).FirstOrDefaultAsync();

            if (paymentMethod == null)
            {
                return NotFound();
            }

            _context.PaymentMethods.Remove(paymentMethod);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Payment method deleted successfully." });
        }
        


    }
}
