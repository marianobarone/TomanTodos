using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using TomanTodos.Models.TomanTodosModels;

namespace TomanTodos.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Producto> Productos { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Categoria> Categorias { get; set; }
        public DbSet<Movimiento> Movimientos { get; set; }
        public DbSet<StockItem> StockItems { get; set; }
        public DbSet<MovimientoDetalle> MovimientosDetalle { get; set; }
        public DbSet<Venta> Ventas { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }

        //public System.Data.Entity.DbSet<TomanTodos.Models.TomanTodosModels.TipoMovimiento> TipoMovimientoes { get; set; }

        //public System.Data.Entity.DbSet<TomanTodos.Models.TomanTodosModels.Venta> Ventas { get; set; }

        //public System.Data.Entity.DbSet<TomanTodos.Models.TomanTodosModels.Sucursal> Sucursals { get; set; }
    }
}