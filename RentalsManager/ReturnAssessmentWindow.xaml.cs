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
		public ReturnAssessmentWindow(Game game, Customer customer)
		{
			this.game = game;
			this.customer = customer;
			PopulateFields();
			InitializeComponent();
		}

		private void PopulateFields()
		{
			object[] output = SQL.GetOutput("SELECT * FROM Rental WHERE " + ((game == null) ? ("customerUsername = " + customer.username) : ("boardgameID = " + game.id)))[0];
			IdTextBlock.Text = "Processing return for rental #" + (string) output[0];
			TextBoxGameName.Text = game.name;
			TextBoxCustomerUsername.Text = customer.username;
			TextBoxManagerUsername.Text = (string) output[5];
			TextBoxStartDate.Text = (string) output[1];
		}

		private void ButtonCancel_Click(object sender, RoutedEventArgs e)
		{ Close(); }

		private void ButtonSave_Click(object sender, RoutedEventArgs e)
		{
			try
			{

			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.StackTrace);
			}
		}
	}
}
