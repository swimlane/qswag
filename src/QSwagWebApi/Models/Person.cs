#region Using

using System.Collections.Generic;

#endregion

namespace QSwagWebApi.Models
{
    public class Person
    {
        public string FirsName { get; set; }
        public string LastName { get; set; }
        public List<string> Phone { get; set; }
        public List<Address> Addresses { get; set; }
    }
}