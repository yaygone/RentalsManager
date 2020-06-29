using System;
using System.Collections.Generic;
using System.ComponentModel;
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
		private const string GAME_BUTTON_TEXT = "Rent/Return game";
		private const string CUST_BUTTON_TEXT = "Rent to customer";
		private const string REVIEW_BUTTON_TEXT = "Remove review";
		private const string MANAGER_BUTTON_TEXT = "";
		private string loggedInManager;
		List<Customer> searchedCustomers = new List<Customer>();
		List<Game> searchedGames = new List<Game>();
		List<Review> searchedReviews = new List<Review>();
		List<Manager> searchedManagers = new List<Manager>();

		public MainStaffWindow(string managerUsername)
		{
			loggedInManager = managerUsername;
			InitializeComponent();
			UpdateLists("");
			listViewContent.ItemsSource = searchedCustomers;
			TextBlockTitle.Text = "Logged in as " + loggedInManager;
			ButtonCustomer_Click(buttonCustomers, null);
		}

		public void ResetButtonSelection(Button buttonToActivate)
		{
			buttonCustomers.Background = Brushes.LightGray;
			buttonGames.Background = Brushes.LightGray;
			buttonReviews.Background = Brushes.LightGray;
			buttonManagers.Background = Brushes.LightGray;
			buttonToActivate.Background = Brushes.Goldenrod;
			ButtonMainAction.IsEnabled = true;
			buttonReports.IsEnabled = true;
			searchBox.Text = "";
			if (buttonToActivate == buttonCustomers) ButtonMainAction.Content = CUST_BUTTON_TEXT;
			else if (buttonToActivate == buttonGames) ButtonMainAction.Content = GAME_BUTTON_TEXT;
			else if (buttonToActivate == buttonReviews)
			{
				ButtonMainAction.Content = REVIEW_BUTTON_TEXT;
				buttonReports.IsEnabled = false;
			}
			else
			{
				ButtonMainAction.Content = MANAGER_BUTTON_TEXT;
				ButtonMainAction.IsEnabled = false;
			}
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
			searchedCustomers.Clear();
			searchedGames.Clear();
			searchedReviews.Clear();
			searchedManagers.Clear();
			foreach (Customer customer in Global.customers) searchedCustomers.Add(customer);
			foreach (Game game in Global.games) searchedGames.Add(game);
			foreach (Review review in Global.reviews) searchedReviews.Add(review);
			foreach (Manager manager in Global.managers) searchedManagers.Add(manager);
			if (condition != "")
			{
				if (ButtonMainAction.Content == CUST_BUTTON_TEXT)
				{
					for (int i = 0; i < searchedCustomers.Count; i++)
						if (!searchedCustomers[i].ToString().ToLower().Contains(condition))
							searchedCustomers.RemoveAt(i--);
				}
				else if (ButtonMainAction.Content == GAME_BUTTON_TEXT)
				{
					for (int i = 0; i < searchedGames.Count; i++)
						if (!searchedGames[i].ToString().ToLower().Contains(condition))
							searchedGames.RemoveAt(i--);
				}
				else if (ButtonMainAction.Content == REVIEW_BUTTON_TEXT)
				{
					for (int i = 0; i < searchedReviews.Count; i++)
						if (!searchedReviews[i].ToString().ToLower().Contains(condition))
							searchedReviews.RemoveAt(i--);
				}
				else
					for (int i = 0; i < searchedManagers.Count; i++)
						if (!searchedManagers[i].ToString().ToLower().Contains(condition))
							searchedManagers.RemoveAt(i--);
			}
		}

		private void ButtonCustomer_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists(searchBox.Text.ToLower());
			listViewContent.ItemsSource = searchedCustomers;
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
				DisplayMemberBinding = new Binding("goodness"),
				Width = 120
			});
		}

		private void ButtonGamesTitle_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists(searchBox.Text.ToLower());

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

		private void ButtonReview_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button)sender);
			UpdateLists(searchBox.Text.ToLower());

			listViewContent.ItemsSource = searchedReviews;
			GridView reviewsGrid = new GridView();
			this.DataContext = reviewsGrid;
			listViewContent.View = reviewsGrid;
			reviewsGrid.AllowsColumnReorder = false;

			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "Game",
				DisplayMemberBinding = new Binding("boardgameName"),
				Width = 120
			});
			reviewsGrid.Columns.Add(new GridViewColumn
			{
				Header = "Customer name",
				DisplayMemberBinding = new Binding("customerUsername"),
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
		}

		private void ButtonManager_Click(object sender, RoutedEventArgs e)
		{
			ResetButtonSelection((Button) sender);
			UpdateLists(searchBox.Text.ToLower());

			listViewContent.ItemsSource = searchedManagers;
			GridView managersGrid = new GridView();
			this.DataContext = managersGrid;
			listViewContent.View = managersGrid;
			managersGrid.AllowsColumnReorder = false;

			managersGrid.Columns.Add(new GridViewColumn
			{
				Header = "Username",
				DisplayMemberBinding = new Binding("username"),
				Width = 200
			});
		}

		private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
		{
			UpdateLists(searchBox.Text.ToLower());
			if (listViewContent.ItemsSource != null)
				CollectionViewSource.GetDefaultView(listViewContent.ItemsSource).Refresh();
		}

		private void ButtonLogout_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonEdit_Click(object sender, RoutedEventArgs e)
		{
			if (ButtonMainAction.Content.Equals(CUST_BUTTON_TEXT)) new CustomerDetailWindow(listViewContent.SelectedItem as Customer, false).ShowDialog();
			else if (ButtonMainAction.Content.Equals(REVIEW_BUTTON_TEXT)) new ReviewDetailWindow(listViewContent.SelectedItem as Review, false).ShowDialog();
			else if (ButtonMainAction.Content.Equals(MANAGER_BUTTON_TEXT)) new ManagerDetailWindow(listViewContent.SelectedItem as Manager).ShowDialog();
			else new GameDetailWindow(listViewContent.SelectedItem as Game, false).ShowDialog();
		}

		private void ButtonAdd_Click(object sender, RoutedEventArgs e)
		{
			if (ButtonMainAction.Content.Equals(GAME_BUTTON_TEXT)) new GameDetailWindow(null, false).ShowDialog();
			else if (ButtonMainAction.Content.Equals(CUST_BUTTON_TEXT)) new CustomerDetailWindow(null, false).ShowDialog();
			else if (ButtonMainAction.Content.Equals(REVIEW_BUTTON_TEXT)) new ReviewDetailWindow(null, false).ShowDialog();
			else if (ButtonMainAction.Content.Equals(MANAGER_BUTTON_TEXT)) new ManagerDetailWindow(null).ShowDialog();
		}

		private void ButtonMainAction_Click(object sender, RoutedEventArgs e)
		{
			var selected = listViewContent.SelectedItem;
			try
			{
				if (selected is Game game)
				{
					if (game.avail) new RentalWindow(game, null, loggedInManager).ShowDialog();
					else new ReturnAssessmentWindow(game).ShowDialog();
				}
				else if (selected is Customer customer)
				{
					if (customer.maxRentNum <= 0 && MessageBox.Show("Maximum concurrent rental count met. Manager override?", "Warning", MessageBoxButton.OKCancel) == MessageBoxResult.Cancel) return;
					if (customer.goodness == 1) MessageBox.Show("Customer has low standing. Please check and update if needed", "Warning", MessageBoxButton.OK);
					new RentalWindow(null, customer, loggedInManager).ShowDialog();
				}
				else if (selected is Review review)
					if (MessageBox.Show("This cannot be undone. Are you sure you want to do this?", "Warning", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
						for (int i = 0; i < Global.reviews.Count; i++)
							if (Global.reviews[i] == review)
								SQL.ExecuteQuery("DELETE FROM Review WHERE customerUsername = '" + review.customer.username + "' AND boardgameID = '" + review.boardgame.id + "'");
			}
			catch (Exception exception)
			{
				MessageBox.Show("Please select a game to rent, or a customer to rent to", "Error", MessageBoxButton.OK);
				Console.WriteLine(exception.StackTrace);
			}
		}

		private void ButtonReports_Click(object sender, RoutedEventArgs e)
		{ if (listViewContent.SelectedItem != null) new ReportWindow(listViewContent.SelectedItem).ShowDialog(); }

		private void ButtonMonthlyStats_Click(object sender, RoutedEventArgs e)
		{
			int currMonth = DateTime.Today.Month;
			int prevMonth = (currMonth == 1) ? 12 : currMonth - 1;

			string outputString = "";
			List<object[]> result;

			outputString += "STATS FOR CURRENT MONTH\n\n";

			outputString += "Boardgame popularity:\n";
			result = SQL.GetOutput("SELECT Boardgame.name, 'Times rented' = COUNT(Rental.boardgameID) FROM Boardgame, Rental WHERE Boardgame.id = Rental.boardgameID AND MONTH(Rental.startDate) = " + currMonth + " GROUP BY Boardgame.name ORDER BY 'Times rented' DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + ", rented " + ((int)values[1]).ToString() + " times\n";
			outputString += "\n";

			outputString += "Highest rated review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MAX(Review.rating) FROM Review WHERE MONTH(Review.dateReviewed) = " + currMonth + ")");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Most critical review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, Review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MIN(Review.rating) FROM Review WHERE MONTH(Review.dateReviewed) = " + currMonth + ")");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Number of rentals per customer:\n";
			result = SQL.GetOutput("SELECT Rental.customerUsername, COUNT(Rental.id) FROM Rental WHERE MONTH(Rental.startDate) = " + currMonth + " GROUP BY Rental.customerUsername ORDER BY COUNT(Rental.id) DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + " rented " + ((int)values[1]).ToString() + " games\n";
			outputString += "\n";

			currMonth = (currMonth == 1) ? 12 : currMonth - 1;

			outputString += "\nSTATS FOR PREVIOUS MONTH\n\n";

			outputString += "Boardgame popularity:\n";
			result = SQL.GetOutput("SELECT Boardgame.name, 'Times rented' = COUNT(Rental.boardgameID) FROM Boardgame, Rental WHERE Boardgame.id = Rental.boardgameID AND MONTH(Rental.startDate) = " + currMonth + " GROUP BY Boardgame.name ORDER BY 'Times rented' DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + ", rented " + ((int)values[1]).ToString() + " times\n";
			outputString += "\n";

			outputString += "Highest rated review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MAX(Review.rating) FROM Review WHERE MONTH(Review.dateReviewed) = " + currMonth + ")");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Most critical review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, Review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MIN(Review.rating) FROM Review WHERE MONTH(Review.dateReviewed) = " + currMonth + ")");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Number of rentals per customer:\n";
			result = SQL.GetOutput("SELECT Rental.customerUsername, COUNT(Rental.id) FROM Rental WHERE MONTH(Rental.startDate) = " + currMonth + " GROUP BY Rental.customerUsername ORDER BY COUNT(Rental.id) DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + " rented " + ((int)values[1]).ToString() + " games\n";
			outputString += "\n";

			outputString += "\nALL-TIME STATS\n\n";

			outputString += "Boardgame popularity:\n";
			result = SQL.GetOutput("SELECT Boardgame.name, 'Times rented' = COUNT(Rental.boardgameID) FROM Boardgame, Rental WHERE Boardgame.id = Rental.boardgameID GROUP BY Boardgame.name ORDER BY 'Times rented' DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + ", rented " + ((int)values[1]).ToString() + " times\n";
			outputString += "\n";

			outputString += "Highest rated review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MAX(Review.rating) FROM Review)");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Most critical review(s):\n";
			result = SQL.GetOutput("SELECT Boardgame.name, Review.customerUsername, Review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Boardgame.id = Review.boardgameID AND Review.rating IN (SELECT MIN(Review.rating) FROM Review)");
			foreach (object[] values in result)
				outputString += (string)values[0] + " reviewed by " + (string)values[1] + " on " + Global.DateToString((DateTime)values[2]) + " scored " + ((int)values[3]).ToString() + "\n";
			outputString += "\n";

			outputString += "Number of rentals per customer:\n";
			result = SQL.GetOutput("SELECT Rental.customerUsername, COUNT(Rental.id) FROM Rental GROUP BY Rental.customerUsername ORDER BY COUNT(Rental.id) DESC");
			foreach (object[] values in result)
				outputString += (string)values[0] + " rented " + ((int)values[1]).ToString() + " games\n";
			outputString += "\n";

			new ReportWindow(outputString).ShowDialog();
		}
	}
}
