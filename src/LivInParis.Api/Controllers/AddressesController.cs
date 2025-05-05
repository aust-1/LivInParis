//TODO: écrire tous les contrôleurs, c'est un exemple en 2 secondes là


using LivInParisRoussilleTeynier.Domain.Models.Order;
using LivInParisRoussilleTeynier.Infrastructure.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace LivInParisRoussilleTeynier.Api.Controllers
{
    /// <summary>
    /// Injection du repository adresses.
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AddressesController(IAddressRepository repo) : ControllerBase
    {
        private readonly IAddressRepository _repo = repo;

        /// <summary>
        /// GET /api/addresses
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Address>>> GetAll()
        {
            var list = await _repo.GetAllAsync();
            return Ok(list);
        }

        /// <summary>
        /// GET /api/addresses/{id}
        /// </summary>
        [HttpGet("{id:int}")]
        public async Task<ActionResult<Address>> GetById(int id)
        {
            var address = await _repo.GetByIdAsync(id);
            return address is not null ? Ok(address) : NotFound();
        }

        /// <summary>
        /// POST /api/addresses
        /// </summary>
        [HttpPost]
        public async Task<ActionResult> Create(Address address)
        {
            await _repo.AddAsync(address);
            return CreatedAtAction(nameof(GetById), new { id = address.AddressId }, address);
        }

        /// <summary>
        /// PUT /api/addresses/{id}
        /// </summary>
        [HttpPut("{id:int}")]
        public ActionResult Update(int id, Address address)
        {
            if (id != address.AddressId)
                return BadRequest();
            _repo.Update(address);
            return NoContent();
        }

        /// <summary>
        /// DELETE /api/addresses/{id}
        /// </summary>
        [HttpDelete("{id:int}")]
        public ActionResult Delete(int id, Address address)
        {
            if (id != address.AddressId)
                return BadRequest();
            _repo.Delete(address);
            return NoContent();
        }
    }
}

//FIXME: int et adress bizarre

//TODO: Essayer de faire un héritage pour que le code soit plus propre et que les contrôleurs soient plus courts
