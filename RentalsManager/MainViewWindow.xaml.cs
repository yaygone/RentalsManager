using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace RentalsManager
{
	/// <summary>
	/// Interaction logic for MainViewWindow.xaml
	/// </summary>
	public partial class MainViewWindow : Window
	{
		public MainViewWindow()
		{
			InitializeComponent();
			GetCustomers();
		}

		private void GetCustomers()
		{
			List<Customer> customers = new List<Customer>();
			customers.Add(new Customer() { uname = "user1", fname = "Alex", lname = "Anderson", phone = "0210210211", email = "alex@email.com", standing = 2 });
			customers.Add(new Customer() { uname = "user2", fname = "Bob", lname = "Bowers", phone = "0210210212", email = "bob@email.com", standing = 4 });
			customers.Add(new Customer() { uname = "user3", fname = "Cat", lname = "Carlton", phone = "0210210213", email = "cat@email.com", standing = 5 });
			customers.Add(new Customer() { uname = "user4", fname = "Denny", lname = "Davies", phone = "0210210214", email = "denny@email.com", standing = 5 });
			customers.Add(new Customer() { uname = "user5", fname = "Ellie", lname = "Eastman", phone = "0210210215", email = "ellie@email.com", standing = 1 });
			customers.Add(new Customer() { uname = "user6", fname = "Fred", lname = "Farnham", phone = "0210210216", email = "fred@email.com", standing = 4 });
			customers.Add(new Customer() { uname = "user7", fname = "Graham", lname = "Graves", phone = "0210210217", email = "graham@email.com", standing = 5 });
			customers.Add(new Customer() { uname = "user8", fname = "Harriet", lname = "Howard", phone = "0210210218", email = "harriet@email.com", standing = 5 });
			customers.Add(new Customer() { uname = "user9", fname = "Ingrid", lname = "Ivarson", phone = "0210210219", email = "ingrid@email.com", standing = 4 });
			customers.Add(new Customer() { uname = "user0", fname = "John", lname = "Jameson", phone = "0210210220", email = "john@email.com", standing = 3 });
			listViewContent.ItemsSource = customers;
			int[] searchIndexes = { 0, 1, 2, 4, 5, 9 };
			List<string[]> managerList = SQL.GetOutput("SELECT * FROM Customer", searchIndexes);
			foreach (string[] managerRecord in managerList)
			{

			}
		}

		class Customer
		{
			public string uname { get; set; }
			public string fname { get; set; }
			public string lname { get; set; }
			public string phone { get; set; }
			public string email { get; set; }
			public int standing { get; set; }
		}
	}
}
