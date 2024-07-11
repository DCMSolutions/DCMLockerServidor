using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared;
using DCMLockerServidor.Shared.Models;
using System.Text.Json;

namespace DCMLockerServidor.Server.Background
{
    public class DCMServerController : BackgroundService
    {
        private readonly ServerHub _serverHub;
        private readonly ILockerRepositorio _locker;
        public IServiceProvider Services { get; set; }

        public DCMServerController(ServerHub serverHub, IServiceProvider services)
        {
            Services = services;
            _serverHub = serverHub;
            var scope = Services.CreateScope();

            _locker =
                scope.ServiceProvider
                    .GetRequiredService<ILockerRepositorio>();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {

            while (true)
            {

                await GetList();

                await Task.Delay(1000);
            }
        }
        async Task GetList()
        {
            if (_locker != null)
            {
                try
                {
                List<Locker> listaLockers = await _locker.GetLockers();
                    if (listaLockers != null)
                    {
                        foreach (Locker item in listaLockers)
                        {
                            if (item.LastUpdateTime.HasValue)
                            {

                                TimeSpan diferencia = DateTime.Now - item.LastUpdateTime.Value;
                                item.Status = "connected";
                                
                                if (diferencia.TotalSeconds > 10)
                                {
                                    item.Status = "disconnected";
                                }
                                else if (diferencia.TotalSeconds > 5)
                                {
                                    item.Status = "reconnecting";
                                }

                                await _locker.EditLocker(item);
                            }
                            else
                            {
                                // LastUpdateTime es null
                                Console.WriteLine("LastUpdateTime es null");
                            }
                        }
                    }
                    await _serverHub.UpdateLockerList();

                }

                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
