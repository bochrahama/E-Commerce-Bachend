using Microsoft.AspNetCore.Mvc;
using EcommerceBackend.Services;
using EcommerceBackend.Entities;
namespace EcommerceBackend.Controllers
{
    //build the controller for the category entity, which will be used to handle HTTP requests and responses
    //. The controller will provide endpoints for creating, reading, updating, and deleting categories.
    [ApiController]
    //it usde for routing the controller to the api/categories endpoint
    [Route("api/[controller]")]
    //ControllerBase is a base class for an MVC controller without view support.
    //It provides the basic functionality for handling HTTP requests and responses, such as model binding, validation, and formatting.
    public class CategoriesController : ControllerBase
    {
        public readonly ICategoryService _categoryService;
        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryService.GetAllAsync();
            return Ok(categories);
        }
        [HttpGet("{id:int}")]
        //this method is used to get a category by its id 
        public async Task<IActionResult> GetById(int id) {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            var createdCategory = await _categoryService.CreateAsync(category);
            return CreatedAtAction(nameof(GetById), new { id = createdCategory.Id }, createdCategory);
        }
        [HttpPut("{id:int}")]
        //this method is used to update a category by its id
        public async Task<IActionResult> Update(int id, Category category)
        {
            if (id != category.Id)
            {//if the id in the url does not match the id in the body, return a bad request response
                //bad request response is a HTTP response status code that indicates that the server cannot process the request due to a client error, such as invalid syntax or missing parameters.
                return BadRequest();
            }
            var updatedCategory = await _categoryService.UpdateAsync(category);
            if (!updatedCategory)
            {
                return NotFound();
            }
            return Ok(category);
        }
        [HttpDelete("{id:int}")]
        //this method is used to delete a category by its id
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _categoryService.DeleteAsync(id);
            if (!deleted)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
