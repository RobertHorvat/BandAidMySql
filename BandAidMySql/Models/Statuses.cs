using System;
using System.Collections.Generic;

namespace BandAidMySql.Models
{
    public partial class Statuses
    {
        public Statuses()
        {
            Events = new HashSet<Events>();
        }

        public int StatusId { get; set; }
        public string Name { get; set; }

        public ICollection<Events> Events { get; set; }
    }
}
