using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecepiesBook.Models;
using RecepiesBook.Services;

namespace RecepiesBook.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IngredientsController : ControllerBase
    {
        private readonly IngredientService _ingredientService;

        public IngredientsController(IngredientService ingredientService)
        {
            _ingredientService = ingredientService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ingredient>> Get()
        {
            return Ok(_ingredientService.GetAllIngredients());
        }

        [HttpGet("{id}")]
        public ActionResult<Recepie> GetIngredientById(int id)
        {
            var ingredient = _ingredientService.GetIngredientById(id);

            if (ingredient == null)
                return NotFound("There is no ingredient in db with that id.");

            return Ok(ingredient);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Ingredient newIngredient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _ingredientService.CreateNewIngredient(newIngredient);

            return CreatedAtAction("Post", newIngredient);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Ingredient changedIngredient)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _ingredientService.UpdateIngredient(id, changedIngredient);

            if (success)
                return NoContent();
            else
                return NotFound();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _ingredientService.DeleteIngredient(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}