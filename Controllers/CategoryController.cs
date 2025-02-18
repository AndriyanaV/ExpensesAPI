using ExpensesAPI.Data;
using ExpensesAPI.Dto.CategoryDto;
using ExpensesAPI.Dto.ExpenceDto;
using ExpensesAPI.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ExpensesAPI.Controllers
{
    [Authorize]
    [Route("api/categories")]
    [ApiController]
    public class CategoryController: ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Vraca sve kategorije iz baze 
        [HttpGet("get-categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _context.Categories
                .Select(c => new CategoryDto
                {
                    Id = c.Id,
                    CategoryName = c.CategoryName
                })
                .ToListAsync();

            return Ok(categories);
        }

        // Vraca zahtevanu kategoriju po id-ju
        [HttpGet("get-category/{id}")]
        public async Task<IActionResult> GetCategory(int id) {
            var category = await _context.Categories
                .Where(n => n.Id == id)
                .FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound(new { Message = "Category not found" });
            }

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                CategoryName = category.CategoryName
                
            };

            return Ok(categoryDto);

        }

        //Azuriranje kategorije po zadatom id-ju
        [HttpPut("update-category/{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryForUpdateDto category)
        {

            try
            {
                

                var categoryToUpdate = await _context.Categories.Where(n => n.Id == id).FirstOrDefaultAsync();

                if (categoryToUpdate == null)
                {
                    return NotFound();
                }

                categoryToUpdate.CategoryName = category.CategoryName;
                
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Category updated succesfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }

        //Dodavanje nove kategorije 
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryForCreationDto category)
        {
            try
            {


                if (category == null ||
                       string.IsNullOrWhiteSpace(category.CategoryName))
                       
                {
                    return BadRequest(new { Message = "Invalid input data. Please check all fields." });
                }



                var categoryToCreate = new Category
                {
                   CategoryName=category.CategoryName
                };

                await _context.Categories.AddAsync(categoryToCreate);
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Category added successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { ex.Message });
            }
        }



        //Brisanje kategorije 
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
           

            var category = await _context.Categories.Where(n =>  n.Id == id).FirstOrDefaultAsync();

            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Category deleted successfully." });
        }






    }
}
