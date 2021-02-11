using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    public class Movimiento
    {
        public int Id { get; set; }
        //QUE FECHA
        public DateTime FechaMovimiento { get; set; }
        public List<MovimientoDetalle> MovimientosDetalle { get; set; }
    }
}