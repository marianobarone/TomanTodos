namespace TomanTodos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Migrations1 : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Productos",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false, maxLength: 255),
                        Precio = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Descuento = c.Decimal(nullable: false, precision: 18, scale: 2),
                        Activo = c.Boolean(nullable: false),
                        Foto = c.Binary(),
                        CategoriaId = c.Guid(nullable: false),
                        Venta_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.CategoriaId, cascadeDelete: true)
                .ForeignKey("dbo.Ventas", t => t.Venta_Id)
                .Index(t => t.CategoriaId)
                .Index(t => t.Venta_Id);
            
            CreateTable(
                "dbo.StockItems",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Cantidad = c.Int(nullable: false),
                        ProductoId = c.Guid(nullable: false),
                        SucursalId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.Sucursales", t => t.SucursalId, cascadeDelete: true)
                .Index(t => t.ProductoId)
                .Index(t => t.SucursalId);
            
            CreateTable(
                "dbo.Sucursales",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nombre = c.String(nullable: false),
                        Direccion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.Movimientos",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        ProductoId = c.Guid(nullable: false),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: true)
                .Index(t => t.ProductoId);
            
            CreateTable(
                "dbo.MovimientoDetalles",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        FechaMovimiento = c.DateTime(nullable: false),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        SucursalId = c.Guid(nullable: false),
                        TipoMovimiento = c.Int(nullable: false),
                        Producto_Id = c.Guid(),
                        Movimiento_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productos", t => t.Producto_Id)
                .ForeignKey("dbo.Sucursales", t => t.SucursalId, cascadeDelete: true)
                .ForeignKey("dbo.Movimientos", t => t.Movimiento_Id)
                .Index(t => t.SucursalId)
                .Index(t => t.Producto_Id)
                .Index(t => t.Movimiento_Id);
            
            CreateTable(
                "dbo.AspNetRoles",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Name = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.Name, unique: true, name: "RoleNameIndex");
            
            CreateTable(
                "dbo.AspNetUserRoles",
                c => new
                    {
                        UserId = c.String(nullable: false, maxLength: 128),
                        RoleId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.UserId, t.RoleId })
                .ForeignKey("dbo.AspNetRoles", t => t.RoleId, cascadeDelete: true)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId)
                .Index(t => t.RoleId);
            
            CreateTable(
                "dbo.AspNetUsers",
                c => new
                    {
                        Id = c.String(nullable: false, maxLength: 128),
                        Email = c.String(maxLength: 256),
                        EmailConfirmed = c.Boolean(nullable: false),
                        PasswordHash = c.String(),
                        SecurityStamp = c.String(),
                        PhoneNumber = c.String(),
                        PhoneNumberConfirmed = c.Boolean(nullable: false),
                        TwoFactorEnabled = c.Boolean(nullable: false),
                        LockoutEndDateUtc = c.DateTime(),
                        LockoutEnabled = c.Boolean(nullable: false),
                        AccessFailedCount = c.Int(nullable: false),
                        UserName = c.String(nullable: false, maxLength: 256),
                    })
                .PrimaryKey(t => t.Id)
                .Index(t => t.UserName, unique: true, name: "UserNameIndex");
            
            CreateTable(
                "dbo.AspNetUserClaims",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        UserId = c.String(nullable: false, maxLength: 128),
                        ClaimType = c.String(),
                        ClaimValue = c.String(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.AspNetUserLogins",
                c => new
                    {
                        LoginProvider = c.String(nullable: false, maxLength: 128),
                        ProviderKey = c.String(nullable: false, maxLength: 128),
                        UserId = c.String(nullable: false, maxLength: 128),
                    })
                .PrimaryKey(t => new { t.LoginProvider, t.ProviderKey, t.UserId })
                .ForeignKey("dbo.AspNetUsers", t => t.UserId, cascadeDelete: true)
                .Index(t => t.UserId);
            
            CreateTable(
                "dbo.Ventas",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Total = c.Decimal(nullable: false, precision: 18, scale: 2),
                        FechaVenta = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Productos", "Venta_Id", "dbo.Ventas");
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.Movimientos", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.MovimientoDetalles", "Movimiento_Id", "dbo.Movimientos");
            DropForeignKey("dbo.MovimientoDetalles", "SucursalId", "dbo.Sucursales");
            DropForeignKey("dbo.MovimientoDetalles", "Producto_Id", "dbo.Productos");
            DropForeignKey("dbo.StockItems", "SucursalId", "dbo.Sucursales");
            DropForeignKey("dbo.StockItems", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Productos", "CategoriaId", "dbo.Categorias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MovimientoDetalles", new[] { "Movimiento_Id" });
            DropIndex("dbo.MovimientoDetalles", new[] { "Producto_Id" });
            DropIndex("dbo.MovimientoDetalles", new[] { "SucursalId" });
            DropIndex("dbo.Movimientos", new[] { "ProductoId" });
            DropIndex("dbo.StockItems", new[] { "SucursalId" });
            DropIndex("dbo.StockItems", new[] { "ProductoId" });
            DropIndex("dbo.Productos", new[] { "Venta_Id" });
            DropIndex("dbo.Productos", new[] { "CategoriaId" });
            DropTable("dbo.Ventas");
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.MovimientoDetalles");
            DropTable("dbo.Movimientos");
            DropTable("dbo.Sucursales");
            DropTable("dbo.StockItems");
            DropTable("dbo.Productos");
            DropTable("dbo.Categorias");
        }
    }
}
