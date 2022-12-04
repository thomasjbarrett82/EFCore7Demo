using EFCore7Demo.Data;
using EFCore7Demo.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCore7Demo.Controllers {
    [Route("api/[controller]")]
    [ApiController]
    public class PersonController : ControllerBase {
        private readonly SqlDbContext _context;

        public PersonController(SqlDbContext context) {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Person>> GetPerson(int id) {
            if (id <= 0)
                return BadRequest();

            var result = await _context.Persons.FirstOrDefaultAsync(p => p.Id == id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpGet("JSON/{id}")]
        public async Task<ActionResult<PersonJson>> GetPersonJson(int id) {
            if (id <= 0)
                return BadRequest();

            var result = await _context.PersonJsons.FirstOrDefaultAsync(p => p.Id == id);
            if (result is null)
                return NotFound();

            return Ok(result);
        }

        [HttpPut]
        public async Task<ActionResult<Person>> UpsertPerson([FromBody] Person request) {
            if (request is null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var entity = await _context.Persons.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (entity is null) {
                _context.Persons.Add(request);
            }
            else {
                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.Addresses = request.Addresses;
                _context.Update(entity);
            }

            await _context.SaveChangesAsync();
            return request;
        }

        [HttpPut("JSON")]
        public async Task<ActionResult<PersonJson>> UpsertPersonJson([FromBody] PersonJson request) {
            if (request is null)
                return BadRequest();
            if (!ModelState.IsValid)
                return BadRequest();

            var entity = await _context.PersonJsons.FirstOrDefaultAsync(p => p.Id == request.Id);
            if (entity is null) {
                _context.PersonJsons.Add(request);
            }
            else {
                entity.FirstName = request.FirstName;
                entity.LastName = request.LastName;
                entity.Addresses = request.Addresses;
                _context.Update(entity);
            }

            await _context.SaveChangesAsync();
            return request;
        }
    }
}
