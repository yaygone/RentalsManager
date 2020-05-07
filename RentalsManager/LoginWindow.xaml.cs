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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace RentalsManager
{
	/// <summary>
	/// Interaction logic for LoginWindow.xaml
	/// </summary>
	public partial class LoginWindow : Window
	{
		public LoginWindow()
		{ InitializeComponent(); }

		private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text;
			string password = PasswordPwBox.Password;
			if (ManagerExists(username, password)) new MainViewWindow().Show();
			else new PopupWindow("Incorrect username and/or password entered.", this).Show();
		}

		private bool ManagerExists(string username, string password)
		{
			Console.WriteLine("Finding manager record...");
			int[] retrieveIndex = { 0 };
			List<string[]> managerList = SQL.GetOutput("SELECT password FROM Manager WHERE username = '" + username + "'", retrieveIndex);
			
			if (managerList.Count != 0 && managerList[0][0].Equals(password)) return true;
			return false;
		}

		private void UsernameTextBox_KeyUp(object sender, KeyEventArgs e)
		{ if (e.Key == Key.Enter) PasswordPwBox.Focus(); }

		private void GoToButtonClick(object sender, KeyEventArgs e)
		{ if (e.Key == Key.Enter) ButtonBase_OnClick(sender, e); }
	}

}
