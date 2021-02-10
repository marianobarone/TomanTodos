using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    public class Sucursal
    {
        [Key]
        public Guid Id { get; set; }
        public string Nombre { get; set; }
        public List<Stock> Stock { get; set; }
    }
}