﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    [Table("Ventas")]

    public class Venta
    {
        [Key]
        public Guid Id { get; set; }
        public decimal Total { get; set; }
        public List<Producto> Producto { get; set; }
        public DateTime FechaVenta { get; set; }
    }
}