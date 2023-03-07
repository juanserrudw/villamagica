using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaMgic_API.Data;
using VillaMgic_API.Models;
using VillaMgic_API.Models.Dto;

namespace VillaMgic_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly ApplicationDbContext _db;
        private readonly IMapper _mapper;
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db, IMapper mapper)
        {
            _logger = logger;
            _db = db;
            _mapper = mapper;
        }

      
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<VillaDto>>> getVillas()
        {

            _logger.LogInformation("Obteniendo las villas");
            IEnumerable<Villa> villalist = await _db.villas.ToListAsync();
            return Ok(_mapper.Map<IEnumerable<VillaDto>>(villalist));
        }
       
        
        
        [HttpGet("id:int", Name ="getvilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<VillaDto>> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id  " + id);
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa= await _db.villas.FirstOrDefaultAsync(v => v.Id == id);
           if (villa == null)
            {
                return NotFound();
            }
            
            return Ok(_mapper.Map<VillaDto>(villa));
        }
       
        
        
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<VillaCreateDto>> crearVilla([FromBody] VillaCreateDto createDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (await _db.villas.FirstOrDefaultAsync(v=>v.Nombre.ToLower() == createDto.Nombre.ToLower()) !=null)
            {
                ModelState.AddModelError("NombreExiste", "esa villa ya existe");
                return BadRequest(ModelState);

            }
            if (createDto == null)
            {
                return BadRequest(createDto);
            }
            
             Villa modelo =_mapper.Map<Villa>(createDto);

            
           await _db.villas.AddAsync(modelo);
           await _db.SaveChangesAsync();

            return CreatedAtRoute("getvilla", new {id = modelo.Id}, modelo);
        }
       
        
        
        
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            if (id ==0)
            {
                return BadRequest();
            }
            var villa = await _db.villas.FirstOrDefaultAsync(v=>v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.villas.Remove(villa);
            await _db.SaveChangesAsync();
            return NoContent();
        }

       
        
        
        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            if (updateDto == null || id!= updateDto.Id)
            {
                return BadRequest();
            }
            Villa modelo = _mapper.Map<Villa>(updateDto);
            
            _db.Update(modelo);
           await _db.SaveChangesAsync();

            return NoContent();
        }

       
        
        
        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto > patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            

            var villa = await _db.villas.AsNoTracking()
                .FirstOrDefaultAsync(v => v.Id == id);

            VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);
            
            if (villa == null) return BadRequest();
         
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo= _mapper.Map<Villa>(villaDto);
           
            _db.villas.Update(modelo);
           await _db.SaveChangesAsync();
            return NoContent();
        }
    }
}
