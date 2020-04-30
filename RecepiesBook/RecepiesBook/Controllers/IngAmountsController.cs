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
    public class IngAmountsController : ControllerBase
    {
        private readonly IngAmountService _ingAmountService;

        public IngAmountsController(IngAmountService ingAmountService)
        {
            _ingAmountService = ingAmountService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<IngAmount>> Get() 
        {
            return Ok(_ingAmountService.GetAllIngAmounts());
        }

        [HttpGet("{id}")]
        public ActionResult<Recepie> GetIngredientById(int id)
        {
            var ingAmount = _ingAmountService.GetIngAmountsById(id);

            if (ingAmount == null)
                return NotFound("There is no ingamount in db with that id.");

            return Ok(ingAmount);
        }

        [HttpPost]
        public ActionResult Post([FromBody] IngAmount newIngAmount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _ingAmountService.CreateNewIngAmount(newIngAmount);

            return CreatedAtAction("Post", newIngAmount);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] IngAmount changedIngAmount)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _ingAmountService.UpdateIngAmount(id, changedIngAmount);

            if (success)
                return NoContent();
            else
                return NotFound();
        }


        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            bool success = _ingAmountService.DeleteIngAmount(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}