using API.FurnitureStore.Data;
using API.FurnitureStore.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace API.FurnitureStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientsController : ControllerBase
    {
        private readonly APIFurnitureStoreContext _Context;
        //constructor recibe un parametro del mismo tipo
        public ClientsController(APIFurnitureStoreContext context)
        {



            _Context = context; //variable global
        }
        
        [HttpGet]  //Decorador   //devuelve lista de clientes y la api va a ser asyncrona
        public async Task<IEnumerable<Client>> Get()
        {
            return await _Context.Clients.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetaills(int id) //IActionResult me permite devolver respuesta de tipo Httpresponsive
        {         //consulta de la tabla clientes traer el primero que encuentre que el id sea igual al id que llega por parametros
            var client = await _Context.Clients.FirstOrDefaultAsync(c => c.id == id);

            if (client == null) return NotFound();

            return Ok(client);
        }
        [HttpPost]
        public async Task<IActionResult> Post(Client client)
        {
            await  _Context.Clients.AddAsync(client);
            await _Context.SaveChangesAsync();

            return CreatedAtAction("Post", client.Id, client);
        }
        [HttpPut]
        public async Task<IActionResult> Put(Client client)

        {
            _Context.Clients.Update(client);
            
            await _Context.SaveChangesAsync();

            return NoContent();
        }              //Enpoints
        [HttpDelete]
        public async Task<IActionResult> Delete(Client client)
        {
            if (client == null) return NotFound();
       
            _Context.Clients.Remove(client);
            await _Context.SaveChangesAsync();

            return NoContent();
        }
            
            
            } 



}

