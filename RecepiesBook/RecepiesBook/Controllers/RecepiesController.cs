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
        private readonly RecepieService _recepieService;

        public RecepiesController(RecepieService recepieService)
        {
            _recepieService = recepieService;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Recepie>> Get()
        {
            return Ok(_recepieService.GetAllRecepies());
        }

        [HttpGet("{id}")]
        public ActionResult<Recepie> GetRecepieById(int id)
        {
            var recepie = _recepieService.GetRecepieById(id);

            if (recepie == null)
                return NotFound("There is no recepie in db with that id.");

            return Ok(recepie);
        }

        [HttpPost]
        public ActionResult Post([FromBody] Recepie newRecepie)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _recepieService.CreateNewRecepie(newRecepie);

            return CreatedAtAction("Post", newRecepie);
        }

        [HttpPut("{id}")]
        public ActionResult Update(int id, [FromBody] Recepie changedRecepie)
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
        public ActionResult Delete(int id)
        {
            bool success = _recepieService.DeleteRecepie(id);

            if (success)
                return Ok();
            else
                return NotFound();
        }
    }
}