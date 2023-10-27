namespace DCMLockerServidor.Server.Background
{
    public class DCMServerController : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //throw new NotImplementedException();
            return Task.CompletedTask;
        }
    }
}
