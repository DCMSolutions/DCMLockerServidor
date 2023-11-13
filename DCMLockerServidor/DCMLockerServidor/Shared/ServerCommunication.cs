using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLockerServidor.Shared
{
    public class ServerStatus
    {
        public string NroSerie { get; set; }
        public List<TLockerMap>? Locker { get; set; }
        public DateTime? LastUpdateTime { get; set; }
        public string? Status { get; set; }

    }

    public class ServerConfig
    {
        public string NroSerie { get; set; }


    }

    public class ServerToken
    {
        public string NroSerie { get; set; }
        public string? Locker { get; set; }
        public string? Token { get; set; }

    }
    public class TLockerMap
    {

        public enum EnumLockerType { NORMAL = 0, COOL = 1, TEMP = 2 }
        public int BoxAddr { get; set; }
        /// <summary>
        /// Indica si el Box esta habilitado para usarse
        /// </summary>
        public bool Enable { get; set; }

        /// <summary>
        /// Indica si el Box puede ser entregado a usuarios temporales
        /// </summary>
        public bool IsUserFixed { get; set; }
        /// <summary>
        /// Indica si el sensor esta instalado
        /// </summary>
        public bool IsSensorPresent { get; set; }
        /// <summary>
        ///  Indica si el Box es Normal, Refrigerado o Con control de temperatura
        /// </summary>
        public EnumLockerType LockerType { get; set; }
        /// <summary>
        /// Temperatura Maxima
        /// </summary>
        public int TempMax { get; set; }
        /// <summary>
        /// Temperatura minima
        /// </summary>
        public int TempMin { get; set; }
        /// <summary>
        /// Alarma que se activa si se realiza la apertura del box 
        /// </summary>
        public int AlamrNro { get; set; }
        public string State { get; set; }
        public int Size { get; set; }
        public int Id { get; set; }
    }
}
