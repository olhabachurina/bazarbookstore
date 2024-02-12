﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public bool Shipped { get; set; }
        public int BookId { get; set; } 
        public DateTime OrderDate { get; set; }
        public virtual ICollection<OrderLine> Lines { get; set; }

        public override string ToString()
        {
            return String.Format("CustomerName - {0}\nCity - {1}\nAddress - {2}\nShipped - {3}.", CustomerName, City, Address, Shipped);
        }
    }

}
