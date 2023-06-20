using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Venta
{
    public int IdVenta { get; set; }

    public string? NumeroDocumento { get; set; }

    public string? TipoPago { get; set; }

    public DateTime? FechaRegistro { get; set; }

    public decimal? Total { get; set; }

    public int? IdCliente { get; set; }

    public int? IdUsuario { get; set; }

    public bool? EsActiva { get; set; }

    public virtual ICollection<DetalleVenta> DetalleVenta { get; set; } = new List<DetalleVenta>();

    public virtual Cliente? IdClienteNavigation { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }
}
