using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Cliente
{
    public int IdCliente { get; set; }

    public string? NumeroIdentificacion { get; set; }

    public string? NombreApellidos { get; set; }

    public DateTime? FechaRegistro { get; set; }
    
}
