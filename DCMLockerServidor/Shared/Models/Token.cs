using DCMLockerServidor.Shared.Models;
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DCMLockerServidor.Shared.Models;

public partial class Token
{
    public int Id { get; set; }

    public int? IdLocker { get; set; }

    public int? IdSize { get; set; }

    public int? IdBox { get; set; }

    public string? Token1 { get; set; }

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaInicio { get; set; }

    public DateTime? FechaFin { get; set; }

    public int? Contador { get; set; }

    public int? Cantidad { get; set; }

    public bool? Confirmado { get; set; }

    public string? Modo { get; set; }

    public virtual Box? IdBoxNavigation { get; set; }

    public virtual Locker? IdLockerNavigation { get; set; }

    public virtual Size? IdSizeNavigation { get; set; }
}
