using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    [Table("MovimientoDetalles")]
    public class MovimientoDetalle
    {
        [Key]
        public Guid Id { get; set; }
        public DateTime FechaMovimiento { get; set; }
        public decimal Cantidad { get; set; }

        //[ForeignKey(nameof(Producto))]
        //public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }

        [ForeignKey(nameof(Sucursal))]
        public Guid SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }

        public TipoMovimiento TipoMovimiento { get; set; }
    }
}