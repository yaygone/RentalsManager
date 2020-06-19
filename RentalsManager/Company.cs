using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	class Company
	{
		internal string name { get; set; }
		public Company(string name)
		{ this.name = name; }
	}

	class Publisher : Company
	{
		public Publisher(string name) : base(name) { }

		public static void AddFromSql(object[] values)
		{ Global.publishers.Add(new Publisher((string) values[0])); }

		public override string ToString()
		{
			return name;
		}
	}

	class Designer : Company
	{
		public Designer(string name) : base(name) { }
		public static void AddFromSql(object[] values)
		{ Global.designers.Add(new Designer((string)values[0])); }

		public override string ToString()
		{
			return name;
		}
	}
}
