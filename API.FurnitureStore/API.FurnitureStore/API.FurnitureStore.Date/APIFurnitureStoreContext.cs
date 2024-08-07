using API.FurnitureStore.Shared;
using Microsoft.EntityFrameworkCore;


namespace API.FurnitureStore.Data    
{
    public class APIFurnitureStoreContext : DbContext   //la clase context hereda de Dbcontext que es el contexto de Entity framework
    {                     //constructor que recibe options            Le estoy diciendo que llame al contructor de la clase Dbcontext de la cual hereda y también le pase options
        public APIFurnitureStoreContext(DbContextOptions options) : base(options) { }  //son las opciones que vamos a usar desde afuera para configurar el context se hace luego en program es para inyectar la dependencia de Entity framework con nuestra configuración particular

        public DbSet<Client> Clients  { get; set; }   //estas son tablas
        public DbSet<Product> Products  { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet <ProductCategory> productCategories { get; set; }
     
        public DbSet<OrderDetail> OrderDetails { get; set; }  //Estableciendo la relación entre los modelos
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite();    // nos permite que en lugar de usar la configuración estandar, use la configuración de sql que tengamos
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder) //recibe el parametro Modelbuilder
        {
            base.OnModelCreating(modelBuilder); // y llama a la clase Base


            modelBuilder.Entity<OrderDetail>() //estañbleciendo que la entidad OrderDetail tiene que tener una clave
               .HasKey(od => new { od.OrderId, od.ProductId }); // haskey tiene que tener esta clave y va a ser un elemento nuevo compuesto por Id de la orden y el producto de la orden
                   
        }
    }

 
    }


