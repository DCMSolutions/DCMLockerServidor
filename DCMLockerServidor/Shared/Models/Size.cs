using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace DCMLockerServidor.Shared.Models;

public partial class Size
{
    public int Id { get; set; }

    public string? Nombre { get; set; }

    public int? Alto { get; set; }

    public int? Ancho { get; set; }

    public int? Profundidad { get; set; }
    
    [JsonIgnore]
    public virtual ICollection<Box>? Boxes { get; set; } = new List<Box>();
    
    [JsonIgnore]
    public virtual ICollection<Token>? Tokens { get; set; } = new List<Token>();

    }
