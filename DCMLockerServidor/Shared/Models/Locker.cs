using System;
using System.Collections.Generic;

namespace DCMLockerServidor.Shared.Models;

public partial class Locker
{
    public int Id { get; set; }

    public string? NroSerieLocker { get; set; }
    
    public int? Empresa { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public string? Status { get; set; }
    public string? Version { get; set; }
    public string? IP { get; set; }
    public string? EstadoCerraduras { get; set; }

    public virtual ICollection<Box>? Boxes { get; set; } = new List<Box>();

    public virtual Empresa? EmpresaNavigation { get; set; }

    public virtual ICollection<Token>? Tokens { get; set; } = new List<Token>();

    public virtual ICollection<Evento>? Eventos { get; set; } = new List<Evento>();
}
