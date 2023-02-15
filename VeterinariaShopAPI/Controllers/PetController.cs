using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using VeterinariaShopAPI.Models;

namespace VeterinariaShopAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PetController : ControllerBase
    {
            private readonly DataContext _context;

            public PetController(DataContext context)
            {
                _context = context;
            }

            [HttpGet]
            public async Task<ActionResult<List<PetDTO>>> Get()
            {
                return Ok(await _context.Pets.ToListAsync());
            }

            [HttpGet("{id}")]
            public async Task<ActionResult<PetDTO>> Get(int id)
            {
                var pet = await _context.Pets.FindAsync(id);
                if (pet == null)
                    return BadRequest("Hero not found.");
                return Ok(pet);
            }

            [HttpPost]
            public async Task<ActionResult<List<PetDTO>>> AddPet(PetDTO pet)
            {
                _context.Pets.Add(pet);
                await _context.SaveChangesAsync();

                return Ok(await _context.Pets.ToListAsync());
            }

            [HttpPut]
            public async Task<ActionResult<List<PetDTO>>> UpdatePet(PetDTO request)
            {
                var pet = await _context.Pets.FindAsync(request.Id);
                if (pet == null)
                    return BadRequest("Hero not found.");

                 pet.Type = request.Type;
                 pet.Name = request.Name;
                 pet.Age = request.Age;
                 pet.Weight = request.Weight;
                 pet.Castrated = request.Castrated; 

                await _context.SaveChangesAsync();

                return Ok(await _context.Pets.ToListAsync());
            }

            [HttpDelete("{id}")]
            public async Task<ActionResult<List<PetDTO>>> DeletePet(int id)
            {
                var pet = await _context.Pets.FindAsync(id);
                if (pet == null)
                    return BadRequest("Hero not found.");

                _context.Pets.Remove(pet);
                await _context.SaveChangesAsync();

                return Ok(await _context.Pets.ToListAsync());
            }

        }
    }