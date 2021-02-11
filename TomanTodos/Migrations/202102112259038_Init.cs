namespace TomanTodos.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Init : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Categorias",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Descripcion = c.String(nullable: false),
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
                        Foto = c.String(),
                        Categoria_Id = c.Guid(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Categorias", t => t.Categoria_Id)
                .Index(t => t.Categoria_Id);
            
            CreateTable(
                "dbo.Movimientoes",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        FechaMovimiento = c.DateTime(nullable: false),
                    })
                .PrimaryKey(t => t.Id);
            
            CreateTable(
                "dbo.MovimientoDetalles",
                c => new
                    {
                        Id = c.Int(nullable: false, identity: true),
                        Cantidad = c.Decimal(nullable: false, precision: 18, scale: 2),
                        ProductoId = c.Guid(nullable: false),
                        SucursalId = c.Guid(nullable: false),
                        Movimiento_Id = c.Int(),
                    })
                .PrimaryKey(t => t.Id)
                .ForeignKey("dbo.Productos", t => t.ProductoId, cascadeDelete: true)
                .ForeignKey("dbo.Sucursals", t => t.SucursalId, cascadeDelete: true)
                .ForeignKey("dbo.Movimientoes", t => t.Movimiento_Id)
                .Index(t => t.ProductoId)
                .Index(t => t.SucursalId)
                .Index(t => t.Movimiento_Id);
            
            CreateTable(
                "dbo.Sucursals",
                c => new
                    {
                        Id = c.Guid(nullable: false),
                        Nombre = c.String(),
                        Direccion = c.String(),
                    })
                .PrimaryKey(t => t.Id);
            
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
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.AspNetUserRoles", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserLogins", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserClaims", "UserId", "dbo.AspNetUsers");
            DropForeignKey("dbo.AspNetUserRoles", "RoleId", "dbo.AspNetRoles");
            DropForeignKey("dbo.MovimientoDetalles", "Movimiento_Id", "dbo.Movimientoes");
            DropForeignKey("dbo.MovimientoDetalles", "SucursalId", "dbo.Sucursals");
            DropForeignKey("dbo.MovimientoDetalles", "ProductoId", "dbo.Productos");
            DropForeignKey("dbo.Productos", "Categoria_Id", "dbo.Categorias");
            DropIndex("dbo.AspNetUserLogins", new[] { "UserId" });
            DropIndex("dbo.AspNetUserClaims", new[] { "UserId" });
            DropIndex("dbo.AspNetUsers", "UserNameIndex");
            DropIndex("dbo.AspNetUserRoles", new[] { "RoleId" });
            DropIndex("dbo.AspNetUserRoles", new[] { "UserId" });
            DropIndex("dbo.AspNetRoles", "RoleNameIndex");
            DropIndex("dbo.MovimientoDetalles", new[] { "Movimiento_Id" });
            DropIndex("dbo.MovimientoDetalles", new[] { "SucursalId" });
            DropIndex("dbo.MovimientoDetalles", new[] { "ProductoId" });
            DropIndex("dbo.Productos", new[] { "Categoria_Id" });
            DropTable("dbo.AspNetUserLogins");
            DropTable("dbo.AspNetUserClaims");
            DropTable("dbo.AspNetUsers");
            DropTable("dbo.AspNetUserRoles");
            DropTable("dbo.AspNetRoles");
            DropTable("dbo.Sucursals");
            DropTable("dbo.MovimientoDetalles");
            DropTable("dbo.Movimientoes");
            DropTable("dbo.Productos");
            DropTable("dbo.Categorias");
        }
    }
}
