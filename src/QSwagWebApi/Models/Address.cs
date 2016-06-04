using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QSwagWebApi.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public AddressType Type { get; set; }
        
    }
}
