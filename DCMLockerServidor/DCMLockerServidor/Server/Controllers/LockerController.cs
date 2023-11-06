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
        private readonly ServerHub _chatHub;
        private readonly HttpClient _httpClient;
        public LockerController(ILogger<LockerController> logger, IHubContext<ServerHub> hubContext, ServerHub chatHub, HttpClient httpClient)
        {
            _logger = logger;
            _chatHub = chatHub;
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
        public bool AgregarLocker([FromBody] Locker locker)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");

                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Locker> listaDeLockers = JsonSerializer.Deserialize<List<Locker>>(content);

                    listaDeLockers.Add(locker);

                    string s = JsonSerializer.Serialize<List<Locker>>(listaDeLockers);

                    using (StreamWriter b = System.IO.File.CreateText(sf))
                    {
                        b.Write(s);
                    }

                    return true;
                }
                else
                {
                    List<Locker> listaDeLockers = new List<Locker> { locker };
                    string s = JsonSerializer.Serialize<List<Locker>>(listaDeLockers);

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
        public bool DeleteLocker([FromBody] Locker locker)
        {
            try
            {
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");
                Console.WriteLine(sf);
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<Locker> listaDeLockers = JsonSerializer.Deserialize<List<Locker>>(content);

                    listaDeLockers = listaDeLockers.Where(x => x.IpLocker != locker.IpLocker).ToList();

                    string s = JsonSerializer.Serialize<List<Locker>>(listaDeLockers);

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
                    return Ok(); // devolver NotFound si el archivo no existe.
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
                string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");
                Console.WriteLine(sf);
                if (System.IO.File.Exists(sf))
                {
                    string content = System.IO.File.ReadAllText(sf);
                    List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);

                    listaDeLockersToken = listaDeLockersToken.Where(x => x.Locker != lockerToken.Locker).ToList();

                    string s = JsonSerializer.Serialize<List<LockerToken>>(listaDeLockersToken);

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
        public ServerCommunication Post(ServerCommunication serverCommunication)
        {

            
            //_chatHub.SendMessage(serverCommunication.IP, serverCommunication.Name);


            string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "dataToken.ans");

            if (System.IO.File.Exists(sf))
            {
                string content = System.IO.File.ReadAllText(sf);
                List<LockerToken> listaDeLockersToken = JsonSerializer.Deserialize<List<LockerToken>>(content);
                if (listaDeLockersToken.Where(x => x.Token == serverCommunication.Token).ToList().Count()>0)
                {
                    serverCommunication.Locker = listaDeLockersToken.Where(x => x.Token == serverCommunication.Token).First().Box;
                }

            }
            else
            {
                serverCommunication.Locker = "";
            }
            return serverCommunication;
        }
    }
    public class ServerCommunication
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public string? Locker { get; set; }
        public string? Token { get; set; }

    }
}
