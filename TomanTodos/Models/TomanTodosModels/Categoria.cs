﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    public class Categoria
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        public string Descripcion { get; set; }
        public List<Producto> Productos { get; set; }
    }
}