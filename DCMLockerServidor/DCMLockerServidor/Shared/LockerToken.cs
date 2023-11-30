namespace DCMLockerServidor.Shared
{
    public class LockerToken
    {
        public ServerStatus Locker { get; set; }
        public string Token { get; set; }
        public string Box { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public string Modo { get; set; }
        public int Contador { get; set; }
    }
}
