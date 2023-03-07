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
        public VillaController(ILogger<VillaController> logger, ApplicationDbContext db)
        {
            _logger = logger;
            _db = db;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult<IEnumerable<VillaDto>> getVillas()
        {

            _logger.LogInformation("Obteniendo las villas");
            return Ok(_db.villas.ToList());
        }
        [HttpGet("id:int", Name ="getvilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public ActionResult<VillaDto> GetVilla(int id)
        {
            if (id == 0)
            {
                _logger.LogError("Error al traer villa con Id  " + id);
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            var villa= _db.villas.FirstOrDefault(v => v.Id == id);
           if (villa == null)
            {
                return NotFound();
            }
            
            return Ok(villa);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<VillaDto> crearVilla([FromBody] VillaDto villaDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (_db.villas.FirstOrDefault(v=>v.Nombre.ToLower() == villaDto.Nombre.ToLower()) !=null)
            {
                ModelState.AddModelError("NombreExiste", "esa villa ya existe");
                return BadRequest(ModelState);

            }
            if (villaDto == null)
            {
                return BadRequest(villaDto);
            }
            if (villaDto.Id> 0)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            //villaDto.Id = VillaStore.villaList.OrderByDescending(v=>v.Id).FirstOrDefault().Id + 1;
            //VillaStore.villaList.Add(villaDto);

            Villa modelo = new Villa()
            {
                
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad

            };
            _db.villas.Add(modelo);
            _db.SaveChanges();

            return CreatedAtRoute("getvilla", new {id = villaDto.Id}, villaDto);
        }
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public IActionResult DeleteVilla(int id)
        {
            if (id ==0)
            {
                return BadRequest();
            }
            var villa = _db.villas.FirstOrDefault(v=>v.Id == id);
            if (villa == null)
            {
                return NotFound();
            }
            _db.villas.Remove(villa);
            _db.SaveChanges();
            return NoContent();
        }

        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult updateVilla(int id, [FromBody] VillaDto villaDto)
        {
            if (villaDto ==null || id!=villaDto.Id)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
            //villa.Nombre= villadto.Nombre;
            //villa.Ocupantes= villadto.Ocupantes;
            //villa.MetrosCuadrados = villadto.MetrosCuadrados;

            Villa modelo = new Villa()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad

            };
            _db.Update(modelo);
            _db.SaveChanges();

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult updatePartialVilla(int id, JsonPatchDocument<VillaDto > patchDto)
        {
            if (patchDto == null || id == 0)
            {
                return BadRequest();
            }
            //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);

            var villa = _db.villas.AsNoTracking()
                .FirstOrDefault(v => v.Id == id);

            VillaDto villaDto = new()
            {
                Id = villa.Id,
               
                Nombre = villa.Nombre,
                Detalle = villa.Detalle,
                ImagenUrl = villa.ImagenUrl,
                Ocupantes = villa.Ocupantes,
                Tarifa = villa.Tarifa,
                MetrosCuadrados = villa.MetrosCuadrados,
                Amenidad = villa.Amenidad
            };
            if (villa == null) return BadRequest();
         
            patchDto.ApplyTo(villaDto, ModelState);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Villa modelo = new()
            {
                Id = villaDto.Id,
                Nombre = villaDto.Nombre,
                Detalle = villaDto.Detalle,
                ImagenUrl = villaDto.ImagenUrl,
                Ocupantes = villaDto.Ocupantes,
                Tarifa = villaDto.Tarifa,
                MetrosCuadrados = villaDto.MetrosCuadrados,
                Amenidad = villaDto.Amenidad
            };
            _db.villas.Update(modelo);
            _db.SaveChanges();
            return NoContent();
        }
    }
}
