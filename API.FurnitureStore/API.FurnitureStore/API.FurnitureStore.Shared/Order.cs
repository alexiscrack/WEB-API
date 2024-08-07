using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace API.FurnitureStore.Shared
{
    public class Order
    
    {
        public int Id { get; set; } //Elemento autoincrmentable que va tener la tabla ordens
        public int OrdenNumber { get; set; }  //opcion para que pueda ser agregado

        public int ClienId { get; set; }  //ara establecer la relación entre clienrtes y ordenes

        public DateTime OrderDate { get; set; }
        public DateTime DeliveryDate { get; set; }
    
        public List<OrderDetail> OrderDetails { get; set; }
       
    }
}
