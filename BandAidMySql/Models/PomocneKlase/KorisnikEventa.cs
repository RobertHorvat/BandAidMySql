using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BandAidMySql.Models.PomocneKlase
{
	public class KorisnikEventa
	{
		public int UserId { get; set; }
		public string Name { get; set; }
		public string Email { get; set; }

		public string Adress { get; set; }
		public string EventName { get; set; }
		public string Status { get; set; }

	}
}
