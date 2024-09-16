using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ContactsTest
{
	public class MainData
	{
		public MainData()
        {
			phoneNumber = new List<Phones>();
        }
		public int id { get; set; }
		public string lastName { get; set; }

		public string firstName { get; set; }

		public string patronicalName { get; set; }
		public DateTime birthDate { get; set; }
		public string nickname { get; set; }
		public string comment { get; set; }

		public List<Phones> phoneNumber { get; set; }
	}
}