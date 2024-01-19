//using DCMLockerServidor.Shared;
//using System.Text.Json;

//namespace DCMLockerServidor.Server.Background
//{
//    public class DCMServerController : BackgroundService
//    {
//        private readonly ServerHub _serverHub;
//        public DCMServerController(ServerHub serverHub)
//        {
//            _serverHub = serverHub;
//        }
//        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
//        {
//            while (true)
//            {
//                await GetList();
//                await Task.Delay(1000);
//            }
//        }
//        async Task<List<ServerStatus>> GetList()
//        {
//            List<ServerStatus> listaLockers = new();
//            string sf = System.IO.Path.Combine(Directory.GetCurrentDirectory(), "data.ans");
//            try
//            {
//                if (System.IO.File.Exists(sf))
//                {
//                    string content = System.IO.File.ReadAllText(sf);
//                    listaLockers = JsonSerializer.Deserialize<List<ServerStatus>>(content);

//                    List<ServerStatus> elementosPorEliminar = new();

//                    foreach (ServerStatus item in listaLockers)
//                    {
//                        if (item.LastUpdateTime.HasValue)
//                        {
//                            TimeSpan diferencia = DateTime.Now - item.LastUpdateTime.Value;

//                            if (diferencia.TotalSeconds > 5)
//                            {
//                                item.Status = "reconnecting";
//                            }
//                            if (diferencia.TotalSeconds > 10)
//                            {
//                                item.Status = "disconnected";
//                            }
//                        }
//                        else
//                        {
//                            // LastUpdateTime es null
//                            Console.WriteLine("LastUpdateTime es null");
//                        }
//                    }

//                    // Eliminar elementos después de la iteración
//                    foreach (var elementoEliminar in elementosPorEliminar)
//                    {
//                        listaLockers.Remove(elementoEliminar);
//                    }
//                }
//                await Save(sf, listaLockers);
//                await _serverHub.SendLockerList();

//            }

//            catch (Exception ex)
//            {
//                Console.WriteLine(ex.Message);
//            }
//            return listaLockers;
//        }
//        async Task Save(string sf, List<ServerStatus> listaLockers)
//        {
//            string s = JsonSerializer.Serialize(listaLockers);
//            using (StreamWriter b = System.IO.File.CreateText(sf))
//            {
//                b.Write(s);
//            }
//        }
//    }
//}
