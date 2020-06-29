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
	/// Interaction logic for ReturnAssessmentWindow.xaml
	/// </summary>
	public partial class ReturnAssessmentWindow : Window
	{
		Game game;
		Customer customer;
		int rentalID;
		public ReturnAssessmentWindow(Game game)
		{
			InitializeComponent();
			this.game = game;
			PopulateFields();
		}

		private void PopulateFields()
		{
			object[] output = SQL.GetOutput("SELECT * FROM Rental WHERE boardgameID = " + game.id + " AND returnDate IS NULL")[0];
			rentalID = (int) output[0];
			IdTextBlock.Text += rentalID.ToString();
			TextBoxGameName.Text = game.name;
			foreach (Customer customer in Global.customers)
				if (customer.username.Equals((string)output[4])) this.customer = customer;
			TextBoxCustomerUsername.Text = customer.username;
			TextBoxManagerUsername.Text = (string) output[5];
			TextBoxStartDate.Text = Global.DateToString((DateTime) output[1]);
			DatePickerReturnDate.SelectedDate = DateTime.Today;
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				int penalty = int.Parse(TextBoxPenalty.Text);
				customer.goodness = (customer.goodness < penalty) ? 0 : customer.goodness - penalty;
				SQL.ExecuteQuery("UPDATE Rental SET returnDate = " + ((DateTime)DatePickerReturnDate.SelectedDate).ToShortDateString() + " WHERE id = " + rentalID);
				SQL.ExecuteQuery("UPDATE Rental SET condition = " + TextBoxReturnCondition.Text + " WHERE id = " + rentalID);
				customer.maxRentNum++;
				customer.UpdateSql();
				game.avail = true;
				game.UpdateSql();
				if (MessageBox.Show("Return accepted. Write a review?", "Information", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
					new ReviewDetailWindow(game, customer).ShowDialog();
				Close();
			}
			catch (Exception exception)
			{
				MessageBox.Show("Error occured. Please check the input format.", "Error", MessageBoxButton.OK);
				Console.WriteLine(exception.StackTrace);
			}
		}
	}
}
