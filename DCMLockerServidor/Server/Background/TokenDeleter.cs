using DCMLockerServidor.Server.Repositorio.Contrato;
using DCMLockerServidor.Shared.Models;
using Microsoft.Extensions.Configuration;

namespace DCMLockerServidor.Server.Background
{
    public class TokenDeleter : BackgroundService
    {
        private readonly ServerHub _serverHub;
        private readonly ITokenRepositorio _token;
        private readonly IConfiguration _configuration;
        public IServiceProvider Services { get; set; }

        public TokenDeleter(ServerHub serverHub, IServiceProvider services, IConfiguration configuration)
        {
            Services = services;
            _serverHub = serverHub;
            var scope = Services.CreateScope();
            _configuration = configuration;

            _token =
                scope.ServiceProvider
                    .GetRequiredService<ITokenRepositorio>();

        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await DeleteTokensNoConfirmados();
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        async Task DeleteTokensNoConfirmados()
        {
            if (_token != null)
            {
                List<Token> tokensForDelete = await _token.GetTokensForDelete();
                try
                {
                    if (tokensForDelete != null)
                    {
                        foreach (Token token in tokensForDelete)
                        {
                            await _token.DeleteToken(token.Id);
                        }
                        await _serverHub.UpdateTokenList();
                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}



