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
    public class RecepiesController : ControllerBase
    {
        private readonly IRecepieService _recepieService;

        public RecepiesController(IRecepieService recepieService)
        {
            _recepieService = recepieService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_recepieService.GetAllRecepies());
        }

        [HttpGet("{id}")]
        public IActionResult GetRecepieById(int id)
        {
            var recepie = _recepieService.GetRecepieById(id);

            if (recepie == null)
                return NotFound("There is no recepie in db with that id.");

            return Ok(recepie);
        }

        [HttpPost]
        public IActionResult Post([FromBody] Recepie newRecepie)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _recepieService.CreateNewRecepie(newRecepie);

            return CreatedAtAction("Post", newRecepie);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, [FromBody] Recepie changedRecepie)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            bool success = _recepieService.UpdateRecepie(id, changedRecepie);

            if (success)
                return NoContent();
            else
                return NotFound();
        }


        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            bool success = _recepieService.DeleteRecepie(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}