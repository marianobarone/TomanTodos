using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    [Table("Movimientos")]
    public class Movimiento
    {
        [Key]
        public Guid Id { get; set; }
        //QUE FECHA
        //public DateTime FechaMovimiento { get; set; }
        [ForeignKey(nameof(Producto))]
        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }
        public List<MovimientoDetalle> MovimientosDetalle { get; set; }
    }
}