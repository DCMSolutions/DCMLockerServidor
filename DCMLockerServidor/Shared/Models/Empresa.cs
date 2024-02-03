using System;
using System.Collections.Generic;

namespace DCMLockerServidor.Shared.Models;

public partial class Empresa
{
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public sbyte? Active { get; set; }
    public string? TokenEmpresa { get; set; }
    public virtual ICollection<Locker> Lockers { get; set; } = new List<Locker>();
}

