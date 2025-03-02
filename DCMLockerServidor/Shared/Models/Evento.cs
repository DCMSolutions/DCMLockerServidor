using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace DCMLockerServidor.Shared.Models;

public partial class Evento
{
    [Key]
    public int Id { get; set; }
    [ForeignKey("Locker")]
    public int? IdLocker { get; set; }
    public DateTime? FechaCreacion { get; set; }
    public string? Descripcion { get; set; }
    public string? Identificador { get; set; }

    public virtual Locker? Locker { get; set; }
}
