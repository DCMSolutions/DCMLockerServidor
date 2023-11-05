
namespace DCMLockerServidor.Shared
{
    public class Locker
    {
        public string IpLocker { get; set; }
        public int IdServer { get; set; }
        public int NumeroCorrespondiente { get; set; }

        public Locker(string ipLocker, int idServer, int numeroCorrespondiente)
        {
            IpLocker = ipLocker;
            IdServer = idServer;
            NumeroCorrespondiente = numeroCorrespondiente;
        }
    }
}
