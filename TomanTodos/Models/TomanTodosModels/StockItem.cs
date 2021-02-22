using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    [Table("StockItems")]

    public class StockItem
    {
        [Key]
        public Guid Id { get; set; }
        public int Cantidad { get; set; }

        public Guid ProductoId { get; set; }
        public Producto Producto { get; set; }

        public Sucursal Sucursal { get; set; }
        public Guid SucursalId { get; set; }

    }
}