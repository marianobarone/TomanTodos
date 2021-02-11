using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    public class MovimientoDetalle
    {
        public int Id { get; set; }

        public decimal Cantidad { get; set; }

        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }

        public Guid SucursalId { get; set; }
        public Sucursal Sucursal { get; set; }
    }
}