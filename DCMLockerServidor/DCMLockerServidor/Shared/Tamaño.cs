using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLockerServidor.Shared
{
    public class Tamaño
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Alto { get; set; }
        public int Ancho { get; set; }
        public int Profundidad { get; set; }
    }
    public class TamañoCantidad
    {
        public Tamaño TamañoC { get; set; }
        public int Cantidad { get; set; }
    }
}
