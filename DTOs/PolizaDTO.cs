using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DTOs
{
    public class PolizaDTO
    {
        public string? Id { get; set; }
        public string NumeroPoliza { get; set; }
        public string NombreCliente { get; set; }
        public string IdentificacionCliente { get; set; }
        public DateTime FechaNacimientoCliente { get; set; }
        public DateTime FechaTomaPoliza { get; set; }
        public string Coberturas { get; set; }
        public decimal ValorMaximoCubierto { get; set; }
        public string NombrePlan { get; set; }
        public string CiudadResidenciaCliente { get; set; }
        public string DireccionResidenciaCliente { get; set; }
        public string PlacaAutomotor { get; set; }
        public string ModeloAutomotor { get; set; }
        public bool TieneInspeccion { get; set; }
        public DateTime FechaInicioVigencia { get; set; }
        public DateTime FechaFinVigencia { get; set; }
    }
}
