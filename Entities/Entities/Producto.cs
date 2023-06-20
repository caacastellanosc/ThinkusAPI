using System;
using System.Collections.Generic;

namespace Domain.Entities;

public partial class Producto
{
    public int IdProducto { get; set; }

    public string? CodigoProducto { get; set; }

    public string? Nombre { get; set; }

    public int? Stock { get; set; }

    public decimal? Precio { get; set; }

    public bool? EsActivo { get; set; }

    public DateTime? FechaRegistro { get; set; }

    
}
