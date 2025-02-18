using ExpensesAPI.Data;
using ExpensesAPI.Dto.ExpenceDto;
using ExpensesAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using static Azure.Core.HttpHeader;

namespace ExpensesAPI.Controllers
{
    [Authorize]
    [Route("api/expenses")]
    [ApiController]
    public class ExpenseController : ControllerBase 
    {
        private readonly ApplicationDbContext _context;

        public ExpenseController(ApplicationDbContext context)
        {
            _context = context;
        }


        //Vraca trskove ulogovanog korinsika sa mogucnoscu filtriranja

        [HttpGet("get-expenses/")]
        public async Task<IActionResult> GetExpenses(int? categoryId, int? paymentMethodId)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expensesQuery = _context.Expenses
                .Where(n => n.UserId == userId) // Filtrira troškove ulogovanog korisnika
                .AsQueryable(); // Omogućava dodavanje filtera dinamički

            if (categoryId.HasValue) // Proveravamo da li je prosleđen categoryId
            {
                expensesQuery = expensesQuery.Where(n => n.CategoryId == categoryId);
            }

            if (paymentMethodId.HasValue) // Proveravamo da li je prosleđen paymentMethodId
            {
                expensesQuery = expensesQuery.Where(n => n.PaymentMethodId == paymentMethodId);
            }

            var expenses = await expensesQuery
                .Select(n => new ExpenseDto
                {
                    Id = n.Id,
                    Title = n.Title,
                    Amount = n.Amount,
                    Description = n.Description,
                    
                })
                .ToListAsync();

            return Ok(expenses);
        }




        //Vraca samo trosak koji zahtevamo po id 
        [HttpGet("get-expence/{id}")]
        public async Task<IActionResult> GetNote(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expense = await _context.Expenses
                .Where(n => n.UserId == userId && n.Id == id)
                .FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound(new { Message = "Expense not found" });
            }

            var expenseDto = new ExpenseDto
            {
                Id = expense.Id,
                Title = expense.Title,
                Amount= expense.Amount,
                Description = expense.Description
            };

            return Ok(expenseDto);
        }

        // Dodaje novi trosak za autentifikovanog korisnika
        [HttpPost("add-expense")]
        public async Task<IActionResult> AddExpense([FromBody] ExpenseForCreationDto expense)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


                if (expense == null ||
                       string.IsNullOrWhiteSpace(expense.Title) ||
                       expense.Amount <= 0 ||
                       expense.categoryId <= 0 ||
                       expense.paymentMethodId <= 0)
                {
                    return BadRequest(new { Message = "Invalid input data. Please check all fields." });
                }

                

                var expenseToCreate = new Expense
                {
                    Title = expense.Title,
                    Description = expense.Description,
                    Amount = expense.Amount,
                    UserId = userId,
                    CategoryId=expense.categoryId,
                    PaymentMethodId=expense.paymentMethodId,
                };

                await _context.Expenses.AddAsync(expenseToCreate);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Expense added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        // Azurira postojeci trosak ako postoji i pripada ulogovanom korisniku
        [HttpPut("update-expense/{id}")]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseForUpdate expense)
        {
            try
            {
                var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

                var expenseToUpdate = await _context.Expenses.Where(n => n.UserId == userId && n.Id == id).FirstOrDefaultAsync();

                if (expenseToUpdate == null)
                {
                    return NotFound();
                }

                expenseToUpdate.Title = expense.Title;
                expenseToUpdate.Description = expense.Description;
                expenseToUpdate.Amount= expense.Amount;
                expenseToUpdate.PaymentMethodId = expense.paymentMethodId;
                expenseToUpdate.CategoryId= expense.categoryId;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Note updated succesfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }


        


        

        // Brise trosak ako postoji i pripada trenutno ulogovanom korisniku
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var expense = await _context.Expenses.Where(n => n.UserId == userId && n.Id == id).FirstOrDefaultAsync();

            if (expense == null)
            {
                return NotFound();
            }

            _context.Expenses.Remove(expense);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Expense deleted successfully." });
        }












    }
}
