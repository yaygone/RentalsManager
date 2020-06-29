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
	/// Interaction logic for ReviewDetailWindow.xaml
	/// </summary>
	public partial class ReviewDetailWindow : Window
	{
		private Review review;

		public ReviewDetailWindow(Review review, bool readOnly)
		{
			InitializeComponent();
			this.review = review;
			if (review != null) PopulateFields(readOnly);
		}

		public ReviewDetailWindow(Game game, Customer customer)
		{
			InitializeComponent();
			TextBoxCustomerUsername.Text = customer.username;
			TextBoxBoardgameId.Text = game.id.ToString();
		}

		private void PopulateFields(bool readOnly)
		{
			TextBoxCustomerUsername.Text = review.customer.username;
			TextBoxBoardgameId.Text = review.boardgame.id.ToString();
			TextBoxCustomerUsername.IsEnabled = false;
			TextBoxBoardgameId.IsEnabled = false;
			TextBoxContent.Text = review.content;
			switch (review.rating)
			{
				case 1:
					rate1.IsChecked = true;
					break;
				case 2:
					rate2.IsChecked = true;
					break;
				case 3:
					rate3.IsChecked = true;
					break;
				case 4:
					rate4.IsChecked = true;
					break;
				case 5:
					rate5.IsChecked = true;
					break;
				case 6:
					rate6.IsChecked = true;
					break;
				case 7:
					rate7.IsChecked = true;
					break;
				case 8:
					rate8.IsChecked = true;
					break;
				case 9:
					rate9.IsChecked = true;
					break;
				case 10:
					rate10.IsChecked = true;
					break;
				default:
					break;
			}
			ButtonSave.IsEnabled = !readOnly;
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				Customer newCustomer = null;
				Game newGame = null;
				bool newRecord = (review == null);
				if (newRecord)
				{
					foreach (Customer customer in Global.customers)
						if (customer.username.Equals(TextBoxCustomerUsername.Text))
							newCustomer = customer;
					if (newCustomer == null)
					{
						MessageBox.Show("User not found, please insert a valid username", "Error", MessageBoxButton.OK);
						return;
					}

					foreach (Game game in Global.games)
						if (game.id == int.Parse(TextBoxBoardgameId.Text))
							newGame = game;
					if (newGame == null)
					{
						MessageBox.Show("Game not found, please insert a valid id", "Error", MessageBoxButton.OK);
						return;
					}

					review = new Review(newCustomer, newGame);
				}

				review.content = TextBoxContent.Text;
				review.rating = (bool)rate1.IsChecked ? 1 : (bool)rate2.IsChecked ? 2 : (bool)rate3.IsChecked ? 3 
					: (bool)rate4.IsChecked ? 4 : (bool)rate5.IsChecked ? 5 : (bool)rate6.IsChecked ? 6 
				: (bool)rate7.IsChecked ? 7 : (bool)rate8.IsChecked ? 8 : (bool)rate9.IsChecked ? 9 : 10;
				if (review.rating == 10 && !(bool)rate10.IsChecked) throw new MissingFieldException();

				if (newRecord) review.InsertSql();
				else review.UpdateSql();
				if (newGame != null) newGame.UpdateSql();
				Close();
			}
			catch (FormatException exception)
			{
				MessageBox.Show("Error processing inputs. Please check that no letters are present in numeric field", "Error", MessageBoxButton.OK);
				Console.WriteLine(exception.StackTrace);
			}
			catch (MissingFieldException exception)
			{
				MessageBox.Show("Error processing inputs. Please select a rating", "Error", MessageBoxButton.OK);
				Console.WriteLine(exception.StackTrace);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.StackTrace);
			}
		}
	}
}
