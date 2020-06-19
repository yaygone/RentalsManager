using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RentalsManager.Properties;

namespace RentalsManager
{
	public class Login
	{
		internal string username { get; set; }
		internal string password { get; set; }

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
		{
			return username;
		}
	}

	public class Customer : Login
	{
		internal string fname { get; set; }
		internal string lname { get; set; }
		internal string phone { get; set; }
		internal string email { get; set; }
		internal string streetAddress { get; set; }
		internal string city { get; set; }
		internal string bankAccount { get; set; }
		internal int standing { get; set; }
		internal DateTime joinDate { get; set; }
		internal DateTime dob { get; set; }
		internal string gender { get; set; }
		internal int maxRentNum { get; set; }

		public Customer(string username, string password, string fname, string lname, string phone, string bankAccount,
			DateTime joinDate) : base(username, password)
		{
			this.fname = fname;
			this.lname = lname;
			this.phone = phone;
			this.bankAccount = bankAccount;
			this.joinDate = joinDate;
			standing = -1;
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
			DateTime joinDate = Global.StringToDate((string)values[10]);
			Customer customer = new Customer(username, password, fname, lname, phone, bankAccount, joinDate);
			customer.email = (string)values[5];
			customer.streetAddress = (string)values[6];
			customer.city = (string)values[7];
			customer.standing = (int)values[9];
			customer.dob = Global.StringToDate((string)values[11]);
			customer.gender = (string)values[12];
			customer.maxRentNum = (int)values[13];
			Global.customers.Add(customer);
		}

		internal List<Genre> GetGenres()
		{
			List<object[]> genresRaw = SQL.GetOutput("SELECT genreName FROM GenrePref WHERE customerUsername = " + this.username);
			List<Genre> thisCustomerGenres = new List<Genre>();
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
			SQL.ExecuteQuery("UPDATE Customer SET goodness = " + standing + " WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET joinDate = '" + joinDate.ToShortDateString() + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET dob = '" + dob.ToShortDateString() + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET gender = '" + gender + "' WHERE username = '" + username + "'");
			SQL.ExecuteQuery("UPDATE Customer SET maxRentNum = " + maxRentNum + " WHERE username = '" + username + "'");
		}

		public void InsertSql()
		{
			SQL.ExecuteQuery("INSERT INTO Customer VALUES ('" + username + "', '" + fname + "', '" + lname + "', '" + password + "', '" 
			                 + phone + "', '" + email + "', '" + streetAddress + "', '" + city + "', '" + bankAccount + "', " + standing + ", '" 
			                 + joinDate.ToShortDateString() + "', '" + dob.ToShortDateString() + "', '" + gender + "', " + maxRentNum + ")");
		}

		public override string ToString()
		{ return fname + ";" + lname + ";" + username + ";" + phone + ";" + email; }
	}
}
