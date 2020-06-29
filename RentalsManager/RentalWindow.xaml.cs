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
	/// Interaction logic for RentalWindow.xaml
	/// </summary>
	public partial class RentalWindow : Window
	{
		Game game;
		Customer customer;
		string managerUsername;
		public RentalWindow(Game game, Customer customer, string managerUsername)
		{
			this.game = game;
			this.customer = customer;
			this.managerUsername = managerUsername;
			InitializeComponent();
			PopulateFields();
			TextBoxManagerUsername.IsEnabled = (managerUsername == null);
			PasswordBoxManagerPassword.IsEnabled = (managerUsername == null);
			DatePickerStartDate.IsEnabled = (managerUsername != null);
		}

		public void PopulateFields()
		{
			if (game != null)
			{
				TextBoxGameId.Text = game.id.ToString();
				TextBoxGameId.IsEnabled = false;
				ButtonGameSearch.IsEnabled = false;
			}

			if (customer != null)
			{
				TextBoxCustomerUsername.Text = customer.username;
				TextBoxCustomerUsername.IsEnabled = false;
				ButtonCustomerSearch.IsEnabled = false;
			}

			TextBoxManagerUsername.Text = managerUsername;
			DatePickerStartDate.SelectedDate = DateTime.Today;
		}

		private void ButtonGameSearch_Click(object sender, RoutedEventArgs e)
		{
			Game searchResultGame = findGame(TextBoxGameId.Text);
			if (searchResultGame == null) MessageBox.Show("No game found under that ID/Name.", "Error", MessageBoxButton.OK);
			else TextBoxGameId.Text = searchResultGame.id.ToString();
			this.game = searchResultGame;
		}

		private void ButtonCustomerSearch_Click(object sender, RoutedEventArgs e)
		{
			Customer searchResultCustomer = findCustomer(TextBoxCustomerUsername.Text);
			if (searchResultCustomer == null) MessageBox.Show("No customer found under that name/contact/username.", "Error", MessageBoxButton.OK);
			else TextBoxCustomerUsername.Text = searchResultCustomer.username;
			this.customer = searchResultCustomer;
		}

		private Game findGame(string key)
		{
			foreach (Game game in Global.games)
				if (game.ToShortString().Contains(key)) return game;
			return null;
		}

		private Customer findCustomer(string key)
		{
			foreach (Customer customer in Global.customers)
				if (customer.ToString().Contains(key)) return customer;
			return null;
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			if (managerUsername == null && SQL.GetOutput("SELECT * FROM Manager WHERE username = '" + TextBoxManagerUsername.Text + "' AND password = '" + PasswordBoxManagerPassword.Password + "'").Count == 0)
			{
				MessageBox.Show("Failed to validate manager details", "Error", MessageBoxButton.OK);
				return;
			}
			if (TextBoxGameId.Text.Equals("") || TextBoxCustomerUsername.Text.Equals(""))
			{
				MessageBox.Show("fields missing, please check input data", "Error", MessageBoxButton.OK);
				return;
			}
			if (game == null) game = findGame(TextBoxGameId.Text);
			if (customer == null) customer = findCustomer(TextBoxCustomerUsername.Text);
			if (game == null || customer == null)
			{
				MessageBox.Show("fields missing, please check input data", "Error", MessageBoxButton.OK);
				return;
			}

			SQL.ExecuteQuery("INSERT INTO Rental VALUES ('" + Global.DateToString((DateTime)DatePickerStartDate.SelectedDate) + "', NULL, NULL, '" + customer.username + "', '" + TextBoxManagerUsername.Text + "', " + game.id.ToString() + ")");
			game.avail = false;
			game.UpdateSql();
			customer.maxRentNum--;
			customer.UpdateSql();
			Close();
		}
	}
}
