using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VillaMgic_API.Data;
using System.Net;
using VillaMgic_API.Models;
using VillaMgic_API.Models.Dto;
using VillaMgic_API.Repository.IRepository;

namespace VillaMgic_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VillaController : ControllerBase
    {
        private readonly ILogger<VillaController> _logger;
        private readonly IVillaRepository _villarepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public VillaController(ILogger<VillaController> logger, IVillaRepository villarepo, IMapper mapper)
        {
            _logger = logger;
            _villarepo=villarepo;
            _mapper = mapper;
            _response = new();
        }

      
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> getVillas()
        {

            try
            {
                _logger.LogInformation("Obteniendo las villas");
                IEnumerable<Villa> villalist = await _villarepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<VillaDto>>(villalist);
                _response.StatusCode = HttpStatusCode.OK;
                return Ok(_response);

            }
            catch (Exception ex)
            {

              _response.IsExitoso= false;
              _response.ErrorMensaje = new List<string>() { ex.ToString() };
            }
            return _response;
            
        }
       
        
        
        [HttpGet("id:int", Name ="getvilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer villa con Id  " + id);
                    _response.IsExitoso = false;
                    _response.StatusCode= HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var villa = await _villarepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso= false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<VillaDto>(villa);
                _response.StatusCode=HttpStatusCode.OK;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
            }
            return _response;
        }
       
        
        
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<APIResponse>> crearVilla([FromBody] VillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _villarepo.Obtener(v => v.Nombre.ToLower() == createDto.Nombre.ToLower()) != null)
                {
                    ModelState.AddModelError("NombreExiste", "esa villa ya existe");
                    return BadRequest(ModelState);

                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                Villa modelo = _mapper.Map<Villa>(createDto);

                modelo.Fechacreacion=DateTime.Now;
                modelo.FechaActualizacion=DateTime.Now;
                await _villarepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("getvilla", new { id = modelo.Id }, _response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
            }
            return _response;
        }
       
        
        
        
        [HttpDelete("id:int")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    
                    _response.IsExitoso=false;
                    _response.StatusCode =HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var villa = await _villarepo.Obtener(v => v.Id == id);
                if (villa == null)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound();
                }
                   await  _villarepo.Remover(villa);

                _response.StatusCode = HttpStatusCode.NoContent;
                return Ok(_response);
            }
            catch (Exception ex)
            {

                _response.IsExitoso = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };

            }
            return BadRequest(_response);
        }




        [HttpPut("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updateVilla(int id, [FromBody] VillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.Id)
                {
                    _response.IsExitoso = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                Villa modelo = _mapper.Map<Villa>(updateDto);

                await _villarepo.Actualizar(modelo);
                _response.StatusCode = HttpStatusCode.NoContent;

                return Ok(_response);
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };

            }
            return BadRequest(_response);
        }



        [HttpPatch("{id:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> updatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        {
            try
            {
                if (patchDto == null || id == 0)
                {
                    return BadRequest();
                }


                var villa = await _villarepo.Obtener(v => v.Id == id, Tracked: false);

                VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

                if (villa == null) return BadRequest();

                patchDto.ApplyTo(villaDto, ModelState);

                if (!ModelState.IsValid)
                {
                        
                    return BadRequest(ModelState);
                }
                Villa modelo = _mapper.Map<Villa>(villaDto);

                await _villarepo.Actualizar(modelo);
                _response.StatusCode = HttpStatusCode.NoContent;


                return Ok(_response);
            
            }
            catch (Exception ex)
            {
                _response.IsExitoso = false;
                _response.ErrorMensaje = new List<string>() { ex.ToString() };
            }
            return BadRequest(_response);
        }
    }
}
