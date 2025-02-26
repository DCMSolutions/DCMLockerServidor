//using DCMLockerServidor.Server.Repositorio.Contrato;
//using DCMLockerServidor.Shared.Models;
//using Microsoft.AspNetCore.Authorization;
//using Microsoft.AspNetCore.Mvc;

//namespace DCMLockerServidor.Server.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EventoController : ControllerBase
//    {

//        private readonly IEventoRepositorio _Evento;

//        public EventoController(IEventoRepositorio Evento)
//        {
//            _Evento = Evento;
//        }


//        //CRUD
//        [HttpGet]
//        public async Task<IActionResult> GetEventos()
//        {
//            try
//            {
//                var response = await _Evento.GetSizes();
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpGet("{Id:int}")]
//        public async Task<IActionResult> GetSizeById(int Id)
//        {
//            try
//            {
//                var response = await _Size.GetSizeById(Id);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPost]
//        public async Task<IActionResult> AddSize([FromBody] Size Size)
//        {
//            try
//            {
//                var response = await _Size.AddSize(Size);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpPut]
//        public async Task<IActionResult> EditSize(Size Size)
//        {
//            try
//            {
//                var response = await _Size.EditSize(Size);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        [HttpDelete("{idSize:int}")]
//        public async Task<IActionResult> DeleteSize(int idSize)
//        {
//            try
//            {
//                var response = await _Size.DeleteSize(idSize);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }

//        //funciones

//        [HttpPost("AddListSizes")]
//        public async Task<IActionResult> AddListSizes([FromBody] List<Size> sizes)
//        {
//            try
//            {
//                var response = await _Size.AddListSizes(sizes);
//                return Ok(response);
//            }
//            catch (Exception ex)
//            {
//                return BadRequest(ex.Message);
//            }
//        }
//    }
//}
