using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecipesBook.Models;
using RecipesBook.Services;

namespace RecipesBook.Controllers
{
    //System.InvalidOperationException: Unable to resolve service for type 'RecipesBook.Services.IRecipeService' while attempting to activate 
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RecipesController : ControllerBase
    {
        private readonly IRecipeService _recipeService;

        public RecipesController(IRecipeService recipeService)
        {
            _recipeService = recipeService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_recipeService.GetAllRecipes());
        }

        [HttpGet("{id}")]
        public IActionResult GetRecipeById(int id)
        {
            var recipe = _recipeService.GetRecipeById(id);

            if (recipe == null)
                return NotFound("There is no recipe in db with that id.");

            return Ok(recipe);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Recipe newRecipe)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _recipeService.CreateNewRecipe(newRecipe);

            return CreatedAtAction("Post", newRecipe);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Recipe changedRecipe)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _recipeService.UpdateRecipe(id, changedRecipe);

            if (success)
                return NoContent();
            else
                return NotFound();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool success = _recipeService.DeleteRecipe(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}