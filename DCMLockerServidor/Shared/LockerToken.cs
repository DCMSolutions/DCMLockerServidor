namespace DCMLockerServidor.Shared
{
    public class LockerToken
    {
        public int Id { get; set; }
        public ServerStatus Locker { get; set; }
        public int Token { get; set; }
        public int? Box { get; set; }
        public Tamaño? Tamaño { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }
        public DateTime FechaCreacion { get; set; }
        public string? Modo { get; set; }
        public int Contador { get; set; }
        public bool Confirmado { get; set; }
    }
}
