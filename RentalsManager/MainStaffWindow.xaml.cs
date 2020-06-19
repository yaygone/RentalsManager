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
	/// Interaction logic for MainStaffWindow.xaml
	/// </summary>
	public partial class MainStaffWindow : Window
	{
		public MainStaffWindow()
		{
			SetupSearch();
			InitializeComponent();
			UpdateLists();
		}

		private void SetupSearch()
		{ CollectionViewSource.GetDefaultView(listViewContent.ItemsSource).Filter = SearchFilter; }

		private bool SearchFilter(object item)
		{ return (string.IsNullOrEmpty(searchBox.Text)) || (item).ToString().Contains(searchBox.Text); }

		public void ResetButtonSelection(Button buttonToActivate)
		{
			buttonCustomers.Background = Brushes.LightGray;
			buttonGames.Background = Brushes.LightGray;
			buttonReviews.Background = Brushes.LightGray;
			buttonManagers.Background = Brushes.LightGray;
			buttonToActivate.Background = Brushes.Goldenrod;
		}

		private void UpdateLists()
		{
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
		}

		private void ButtonCustomer_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists();

			GridView customersGrid = new GridView();
			listViewContent.View = customersGrid;
			customersGrid.AllowsColumnReorder = false;

			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Username",
				DisplayMemberBinding = new Binding("username"),
				Width = 80
			});
			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "First name",
				DisplayMemberBinding = new Binding("fname"),
				Width = 120
			});
			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Last name",
				DisplayMemberBinding = new Binding("lname"),
				Width = 120
			});
			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Phone",
				DisplayMemberBinding = new Binding("phone"),
				Width = 120
			});
			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Email",
				DisplayMemberBinding = new Binding("email"),
				Width = 120
			});
			customersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Standing",
				DisplayMemberBinding = new Binding("standing"),
				Width = 120
			});

			listViewContent.ItemsSource = Global.customers;
		}

		private void ButtonGamesTitle_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists();

			GridView gamesGrid = new GridView();
			listViewContent.View = gamesGrid;
			gamesGrid.AllowsColumnReorder = false;

			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Available",
				DisplayMemberBinding = new Binding("avail"),
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
				DisplayMemberBinding = new Binding("minPlayers"),
				Width = 80
			});
			gamesGrid.Columns.Add(new GridViewColumn
			{
				Header = "Year released",
				DisplayMemberBinding = new Binding("releaseYear"),
				Width = 80
			});

			listViewContent.ItemsSource = Global.games;
		}

		private void ButtonReview_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists();

			GridView reviewsGrid = new GridView();
			listViewContent.View = reviewsGrid;
			reviewsGrid.AllowsColumnReorder = false;

			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "Game",
				DisplayMemberBinding = new Binding("boardgame"),
				Width = 120
			});
			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "First name",
				DisplayMemberBinding = new Binding("customer"),
				Width = 120
			});
			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "Date reviewed",
				DisplayMemberBinding = new Binding("dateReviewed"),
				Width = 80
			});
			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "Rating",
				DisplayMemberBinding = new Binding("rating"),
				Width = 80
			});

			listViewContent.ItemsSource = Global.reviews;
		}

		private void ButtonManager_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button) sender);
			UpdateLists();

			GridView managersGrid = new GridView();
			listViewContent.View = managersGrid;
			managersGrid.AllowsColumnReorder = false;

			managersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Username",
				DisplayMemberBinding = new Binding("username"),
				Width = 200
			});

			listViewContent.ItemsSource = Global.managers;
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{ CollectionViewSource.GetDefaultView(listViewContent.ItemsSource).Refresh(); }

		private void ButtonLogout_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			if (buttonGames.Background == Brushes.Goldenrod) new GameDetailWindow(listViewContent.SelectedItem as Game).Show();

		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			if (buttonGames.Background == Brushes.Goldenrod) new GameDetailWindow(null).Show();
			else if (buttonCustomers.Background == Brushes.Goldenrod) new CustomerDetailWindow(null).Show();

		}

		private void ButtonMainAction_Click(object sender, RoutedEventArgs e)
		{
			object selected = listViewContent.SelectedItem;
			if (selected is Game game)
			{
				if (game.avail) new RentalWindow(game, null).Show();
				else
				{
					MessageBoxResult result = MessageBox.Show("Accept this return and mark it as available?", "Confirmation required", MessageBoxButton.YesNo);
					if (result == MessageBoxResult.Yes)
					{
						object[] rentalRecord =
							SQL.GetOutput("SELECT customerUsername FROM Rental WHERE boardgameID = " + game.id)[0];
						string rentalID = (string) rentalRecord[0];
						string customerUsername = (string) rentalRecord[4];
						Customer returnee = 


						SQL.ExecuteQuery("UPDATE Rental SET returnDate = " + DateTime.Today.ToShortDateString() + " WHERE id = " + rentalID);
					}
				}

			}
			else if (selected is Customer customer)
			{
				if (customer.standing < 2) MessageBox.Show("Customer has low standing. Please check and update if needed", "Warning", MessageBoxButton.OK);
				new RentalWindow(null, customer).Show();
			}
			else MessageBox.Show("Please select a game to rent, or a customer to rent to", "Error", MessageBoxButton.OK);
		}
	}
}
