using DCMLockerServidor.Shared.Models;
using System;
using System.Collections.Generic;

namespace DCMLockerServidor.Shared.Models;

public partial class Size
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public string? Alto { get; set; }

    public string? Ancho { get; set; }

    public string? Profundidad { get; set; }

    public int? IdLocker { get; set; }

    public virtual ICollection<Box> Boxes { get; set; } = new List<Box>();

    public virtual Locker IdNavigation { get; set; } = null!;

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
