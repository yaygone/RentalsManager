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
using System.Windows.Forms.DataVisualization.Charting;

namespace RentalsManager
{
	/// <summary>
	/// Interaction logic for ReportWindow.xaml
	/// </summary>
	public partial class ReportWindow : Window
	{
		public ReportWindow(object item)
		{
			InitializeComponent();

			Chart chart = FindName("winFormsChart") as Chart;
			string outputString = "";
			List<object[]> result;
			if (item is Game game)
			{
				textBlockChartDesc.Text = "Game rental record for each month";

				Dictionary<int, int> rentalRecord = new Dictionary<int, int>();
				List<object[]> output = SQL.GetOutput("SELECT MONTH(startDate), COUNT(boardgameID) FROM Rental WHERE boardgameID = " + game.id + " GROUP BY MONTH(startDate) ORDER BY MONTH(startDate)");
				if (output.Count != 0)
					foreach (object[] values in output)
						rentalRecord.Add((int)values[0], (int)values[1]);
				else rentalRecord.Add(0, 0);
				chart.DataSource = rentalRecord;
				chart.Series[0].XValueMember = "Key";
				chart.Series[0].YValueMembers = "Value";

				outputString += "Currently loaned to:\n";
				outputString += "Username\n";
				result = SQL.GetOutput("SELECT Rental.customerUsername WHERE Rental.boardgameID = " + game.id + " AND Rental.returnDate IS NULL");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\n";
				outputString += "\n";

				outputString += "Rental history:\n";
				outputString += "Username\t\tRented on\n";
				result = SQL.GetOutput("SELECT Rental.customerUsername, Rental.startDate FROM Rental WHERE Rental.boardgameID = '" + game.id + "' AND Rental.returnDate IS NOT NULL ORDER BY Rental.startDate DESC");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + Global.DateToString((DateTime)values[1]) + "\n";
				outputString += "\n";

				outputString += "Highest rated review of this game:\n";
				outputString += "Username\t\tDate reviewed\t\tRating\n";
				result = SQL.GetOutput("SELECT Review.customerUsername, Review.dateReviewed, Review.rating FROM Review WHERE Review.rating IN (SELECT MAX(Review.rating) FROM Review WHERE Review.boardgameID = " + game.id + ")");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + Global.DateToString((DateTime)values[1]) + "\t\t" + ((int)values[2]).ToString() + "\n";
				outputString += "\n";

				outputString += "Most critical review of this game:\n";
				outputString += "Username\t\tDate reviewed\t\tRating\n";
				result = SQL.GetOutput("SELECT Review.customerUsername, Review.dateReviewed, Review.rating FROM Review WHERE Review.rating IN (SELECT MIN(Review.rating) FROM Review WHERE Review.boardgameID = " + game.id + ")");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + Global.DateToString((DateTime)values[1]) + "\t\t" + ((int)values[2]).ToString() + "\n";
				outputString += "\n";
			}
			else if (item is Customer customer)
			{
				textBlockChartDesc.Text = "Customer rental record for each month";

				Dictionary<int, int> rentalRecord = new Dictionary<int, int>();
				List<object[]> output = SQL.GetOutput("SELECT MONTH(startDate), COUNT(customerUsername) FROM Rental WHERE customerUsername = '" + customer.username + "' GROUP BY MONTH(startDate) ORDER BY MONTH(startDate)");
				if (output.Count != 0)
					foreach (object[] values in output)
						rentalRecord.Add((int)values[0], (int)values[1]);
				else rentalRecord.Add(0, 0);
				chart.DataSource = rentalRecord;
				chart.Series[0].XValueMember = "Key";
				chart.Series[0].YValueMembers = "Value";

				outputString += "Current loans for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Rented on\t\tGame\n";
				result = SQL.GetOutput("SELECT Rental.startDate, Boardgame.name FROM Rental, Boardgame WHERE Rental.customerUsername = '" + customer.username + "' AND Rental.boardgameID = Boardgame.id AND Rental.returnDate IS NULL ORDER BY Rental.startDate DESC");
				foreach (object[] values in result)
					outputString += Global.DateToString((DateTime)values[0]) + "\t\t" + (string)values[1] + "\n";
				outputString += "\n";

				outputString += "Rental record for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Rented on\t\tReturned on\t\tGame\n";
				result = SQL.GetOutput("SELECT Rental.startDate, Rental.returnDate, Boardgame.name FROM Rental, Boardgame WHERE Rental.customerUsername = '" + customer.username + "' AND Rental.boardgameID = Boardgame.id AND Rental.returnDate IS NOT NULL ORDER BY Rental.startDate DESC");
				foreach (object[] values in result)
					outputString += Global.DateToString((DateTime)values[0]) + "\t\t" + (values[1] is DBNull ? "Not returned" : Global.DateToString((DateTime) values[1]) ) + "\t\t" + (string)values[2] + "\n";
				outputString += "\n";

				outputString += "Review history for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Game\t\tReview date\t\tScore\n";
				result = SQL.GetOutput("SELECT Boardgame.name, Review.dateReviewed, Review.rating FROM Boardgame, Review WHERE Review.customerUsername = '" + customer.username + "' AND Boardgame.id = Review.boardgameID ORDER BY Review.DateReviewed DESC");
				foreach (object[] values in result)
					outputString += (string) values[0] + "\t\t" + (values[1] is DBNull ? "Unknown" : Global.DateToString((DateTime)values[1])) + "\t\t" + ((int)values[2]).ToString() + "\n";
				outputString += "\n";

				outputString += "Favourite publishers for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Publisher\t\tTimes rented\n";
				result = SQL.GetOutput("SELECT GamePublishedBy.publisherName, COUNT(GamePublishedBy.boardgameID) FROM GamePublishedBy, Boardgame, Rental WHERE Boardgame.id = GamePublishedBy.boardgameID AND Boardgame.id = Rental.boardgameID AND Rental.customerUsername = '" + customer.username + "' GROUP BY GamePublishedBy.publisherName ORDER BY COUNT(GamePublishedBy.boardgameID) DESC");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + ((int)values[1]).ToString() + "\n";
				outputString += "\n";

				outputString += "Favourite designers for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Designer\t\tTimes rented\n";
				result = SQL.GetOutput("SELECT GameDesignedBy.designerName, COUNT(GameDesignedBy.boardgameID) FROM GameDesignedBy, Boardgame, Rental WHERE Boardgame.id = GameDesignedBy.boardgameID AND Boardgame.id = Rental.boardgameID AND Rental.customerUsername = '" + customer.username + "' GROUP BY GameDesignedBy.designerName ORDER BY COUNT(GameDesignedBy.boardgameID) DESC");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + ((int)values[1]).ToString() + "\n";
				outputString += "\n";

				outputString += "Favourite genres for " + customer.fname + " " + customer.lname + ":\n";
				outputString += "Genre\t\tTimes rented\n";
				result = SQL.GetOutput("SELECT GameGenreValue.genreName, COUNT(GameGenreValue.boardgameID) FROM GameGenreValue, Boardgame, Rental WHERE Boardgame.id = GameGenreValue.boardgameID AND Boardgame.id = Rental.boardgameID AND Rental.customerUsername = '" + customer.username + "' GROUP BY GameGenreValue.genreName ORDER BY COUNT(GameGenreValue.boardgameID) DESC");
				foreach (object[] values in result)
					outputString += (string)values[0] + "\t\t" + ((int)values[1]).ToString() + "\n";
				outputString += "\n";
			}
			else if (item is Manager manager)
			{
				textBlockChartDesc.Text = "Game rental approvals for each month";

				Dictionary<int, int> rentalRecord = new Dictionary<int, int>();
				List<object[]> output = SQL.GetOutput("SELECT MONTH(startDate), COUNT(managerUsername) FROM Rental WHERE managerUsername = '" + manager.username + "' GROUP BY MONTH(startDate) ORDER BY MONTH(startDate)");
				if (output.Count != 0)
					foreach (object[] values in output)
						rentalRecord.Add((int)values[0], (int)values[1]);
				else rentalRecord.Add(0, 0);
				chart.DataSource = rentalRecord;
				chart.Series[0].XValueMember = "Key";
				chart.Series[0].YValueMembers = "Value";

				outputString += "Rentals approved by " + manager.username + ":\n";
				outputString += "Rented on\t\tCustomer\t\tGame\n";
				result = SQL.GetOutput("SELECT Rental.startDate, Rental.customerUsername, Boardgame.name FROM Rental, Boardgame WHERE Rental.managerUsername = '" + manager.username + "' AND Rental.boardgameID = Boardgame.id AND Rental.returnDate IS NULL ORDER BY Rental.startDate DESC");
				foreach (object[] values in result)
					outputString += Global.DateToString((DateTime)values[0]) + "\t\t" + (string) values[1] + "\t\t" + (string)values[2] + "\n";
				outputString += "\n";
			}

			textBoxContent.Text = outputString;
		}
		public ReportWindow(string content)
		{
			InitializeComponent();
			textBoxContent.Text = content;

			Chart chart = FindName("winFormsChart") as Chart;

			textBlockChartDesc.Text = "Total rentals each month";

			Dictionary<int, int> rentalRecord = new Dictionary<int, int>();
			List<object[]> output = SQL.GetOutput("SELECT MONTH(startDate), COUNT(managerUsername) FROM Rental GROUP BY MONTH(startDate) ORDER BY MONTH(startDate)");
			if (output.Count != 0)
				foreach (object[] values in output)
					rentalRecord.Add((int)values[0], (int)values[1]);
			else rentalRecord.Add(0, 0);
			chart.DataSource = rentalRecord;
			chart.Series[0].XValueMember = "Key";
			chart.Series[0].YValueMembers = "Value";

		}

		private void ButtonClose_Click(object sender, RoutedEventArgs e)
		{ Close(); }

	}
}
