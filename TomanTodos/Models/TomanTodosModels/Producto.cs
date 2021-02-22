using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TomanTodos.Models.TomanTodosModels
{
    [Table("Productos")]
    public class Producto
    {
        [Key]
        public Guid Id { get; set; }
        [Required]
        [MaxLength(255)]
        public string Nombre { get; set; }
        [Required]
        public decimal Precio { get; set; }

        public decimal Descuento { get; set; }
        public bool Activo { get; set; }
        //public string Foto { get; set; }
        public byte[] Foto { get; set; }
        //public imageUrl MyProperty { get; set; }

        [ForeignKey(nameof(Categoria))]
        public Guid CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public List<StockItem> Stock { get; set; }

        //public List<Venta> Ventas { get; set; }
    }
}