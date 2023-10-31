using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

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
       

        [HttpPost]
        public ServerCommunication Post(ServerCommunication serverCommunication)
        {
            Console.WriteLine(serverCommunication.Name);
            Console.WriteLine(serverCommunication.IP);
            
            _chatHub.SendMessage(serverCommunication.IP, serverCommunication.Name);
            Console.Write("Ingrese el valor para serverCommunication.CU: ");
            serverCommunication.CU = Convert.ToInt32(Console.ReadLine());
            serverCommunication.CU = 1;
            Console.Write("Ingrese el valor para serverCommunication.Locker: ");
            serverCommunication.Locker = Convert.ToInt32(Console.ReadLine());


            return serverCommunication;
        }
    }
    public class ServerCommunication
    {
        public string IP { get; set; }
        public string Name { get; set; }
        public int? CU { get; set; }
        public int? Locker { get; set; }

    }
}
