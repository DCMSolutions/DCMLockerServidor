
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DCMLockerServidor.Shared.Models;

public partial class Box
{
    public int Id { get; set; }

    public int? IdFisico { get; set; }

    public int IdLocker { get; set; }

    public int? IdSize { get; set; }

    public int? Box1 { get; set; }

    public bool? Puerta { get; set; }

    public bool? Ocupacion { get; set; }

    public bool? Libre { get; set; }

    public DateTime? LastUpdateTime { get; set; }

    public string? Status { get; set; }

    public bool? Enable { get; set; }

    public virtual Locker IdLockerNavigation { get; set; } = null!;

    public virtual Size? IdSizeNavigation { get; set; }

    public virtual ICollection<Token> Tokens { get; set; } = new List<Token>();
}
