using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurnitureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _context; //Agrega el context para poder usarlo

        public OrdersController(APIFurnitureStoreContext context)
        {
            _context = context;
        }
        [HttpGet] //decorador / es un Endpoint
        public async Task<IEnumerable<Order>> Get()

        {                                             //es la lista que se agrego a la clase order
            return await _context.Orders.Include(o => o.OrderDetails).ToListAsync();
        }   //Entity framework se va a dar cuenta solo con el echo de decir que incluya la lista busca el dbset OrderaDetails y lo va a incluir al de Orders va ahacer el join automaticamente y me devuelve todo el data ordenes y eñ detalle 

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetails(int id)
       { 
                         //Incluye para que se asegure de incluir los detalles    que el id sea igual igual al parametro
            var order = await _context.Orders.Include (o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == id);

          if (order == null) return NotFound();
        
           return Ok(order);
        }

        [HttpPost]       // La siguiente línea de código es la declaración de un método en un controlador de ASP.NET Core:
                         // public async Task<IActionResult> Post(Order order)

        // `public` es un modificador de acceso que indica que el método es accesible desde fuera de la clase
        // en la que se encuentra. En el contexto de un controlador ASP.NET, esto significa que el método
        // puede ser llamado como parte de una solicitud HTTP.

        // `async` significa que el método es asíncrono, lo que permite realizar operaciones de entrada y salida
        // de manera no bloqueante. Esto permite que el método utilice la palabra clave `await` para esperar a
        // que se completen tareas asíncronas sin bloquear el hilo principal.

        // `Task<IActionResult>` es el tipo de retorno del método. `Task` representa una operación que puede
        // completarse en el futuro, y `IActionResult` es una interfaz que representa el resultado de una
        // acción en un controlador ASP.NET. Puede ser cualquier cosa que derive de `IActionResult`, como
        // `ViewResult`, `JsonResult`, `RedirectResult`, etc.

        // `Post` es el nombre del método. En ASP.NET Core, los métodos en los controladores suelen estar
        // asociados con los diferentes verbos HTTP. `Post` sugiere que este método maneja solicitudes HTTP POST.

        // `Order order` es el parámetro del método. `Order` es el tipo del parámetro que se espera recibir.
        // En el contexto de un controlador ASP.NET, esto generalmente se usa para recibir datos que se
        // envían en el cuerpo de la solicitud HTTP POST. `Order` sería una clase que representa el modelo
        // de datos para la orden que el cliente está enviando.
        public async Task<IActionResult> Post(Order order)
        {
            if (order.OrderDetails == null) 
                return BadRequest("El pedido debe tener al menos un detalle");
         
           await _context.Orders.AddAsync(order);  //se inserta la orden
           await _context.OrderDetails.AddRangeAsync(order.OrderDetails); //se insertan todos los detalles con AddRangeAsync
         
           await _context.SaveChangesAsync(); //guarda los detalles de las ordenes


            return CreatedAtAction("Post",  order.Id, order);
             
        
        }

        [HttpPut]
        public async Task<IActionResult> Put(Order order) //Firma del metodo

        {
                                                      
            if (order == null) return NotFound(); // validaciones basicas order no puede estar vacio por que es un metodo de persistencia
            if (order.Id <= 0) return NotFound();  

            //busca la orden que tenemos actualmente en la base de datos
           // incluyendo todos sus detalles
            var existingOrder = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == order.Id);// busca la orden que tenemos en la base de datos incluyendo todos sus detalles
            if (existingOrder == null) return NotFound(); //el  ide de order no puede ser 0 ni menor a 0


            //Maestro actualización de Orders
            existingOrder.OrdenNumber = order.OrdenNumber; //es la orden que esta en la base de datos //lo que se esta haciendo es actualizar la orden que esta en la base de dato por la orden que viene por parametros
            existingOrder.OrderDate = order.OrderDate;
            existingOrder.DeliveryDate = order.DeliveryDate;
            existingOrder.ClienId = order.ClienId;

             //se eliminan todos los detalles
            _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            
             // le digo a Entity framework que los datos que actualice del maestro los tiene que reemplazar en la base d datos
            _context.Orders.Update(existingOrder);  // aqui le estoy diciendo a Entityframword que los datos que actulice en el maestro los tengo que reemplazar por los que estan en la base de datos
            
             // luego le digo que agregue todos los detalles que llegaron por parametros que son los nuevos detalles
            _context.OrderDetails.AddRange(order.OrderDetails);

           
            //Finalemente le digo que guarde los cambios
            await _context.SaveChangesAsync();
            
            return NoContent();
        }

            [HttpDelete]
         
           public async Task<IActionResult> Delete(Order order)

        {
            if (order == null) return NotFound();

            var existingOrder = await _context.Orders.Include(o => o.OrderDetails).FirstOrDefaultAsync(o => o.Id == order.Id);
            //busca en la base de datos la orden que tenga existente 
             
            if (existingOrder == null) return NotFound();  
             _context.OrderDetails.RemoveRange(existingOrder.OrderDetails);
            _context.Orders.Remove(existingOrder);

            await _context.SaveChangesAsync();  

            return NoContent();
        
         
        } 
            
    
    }

                
}
