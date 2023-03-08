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
    public class NumeroVillaController : ControllerBase
    {
        private readonly ILogger<NumeroVillaController> _logger;
        private readonly IVillaRepository _villarepo;
        private readonly INumeroVillaRepository _numeroRepo;
        private readonly IMapper _mapper;
        protected APIResponse _response;
        public NumeroVillaController(ILogger<NumeroVillaController> logger, 
                                     IVillaRepository villarepo, IMapper mapper,
                                     INumeroVillaRepository numeroRepo)
        {
            _logger = logger;
            _villarepo=villarepo;
            _numeroRepo = numeroRepo;
            _mapper = mapper;
            _response = new();
        }

      
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<APIResponse>> getNumeroVillas()
        {

            try
            {
                _logger.LogInformation("Obtener numero de villas");
                IEnumerable<NumeroVilla> numerovillalist = await _numeroRepo.ObtenerTodos();
                _response.Resultado = _mapper.Map<IEnumerable<NumeroVillaDto>>(numerovillalist);
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
       
        
        
        [HttpGet("id:int", Name ="getnumerovilla")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<APIResponse>> GetNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    _logger.LogError("Error al traer numero villa con Id  " + id);
                    _response.IsExitoso = false;
                    _response.StatusCode= HttpStatusCode.BadRequest;

                    return BadRequest(_response);
                }
                //var villa = VillaStore.villaList.FirstOrDefault(v => v.Id == id);
                var numerovilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numerovilla == null)
                {
                    _response.IsExitoso= false;
                    _response.StatusCode = HttpStatusCode.NotFound;
                    return NotFound(_response);
                }
                _response.Resultado = _mapper.Map<NumeroVillaDto>(numerovilla);
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
        public async Task<ActionResult<APIResponse>> crearNumeroVilla([FromBody] NumeroVillaCreateDto createDto)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }
                if (await _numeroRepo.Obtener(v => v.VillaNo == createDto.VillaNo) != null)
                {
                    ModelState.AddModelError("NombreExiste", "El numero de villa ya existe");
                    return BadRequest(ModelState);

                }
                if (await _villarepo.Obtener(v=>v.Id==createDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea", "El Id de la villa ya existe");
                    return BadRequest(ModelState);
                }
                if (createDto == null)
                {
                    return BadRequest(createDto);
                }

                NumeroVilla modelo = _mapper.Map<NumeroVilla>(createDto);

                modelo.FechaCreacion=DateTime.Now;
                modelo.FechaActualizacion=DateTime.Now;
                await _numeroRepo.Crear(modelo);
                _response.Resultado = modelo;
                _response.StatusCode = HttpStatusCode.Created;


                return CreatedAtRoute("getNumerovilla", new { id = modelo.VillaNo}, _response);
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
        public async Task<IActionResult> DeleteNumeroVilla(int id)
        {
            try
            {
                if (id == 0)
                {
                    
                    _response.IsExitoso=false;
                    _response.StatusCode =HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                var numerovilla = await _numeroRepo.Obtener(v => v.VillaNo == id);
                if (numerovilla == null)
                {
                    _response.IsExitoso=false;
                    _response.StatusCode=HttpStatusCode.NotFound;
                    return NotFound();
                }
                   await  _numeroRepo.Remover(numerovilla);

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
        public async Task<IActionResult> updateNumeroVilla(int id, [FromBody] NumeroVillaUpdateDto updateDto)
        {
            try
            {
                if (updateDto == null || id != updateDto.VillaNo)
                {
                    _response.IsExitoso = !false;
                    _response.StatusCode = HttpStatusCode.BadRequest;
                    return BadRequest(_response);
                }
                if (await _villarepo.Obtener(v=>v.Id == updateDto.VillaId) == null)
                {
                    ModelState.AddModelError("ClaveForanea","El Id de la villa no exixte");
                    return BadRequest(ModelState);
                }
                NumeroVilla modelo = _mapper.Map<NumeroVilla>(updateDto);

                await _numeroRepo.Actualizar(modelo);
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



        //[HttpPatch("{id:int}")]
        //[ProducesResponseType(StatusCodes.Status204NoContent)]
        //[ProducesResponseType(StatusCodes.Status400BadRequest)]
        //public async Task<IActionResult> updatePartialVilla(int id, JsonPatchDocument<VillaUpdateDto> patchDto)
        //{
        //    try
        //    {
        //        if (patchDto == null || id == 0)
        //        {
        //            return BadRequest();
        //        }


        //        var villa = await _villarepo.Obtener(v => v.Id == id, Tracked: false);

        //        VillaUpdateDto villaDto = _mapper.Map<VillaUpdateDto>(villa);

        //        if (villa == null) return BadRequest();

        //        patchDto.ApplyTo(villaDto, ModelState);

        //        if (!ModelState.IsValid)
        //        {
                        
        //            return BadRequest(ModelState);
        //        }
        //        Villa modelo = _mapper.Map<Villa>(villaDto);

        //        await _villarepo.Actualizar(modelo);
        //        _response.StatusCode = HttpStatusCode.NoContent;


        //        return Ok(_response);
            
        //    }
        //    catch (Exception ex)
        //    {
        //        _response.IsExitoso = false;
        //        _response.ErrorMensaje = new List<string>() { ex.ToString() };
        //    }
        //    return BadRequest(_response);
        //}
    }
}
