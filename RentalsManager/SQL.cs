using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Windows.Controls;

namespace RentalsManager
{
	class SQL
	{
		public static SqlConnection connection = new SqlConnection(@"Data Source = CLOONEY\SQLEXPRESS; Initial Catalog = BoardgameRentalRecords; Integrated Security = True");
		public static SqlCommand command = new SqlCommand();
		public static SqlDataReader reader;

		private static void ConnectAndSetQuery(string query)
		{
			connection.Close();
			command.Connection = connection;
			connection.Open();
			command.CommandText = query;
		}

		public static void ExecuteQuery(string query)
		{
			ConnectAndSetQuery(query);
			try { command.ExecuteNonQuery(); }
			catch (Exception e) { Console.WriteLine(e); }
		}

		public static List<Object[]> GetOutput(string query)
		{
			Console.WriteLine("Getting output from query: " + query);
			List<Object[]> output = new List<Object[]>();

			ConnectAndSetQuery(query);
			try { reader = command.ExecuteReader(); }
			catch (Exception e) { Console.WriteLine(e); return null; }

			while (SQL.reader.Read())
			{
				Object[] row = new object[reader.FieldCount];
				reader.GetValues(row);
				foreach (Object o in row)
					Console.WriteLine(o);
				output.Add(row);
			}
			return output;
		}
	}
}
