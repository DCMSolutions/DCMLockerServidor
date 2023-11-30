using DCMLockerServidor.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using System.Text.Json;

namespace DCMLockerServidor.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LockerController : ControllerBase
    {
        private readonly ILogger<LockerController> _logger;
        private readonly ServerHub _serverHub;
        private readonly HttpClient _httpClient;
        private IHubContext<ServerHub> _hubContext;
        public LockerController(ILogger<LockerController> logger, IHubContext<ServerHub> hubContext, ServerHub serverHub, HttpClient httpClient)
        {
            _logger = logger;
            _serverHub = serverHub;
            _httpClient = httpClient;

        }

        //Lockers
        [HttpGet]
        public IActionResult GetDelTxt()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    return Ok(content);
                }
                else
                {
                    return Ok(); // Si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }

        [HttpPost("addLocker")]
        public bool AgregarLocker([FromBody] ServerStatus locker)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<ServerStatus> listaDeLockers = JsonSerializer.Deserialize<List<ServerStatus>>(content);

                    listaDeLockers.Add(locker);

                    string s = JsonSerializer.Serialize<List<ServerStatus>>(listaDeLockers);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    List<ServerStatus> listaDeLockers = new List<ServerStatus> { locker };
                    string s = JsonSerializer.Serialize<List<ServerStatus>>(listaDeLockers);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                    return true;
                }
            }
            catch
            {
                throw;
            }
        }

        [HttpPost("deleteLocker")]
        public bool DeleteLocker([FromBody] ServerStatus locker)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");
                Console.WriteLine(sf);
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<ServerStatus> listaDeLockers = JsonSerializer.Deserialize<List<ServerStatus>>(content);

                    listaDeLockers = listaDeLockers.Where(x => x.NroSerie != locker.NroSerie).ToList();

                    string s = JsonSerializer.Serialize<List<ServerStatus>>(listaDeLockers);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }


        //Tokens
        [HttpGet("Token")]
        public IActionResult GetDelTxtToken()
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    return Ok(content);
                }
                else
                {
                    List<LockerToken> emptyList = new List<LockerToken>();
                    string emptyListJson = JsonSerializer.Serialize<List<LockerToken>>(emptyList);
                    return Ok(emptyListJson); // devolver lista vacia si el archivo no existe.
                }
            }
            catch
            {
                return StatusCode(500); // En caso de un error, devolver un código de estado 500 (Internal Server Error).
            }
        }

        [HttpPost("addLockerToken")]
        public bool AgregarLockerToken([FromBody] LockerToken lockerToken)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);

                    listaDeLockersToken.Add(lockerToken);

                    string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    List<LockerToken> listaDeLockersToken = new List<LockerToken> { lockerToken };
                    string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }
                }
                return false;
            }
            catch
            {
                throw;
            }

        }

        [HttpPost("deleteLockerToken")]
        public bool DeleteLockerToken([FromBody] LockerToken lockerToken)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);

                    listaDeLockersToken = listaDeLockersToken.Where(x => x.Token != lockerToken.Token).ToList();

                    string s = JsonSerializer.Serialize(listaDeLockersToken);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception er)
            {
                throw;
            }
        }


        //comunication de servers
        [HttpPost]
        public ServerToken Post(ServerToken serverCommunication)
        {


            //_chatHub.SendMessage(serverCommunication.IP, serverCommunication.Name);


            string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

            if (System.IO.File.Exists(sf))
            {
                string content = System.IO.File.ReadAllText(sf);
                List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);
                if (listaDeLockersToken.Where(x => x.Token == serverCommunication.Token).ToList().Count() > 0)
                {
                    serverCommunication.Locker = listaDeLockersToken.Where(x => x.Token == serverCommunication.Token).First().Box;
                }
            }
            else
            {
                serverCommunication.Locker = null;
            }
            return serverCommunication;
        }

        [HttpPost("status")]
        public async Task<ActionResult> PostConfig(ServerStatus status)
        {
            foreach (var item in status.Locker)
            {
                Console.WriteLine(item.Id);
            }
            List<ServerStatus> listaLockers = new();
            string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");
            try
            {
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    listaLockers = JsonSerializer.Deserialize<List<ServerStatus>>(content);

                    if (listaLockers.Where(x => x.NroSerie == status.NroSerie).ToList().Count == 0)
                    {
                        status.Status = "connected";
                        status.LastUpdateTime = DateTime.Now;
                        listaLockers.Add(status);
                    }
                    else
                    {
                        var item = listaLockers.Where(x => x.NroSerie == status.NroSerie).First();
                        item.Status = "connected";
                        item.LastUpdateTime = DateTime.Now;
                        item.Locker = status.Locker;
                    }
                }
                else
                {
                    status.LastUpdateTime = DateTime.Now;
                    status.Status = "connected";
                    listaLockers = new List<ServerStatus> { status };


                }
                Save(sf, listaLockers);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            return Ok();

        }
        async Task Save(string sf, List<ServerStatus> listaLockers)
        {
            string s = JsonSerializer.Serialize(listaLockers);
            using (StreamWriter b = System.IO.File.CreateText(sf))
            {
                b.Write(s);
            }
        }
    }

}
