using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalsManager.Properties;
using System.ComponentModel;

namespace RentalsManager
{
	public abstract class Login
	{
		public string username { get; set; }
		public string password { get; set; }

		public Login(string username, string password)
		{
			this.username = username;
			this.password = password;
		}
	}

	public class Manager : Login
	{
		public Manager(string username, string password) : base(username, password) { }

		public static void AddFromSql(Object[] values)
		{ Global.managers.Add(new Manager((string) values[0], (string) values[1])); }

		public override string ToString()
		{ return username; }
	}

	public class Customer : Login
	{
		public string fname { get; set; }
		public string lname { get; set; }
		public string phone { get; set; }
		public string email { get; set; }
		public string streetAddress { get; set; }
		public string city { get; set; }
		public string bankAccount { get; set; }
		public int goodness { get; set; }
		public DateTime joinDate { get; set; }
		public DateTime dob { get; set; }
		public string gender { get; set; }
		public int maxRentNum { get; set; }

		public Customer(string username, string password, string fname, string lname, string phone, string bankAccount,
			DateTime joinDate) : base(username, password)
		{
			this.fname = fname;
			this.lname = lname;
			this.phone = phone;
			this.bankAccount = bankAccount;
			this.joinDate = joinDate;
			goodness = -1;
			maxRentNum = -1;
		}

		public static void AddFromSql(Object[] values)
		{
			string username = (string)values[0];
			string password = (string)values[3];
			string fname = (string)values[1];
			string lname = (string)values[2];
			string phone = (string)values[4];
			string bankAccount = (string)values[8];
			DateTime joinDate = (DateTime) values[10];
			Customer customer = new Customer(username, password, fname, lname, phone, bankAccount, joinDate);
			customer.email = (string)values[5];
			customer.streetAddress = (string)values[6];
			customer.city = (string)values[7];
			customer.goodness = values[9] is DBNull ? -1 : (int)values[9];
			customer.dob = (DateTime)values[11];
			customer.gender = (string)values[12];
			customer.maxRentNum = (int)values[13];
			Global.customers.Add(customer);
		}

		internal List<Genre> GetGenres()
		{
			List<object[]> genresRaw = SQL.GetOutput("SELECT genreName FROM GenrePref WHERE customerUsername = " + this.username);
			List<Genre> thisCustomerGenres = new List<Genre>();
			if (genresRaw != null)
				foreach (object[] values in genresRaw)
					foreach (Genre genre in Global.genres)
						if (((string)values[0]).Equals(genre.name)) thisCustomerGenres.Add(genre);
			return thisCustomerGenres;
		}

		public void UpdateSql()
		{
			SQL.ExecuteQuery("UPDATE Customer SET password = '" + password + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET fname = '" + fname + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET lname = '" + lname + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET phone = '" + phone + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET email = '" + email + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET streetAddress = '" + streetAddress + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET city = '" + city + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET bankAcct = '" + bankAccount + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET goodness = " + goodness + " WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET joinDate = '" + Global.DateToString(joinDate) + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET dob = '" + Global.DateToString(dob) + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET gender = '" + gender + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET maxRentNum = " + maxRentNum + " WHERE username = '" + username + "'");
		}

		public void InsertSql()
		{
			SQL.ExecuteQuery("INSERT INTO Customer VALUES ('" + username + "', '" + fname + "', '" + lname + "', '" + password + "', '" 
			                 + phone + "', '" + email + "', '" + streetAddress + "', '" + city + "', '" + bankAccount + "', " + goodness + ", '" 
			                 + Global.DateToString(joinDate) + "', '" + Global.DateToString(dob) + "', '" + gender + "', " + maxRentNum + ")");
		}

		public event PropertyChangedEventHandler PropertyChanged;

		public override string ToString()
		{ return fname + ";" + lname + ";" + username + ";" + phone + ";" + email; }
	}
}
