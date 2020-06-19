using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	class Genre
	{
		internal string name { get; set; }

		public Genre(string name)
		{ this.name = name; }

		public static void AddFromSql(object[] values)
		{ Global.genres.Add(new Genre((string) values[0])); }

		public override string ToString()
		{ return name; }
	}
}
