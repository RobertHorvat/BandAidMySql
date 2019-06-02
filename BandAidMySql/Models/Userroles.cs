using System;
using System.Collections.Generic;

namespace BandAidMySql.Models
{
    public partial class Userroles
    {
        public Userroles()
        {
            Users = new HashSet<Users>();
        }

        public int RoleId { get; set; }
        public string Name { get; set; }

        public ICollection<Users> Users { get; set; }
    }
}
