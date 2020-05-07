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

		public static List<string[]> GetOutput(string query, int[] indexes)
		{
			Console.WriteLine("Getting output from query: " + query);
			List<string[]> output = new List<string[]>();

			ConnectAndSetQuery(query);
			try { reader = command.ExecuteReader(); }
			catch (Exception e) { Console.WriteLine(e); return null; }

			while (SQL.reader.Read())
			{
				string[] entry = new string[indexes.Length];
				for (int i = 0; i < entry.Length; i++)
					entry[i] = reader[i].ToString();
				output.Add(entry);
			}

			return output;
		}
	}
}
