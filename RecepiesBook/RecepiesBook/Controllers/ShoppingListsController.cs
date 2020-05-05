using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecepiesBook.Models;
using RecepiesBook.Services;

namespace RecepiesBook.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ShoppingListsController : ControllerBase
    {
        private readonly ShoppingListService _shoppingListService;

        public ShoppingListsController(ShoppingListService shoppingListService)
        {
            _shoppingListService = shoppingListService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<ShoppingList>> Get()
        {
            return Ok(_shoppingListService.GetAllSoppingLists());
        }

        [HttpGet("{id}")]
        public ActionResult<Recepie> GetShoppingListById(int id)
        {
            var sl = _shoppingListService.GetShoppingListById(id);

            if (sl == null)
                return NotFound("There is no shopping list in db with that id.");

            return Ok(sl);
        }

        [HttpPost]
        public ActionResult Post([FromBody] ShoppingList newShoppingList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _shoppingListService.CreateNewShoppingList(newShoppingList);

            return CreatedAtAction("Post", newShoppingList);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] ShoppingList changedShoppingList)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _shoppingListService.UpdateShoppingList(id, changedShoppingList);

            if (success)
                return NoContent();
            else
                return NotFound();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _shoppingListService.DeleteShoppingList(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}