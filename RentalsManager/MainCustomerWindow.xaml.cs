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
	/// Interaction logic for MainCustomerWindow.xaml
	/// </summary>
	public partial class MainCustomerWindow : Window
	{
		List<Game> searchedGames = new List<Game>();
		Customer thisCustomer;
		public MainCustomerWindow(string username)
		{
			InitializeComponent();
			UpdateLists("");
			foreach (Customer customer in Global.customers) if (customer.username.Equals(username)) thisCustomer = customer;
			TextBlockTitle.Text = "Logged in as " + username;
			listViewContent.ItemsSource = searchedGames;
			GridView gamesGrid = new GridView();
			this.DataContext = gamesGrid;
			listViewContent.View = gamesGrid;
			gamesGrid.AllowsColumnReorder = false;

			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Availability",
				DisplayMemberBinding = new Binding("inStock"),
				Width = 80
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "ID",
				DisplayMemberBinding = new Binding("id"),
				Width = 80
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Name",
				DisplayMemberBinding = new Binding("name"),
				Width = 120
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Min players",
				DisplayMemberBinding = new Binding("minPlayers"),
				Width = 80
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Max players",
				DisplayMemberBinding = new Binding("maxPlayers"),
				Width = 80
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Year released",
				DisplayMemberBinding = new Binding("releaseYear"),
				Width = 80
			});
		}

		private void UpdateLists(string condition)
		{
			Global.customers.Clear();
			Global.games.Clear();
			Global.reviews.Clear();
			Global.managers.Clear();
			Global.publishers.Clear();
			Global.designers.Clear();
			Global.genres.Clear();
			List<object[]> customersRaw = SQL.GetOutput("SELECT * FROM Customer");
			List<object[]> gamesRaw = SQL.GetOutput("SELECT * FROM Boardgame");
			List<object[]> reviewsRaw = SQL.GetOutput("SELECT * FROM Review");
			List<object[]> managersRaw = SQL.GetOutput("SELECT * FROM Manager");
			List<object[]> publishersRaw = SQL.GetOutput("SELECT * FROM Publisher");
			List<object[]> designersRaw = SQL.GetOutput("SELECT * FROM Designer");
			List<object[]> genresRaw = SQL.GetOutput("SELECT * FROM Genre");
			foreach (object[] values in customersRaw) Customer.AddFromSql(values);
			foreach (object[] values in gamesRaw) Game.AddFromSql(values);
			foreach (object[] values in reviewsRaw) Review.AddFromSql(values);
			foreach (object[] values in managersRaw) Manager.AddFromSql(values);
			foreach (object[] values in publishersRaw) Publisher.AddFromSql(values);
			foreach (object[] values in designersRaw) Designer.AddFromSql(values);
			foreach (object[] values in genresRaw) Genre.AddFromSql(values);
			searchedGames.Clear();
			foreach (Game game in Global.games) searchedGames.Add(game);
			if (condition != "")
				for (int i = 0; i < searchedGames.Count; i++)
					if (!searchedGames[i].ToString().ToLower().Contains(condition))
						searchedGames.RemoveAt(i--);
		}

		private void ButtonMainAction_Click(object sender, RoutedEventArgs e)
		{
			Game selectedGame = listViewContent.SelectedItem as Game;
			// Nothing is selected
			if (selectedGame == null)
				MessageBox.Show("Please selecte a game first", "Error", MessageBoxButton.OK);
			// Game is available for rent
			else if (selectedGame.avail)
				new RentalWindow(selectedGame, thisCustomer, null).ShowDialog();
			// Game is rented but not to this customer, throw unavailable message
			else if (SQL.GetOutput("SELECT * FROM Rental WHERE customerUsername = '" + thisCustomer.username + "' AND boardgameID = " + selectedGame.id).Count == 0)
				MessageBox.Show("This game is unavailable. Please try another one or see a staff member", "Error", MessageBoxButton.OK);
			// Game is currently rented out to this customer, prompt to return procedure
			else
				MessageBox.Show("Please see a staff member to process returns", "Notice", MessageBoxButton.OK);
		}

		private void ButtonDetails_Click(object sender, RoutedEventArgs e)
		{
			if (listViewContent.SelectedItem as Game == null)
				MessageBox.Show("Please select a game to display.", "Error", MessageBoxButton.OK);
			else new GameDetailWindow(listViewContent.SelectedItem as Game, true).ShowDialog();
		}

		private void ButtonReports_Click(object sender, RoutedEventArgs e)
		{ if (listViewContent.SelectedItem != null) new ReportWindow(listViewContent.SelectedItem).ShowDialog(); }

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateLists(searchBox.Text.ToLower());
			if (listViewContent.ItemsSource != null)
				CollectionViewSource.GetDefaultView(listViewContent.ItemsSource).Refresh();
		}

		private void Logout_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonUpdateProfile_Click(object sender, RoutedEventArgs e)
		{ new CustomerDetailWindow(thisCustomer, true).ShowDialog(); }
	}
}
