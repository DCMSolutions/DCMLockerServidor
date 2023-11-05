namespace DCMLockerServidor.Shared
{
    public class LockerToken
    {
        public Locker Locker { get; set; }
        public string Token { get; set; }
        public string Box { get; set; }

        public LockerToken(Locker locker, string token,string box)
        {
            Locker = locker;
            Token = token;
            Box = box;
        }
    }
}
