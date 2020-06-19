using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RentalsManager
{
	class Global
	{
		public static List<Customer> customers = new List<Customer>();
		public static List<Game> games = new List<Game>();
		public static List<Review> reviews = new List<Review>();
		public static List<Manager> managers = new List<Manager>();
		public static List<Designer> designers = new List<Designer>();
		public static List<Publisher> publishers = new List<Publisher>();
		public static List<Genre> genres = new List<Genre>();
		public static DateTime StringToDate(string value)
		{
			string[] joinDateData = value.Split(new char[] { '-' });
			return new DateTime(int.Parse(joinDateData[0]), int.Parse(joinDateData[1]), int.Parse(joinDateData[2]));
		}
	}
}
